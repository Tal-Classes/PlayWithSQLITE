# 🔗 שלב 3: מימוש וטעינת קשרי גומלין בשכבת השירות (`SQLiteNetExtensions.Async`)

בשלב 1 תכננו את תרשים ה-**ERD**, למדנו מה מוגדר בכל צד של הקשר (`1:N` ו-`M:N`), בחרנו `CascadeOperation` מתאים וקישטנו את 4 המודלים.  
בשלב 2 פתחנו חיבור אסינכרוני דרך `Constants` ובנינו את פעולות ה-CRUD הבסיסיות.

כעת, בשלב 3, נפגוש בעיה מעשית חשובה:  
**אם תריצו `await _database.Table<Course>().ToListAsync()`, השדה `course.Teacher` יחזור כ-`null` ורשימת `course.Students` תחזור ריקה!**  
מדוע? כי `sqlite-net-pcl` הבסיסי שולף עמודות מטבלה אחת בלבד ואינו מכיר קשרי גומלין. כאן נכנסת לפעולה חבילת **`SQLiteNetExtensions.Async`**!

---

## 1. שיטות הטעינה עם ילדים (`GetWithChildrenAsync` ו-`GetAllWithChildrenAsync`)

הוסיפו בראש `DatabaseService.cs` את מרחב השמות של מתודות ההרחבה האסינכרוניות:
```csharp
using SQLiteNetExtensionsAsync.Extensions;
```

מרחב שמות זה מוסיף למחלקת `SQLiteAsyncConnection` את מתודות ההרחבה הבאות:

| מתודה | מה היא מבצעת? | היכן נשתמש בה ב-`DatabaseService`? |
| :--- | :--- | :--- |
| **`GetWithChildrenAsync<T>(id)`** | שולפת ישות יחידה לפי `PrimaryKey`, וקוראת אוטומטית את כל מאפייני הניווט שלה (`[OneToMany]`, `[ManyToOne]`, `[ManyToMany]`)! | `GetTeacherByIdAsync(id, includeCourses: true)`<br/>`GetCourseByIdAsync(id, includeRelations: true)`<br/>`GetStudentByIdAsync(id, includeCourses: true)` |
| **`GetAllWithChildrenAsync<T>()`** | שולפת את כל הרשומות בטבלה וממלאת את מאפייני הניווט עבור כל אחת מהן! | `GetCoursesAsync(includeRelations: true)`<br/>`GetEnrollmentsAsync()` |

### א. שליפת מורה יחד עם כל הקורסים שלו (`1:N`)
כאשר ה-ViewModel של מסך המורים מבקש להציג כמה קורסים מלמד כל מורה (`includeCourses = true`), נטפל גם במקרה שהמזהה אינו קיים (שימו לב: `GetWithChildrenAsync` עלול לזרוק `InvalidOperationException` אם ה-`id` לא נמצא, ולכן נגן עליו ב-`try / catch` או נבדוק קודם קיום):
```csharp
public async Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false)
{
    await EnsureInitializedAsync();
    if (includeCourses)
    {
        try
        {
            return await _database!.GetWithChildrenAsync<Teacher>(id);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    return await _database!.Table<Teacher>().FirstOrDefaultAsync(t => t.Id == id);
}
```

### ב. שליפת קורסים יחד עם פרטי המורה (`ManyToOne`) והתלמידים (`ManyToMany`)
במסך הקורסים (`CoursesPage`), כל כרטיס קורס מציג את `Course.TeacherName` (הנשען על `Teacher?.FullName`).  
כאשר `includeRelations == true`:
```csharp
public async Task<List<Course>> GetCoursesAsync(bool includeRelations = false)
{
    await EnsureInitializedAsync();
    if (includeRelations)
    {
        var courses = await _database!.GetAllWithChildrenAsync<Course>();
        return courses.OrderBy(c => c.Title).ToList();
    }

    return await _database!.Table<Course>().OrderBy(c => c.Title).ToListAsync();
}
```

---

## 2. ניהול טבלת הגישור `Enrollments` (קשר `M:N` עם מידע נוסף)

מכיוון שטבלת `Enrollments` שומרת לא רק את החיבור בין `StudentId` ל-`CourseId` אלא גם **ציון (`Grade`)**, **תאריך רישום** ו**סטטוס תשלום**, ננהל אותה דרך 4 פעולות מפורשות ב-`DatabaseService`:

1. **`GetEnrollmentsAsync()`**: טוענת את כל הרישומים בעזרת `GetAllWithChildrenAsync<Enrollment>()` כך שכל `Enrollment` יכיל גם את אובייקט ה-`Student` וגם את אובייקט ה-`Course`.
2. **`EnrollStudentAsync(int studentId, int courseId, double? grade = null)`**:
   - **מניעת כפילות רישום (Idempotency):** ראשית בודקים האם כבר קיימת שורה בטבלת `Enrollment` עבור אותו `(studentId, courseId)`.
   - אם כבר קיימת ➡️ מעדכנים את הציון (`existing.Grade = grade`) ומבצעים `UpdateAsync`.
   - אם לא קיימת ➡️ יוצרים `Enrollment` חדש ומבצעים `InsertAsync`.
3. **`UpdateEnrollmentGradeAsync(int enrollmentId, double grade)`**: שולפת את הרישום לפי `enrollmentId`, מעדכנת את `Grade` ושומרת ב-`UpdateAsync`.
4. **`RemoveEnrollmentAsync(int studentId, int courseId)`**: מאתרת את הרישום התואם ומוחקת אותו ב-`DeleteAsync`.

---

## 3. מניעת רשומות יתומות (`Orphan Records`) במחיקה

זוכרים את האזהרה משלב 1 לגבי `CascadeOperation`?  
ב-SQLite (בברירת מחדל), פקודת `DeleteAsync(teacher)` פשוטה מוחקת רק את השורה מטבלת `Teachers` — ומשאירה את כל הקורסים של אותו מורה "יתומים" עם `TeacherId` שמצביע על מורה שכבר אינו קיים!

לכן בשכבת השירות נוודא **שלמות התייחסותית (`Referential Integrity`)** בעת מחיקה:
- **`DeleteCourseAsync(course)`**: מוחקים תחילה את כל ה-`Enrollments` שבהם `CourseId == course.Id`, ורק אז מוחקים את הקורס עצמו.
- **`DeleteStudentAsync(student)`**: מוחקים תחילה את כל ה-`Enrollments` שבהם `StudentId == student.Id`, ורק אז מוחקים את התלמיד עצמו.
- **`DeleteTeacherAsync(teacher)`**: מוחקים תחילה את כל הקורסים של המורה (באמצעות `DeleteCourseAsync` שגם מנקה את הרישומים שלהם!), ורק אז מוחקים את המורה.

> [!IMPORTANT]
> **יתרון הגישה המבוקרת בשכבת השירות:**
> כשמוחקים קורס, אנו מוחקים רק את ה-`Enrollments` שלו — **ולעולם לא מוחקים בטעות את התלמידים עצמם** (סכנה שעלולה לקרות אם מפעילים מחיקה רקורסיבית עיוורת על קשר `M:N` עם `CascadeOperation.All`)!

---

## 4. משימתכם המעשית בשלב 3

1. הוסיפו `using SQLiteNetExtensionsAsync.Extensions;` בראש [DatabaseService.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE/Services/DatabaseService.cs).
2. שדרגו את `GetTeacherByIdAsync`, `GetCoursesAsync`, `GetCourseByIdAsync` ו-`GetStudentByIdAsync` לטעינת ילדים באמצעות `GetWithChildrenAsync` ו-`GetAllWithChildrenAsync`.
3. ממשו את 4 פעולות טבלת הגישור: `GetEnrollmentsAsync`, `EnrollStudentAsync`, `UpdateEnrollmentGradeAsync`, ו-`RemoveEnrollmentAsync`.
4. שדרגו את `DeleteTeacherAsync`, `DeleteCourseAsync` ו-`DeleteStudentAsync` כך שינקו רשומות תלויות וימנעו רשומות יתומות.

### ✅ בדיקה עצמית בסיום שלב 3:
- פתחו את [DatabaseServiceTests.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE.Tests/DatabaseServiceTests.cs), הסירו את `Skip` מהבדיקות `GetCoursesAsync_WithRelations_LoadsTeacherDetails` ו-`EnrollStudentAsync_CreatesNewEnrollment`, והריצו `dotnet test`!
- להשוואה מול הפתרון המוכן: `git checkout step-03-relationships`.

עברו לשלב הבא: [04-Advanced-Queries-Aggregations.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/04-Advanced-Queries-Aggregations.md).
