# 🔗 שלב 3: מימוש וטעינת קשרי גומלין בשכבת השירות (`SQLiteNetExtensions.Async`)

בשלב 1 הגדרנו את קשרי הגומלין במודלים (`[OneToMany]`, `[ManyToOne]`, `[ManyToMany]`), ובשלב 2 בנינו CRUD בסיסי עם `InitDBAsync()`.  
כעת נפגוש תופעה חשובה: כאשר מריצים `_database!.Table<Course>().ToListAsync()`, העמודה `TeacherId` חוזרת עם המספר `1`, **אבל המאפיין `course.Teacher` נשאר `null`!**  
כדי למלא אוטומטית את מאפייני הניווט (הילדים וההורים), נשתמש בהרחבות של **`SQLiteNetExtensions.Async`**.

הוסיפו בראש `PlayWithSQLITE/Services/DatabaseService.cs`:
```csharp
using SQLiteNetExtensionsAsync.Extensions;
```

> [!NOTE]
> **תזכורת מהירה — מהו סימן הקריאה (`!`) בביטוי `_database!.` ("ה-Bang")?**  
> השדה `_database` הוגדר כטיפוס שיכול להיות `null` (`SQLiteAsyncConnection?`).  
> ברגע שאנחנו קוראים בתחילת הפעולה ל-`await InitDBAsync();`, החיבור כבר פתוח ובוודאות אינו `null`.  
> האופרטור **`!`** (Null-Forgiving Operator) אומר למהדר של C#: *"אני מתחייב ש-`_database` אינו `null` בנקודה זו — אל תציג אזהרת קומפילציה"*.

---

## 🧰 טבלת הפעולות שחבילת ה-NuGet (`SQLiteNetExtensions.Async`) מספקת לנו

כל הפעולות הבאות הופכות זמינות על `_database` לאחר ייבוא `using SQLiteNetExtensionsAsync.Extensions;`:

| פעולה (`Extension Method`) | מה היא מספקת / מחזירה? | דוגמת קריאה |
| :--- | :--- | :--- |
| `GetWithChildrenAsync<T>(id)` | שולפת ישות יחידה לפי `PrimaryKey` (`id`) וממלאת אוטומטית את כל מאפייני הניווט שלה (`[OneToMany]`, `[ManyToOne]`, `[ManyToMany]`). מחזירה `Task<T>` *(שימו לב: זורקת `InvalidOperationException` אם המזהה לא קיים!)* | `var teacher = await _database!.GetWithChildrenAsync<Teacher>(id);` |
| `GetAllWithChildrenAsync<T>()` | שולפת את **כל** השורות בטבלה `T` וממלאת עבור כל אחת מהן את ההורים/הילדים המקושרים. מחזירה `Task<List<T>>`. | `var courses = await _database!.GetAllWithChildrenAsync<Course>();` |
| `InsertWithChildrenAsync(obj)` *(העשרה)* | שומרת ישות חדשה **וגם** את ישויות הילדים שלה שמסומנות ב-`CascadeInsert` / `All` בפעולה אחת (`Task`). | `await _database!.InsertWithChildrenAsync(teacherWithCourses);` |
| `UpdateWithChildrenAsync(obj)` *(העשרה)* | מעדכנת את המפתחות הזרים וטבלאות הגישור לפי השינויים שבוצעו ברשימות הניווט של האובייקט (`Task`). | `await _database!.UpdateWithChildrenAsync(student);` |
| `DeleteAsync(obj, recursive: true)` *(העשרה)* | מוחקת את הישות **וגם** רקורסיבית את הילדים המסומנים ב-`CascadeDelete` *(זהירות בקשר `M:N` — עדיפה מחיקה מבוקרת כמו בדוגמה 4!)* | `await _database!.DeleteAsync(teacher, recursive: true);` |
| `.FirstOrDefaultAsync(e => ... && ...)` *(מ-`sqlite-net-pcl`)* | מאתרת רשומת גישור יחידה לפי שני מפתחות זרים יחד (`StudentId` ו-`CourseId`) למניעת כפילות. מחזירה `Task<Enrollment?>`. | `await _database!.Table<Enrollment>().FirstOrDefaultAsync(e => e.StudentId == sid && e.CourseId == cid);` |

---

## חלק 1: ניתוח 4 דוגמאות מלאות (להעתקה והטמעה ב-`DatabaseService.cs`)

להלן 4 דוגמאות מפורקות שלב-אחר-שלב המכסות את כל סוגי הקשרים במערכת.  
**קראו את הניתוח והעתיקו את 4 הפעולות הללו אל החלקים המתאימים ב-`DatabaseService.cs` שלכם כדי שתוכלו להריץ ולאמת, ולאחר מכן ממשו בעצמכם את הפעולות המקבילות בחלק 2!**

### דוגמה 1 — שליפת ישות אחת עם ילדיה (`1:N`): `GetTeacherByIdAsync`
- **מה הפעולה מקבלת?** `int id` (מזהה המורה) ו-`bool includeCourses = false` (האם לטעון גם את רשימת הקורסים שלו?).
- **מה היא מחזירה?** `Task<Teacher?>` (המורה עם רשימת `Courses` מלאה אם התבקש, או `null` אם המזהה לא קיים).
- **ניתוח שלב-אחר-שלב:**
  1. מוודאים אתחול פנימי (`await InitDBAsync()`).
  2. אם `includeCourses == true`: קוראים ל-`GetWithChildrenAsync<Teacher>(id)`. מכיוון ששיטה זו זורקת `InvalidOperationException` כאשר ה-`id` אינו קיים בטבלה, עוטפים אותה ב-`try / catch` ומחזירים `null`.
  3. אחרת: מבצעים שליפה רגילה ומהירה (`FirstOrDefaultAsync`).

```csharp
public async Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false)
{
    await InitDBAsync();
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

### דוגמה 2 — שליפת רשימת ישויות עם הוריהן וילדיהן (`N:1` + `M:N`): `GetCoursesAsync`
- **מה הפעולה מקבלת?** `bool includeRelations = false` (כאשר `true`, נטען עבור כל קורס את אובייקט `Teacher` ואת רשימת `Students`).
- **מה היא מחזירה?** `Task<List<Course>>` ממוינת לפי שם הקורס (`Title`).
- **ניתוח שלב-אחר-שלב:**
  1. מוודאים אתחול (`InitDBAsync`).
  2. אם `includeRelations == true`: קוראים ל-`GetAllWithChildrenAsync<Course>()` וממיינים את הרשימה בזיכרון (`OrderBy(c => c.Title).ToList()`).
  3. אחרת: מבצעים שליפת `Table<Course>()` פשוטה וממוינת.

```csharp
public async Task<List<Course>> GetCoursesAsync(bool includeRelations = false)
{
    await InitDBAsync();
    if (includeRelations)
    {
        var courses = await _database!.GetAllWithChildrenAsync<Course>();
        return courses.OrderBy(c => c.Title).ToList();
    }

    return await _database!.Table<Course>().OrderBy(c => c.Title).ToListAsync();
}
```

### דוגמה 3 — קשר `M:N` עם **מניעת כפילות רישום**: `EnrollStudentAsync`
- **מה הפעולה מקבלת?** `int studentId`, `int courseId`, וציון אופציונלי `double? grade = null`.
- **מה היא מחזירה?** `Task<int>` (מספר השורות שנוספו או עודכנו).
- **מדוע צריך בדיקת כפילות?** אם תלמיד מסוים כבר רשום לקורס "תכנות C#", איננו רוצים שיהיו לו שתי שורות כפולות בטבלת `Enrollments`!
- **ניתוח שלב-אחר-שלב:**
  1. מוודאים אתחול (`InitDBAsync`).
  2. מחפשים בטבלת `Enrollment` האם כבר קיימת שורה שבה **גם** `StudentId == studentId` **וגם** `CourseId == courseId`.
  3. **אם נמצאה שורה (`existing != null`):** מעדכנים לה את הציון (`existing.Grade = grade`) ומחזירים `await _database.UpdateAsync(existing)`.
  4. **אם לא נמצאה שורה:** יוצרים אובייקט `Enrollment` חדש (`EnrollmentDate = DateTime.Now`, `IsPaid = true`) ומחזירים `await _database.InsertAsync(enrollment)`.

```csharp
public async Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null)
{
    await InitDBAsync();

    // בדיקה למניעת כפילות רישום של אותו תלמיד לאותו קורס
    var existing = await _database!.Table<Enrollment>()
        .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

    if (existing != null)
    {
        existing.Grade = grade;
        return await _database.UpdateAsync(existing);
    }

    var enrollment = new Enrollment
    {
        StudentId = studentId,
        CourseId = courseId,
        Grade = grade,
        EnrollmentDate = DateTime.Now,
        IsPaid = true
    };
    return await _database.InsertAsync(enrollment);
}
```

### דוגמה 4 — מחיקה מבוקרת המונעת רשומות יתומות (`Orphan Records`): `DeleteCourseAsync`
- **מה הפעולה מקבלת?** `Course course` למחיקה.
- **מה המטרה?** למחוק קודם את כל שורות ה-`Enrollment` ששייכות לקורס הזה (כדי שלא יישארו רישומים המצביעים על קורס שנמחק), ורק אז למחוק את הקורס עצמו — **מבלי למחוק את התלמידים עצמם!**
```csharp
public async Task<int> DeleteCourseAsync(Course course)
{
    await InitDBAsync();

    // 1. איתור ומחיקת כל הרישומים המצביעים על הקורס
    var enrollments = await _database!.Table<Enrollment>()
        .Where(e => e.CourseId == course.Id)
        .ToListAsync();
    foreach (var enrollment in enrollments)
    {
        await _database.DeleteAsync(enrollment);
    }

    // 2. מחיקת הקורס עצמו
    return await _database.DeleteAsync(course);
}
```

---

## חלק 2: ✍️ משימתכם המעשית בשלב 3 (שדרוג והשלמת הפעולות המקבילות)

כעת, לפי 4 הדוגמאות המלאות למעלה, שדרגו והשלימו ב-`DatabaseService.cs` את 7 הפעולות הבאות:

| מתודה לשדרוג / מימוש | קלט (`Parameters`) | מה עליה לבצע ולהחזיר? | לאיזו דוגמה היא מקבילה? |
| :--- | :--- | :--- | :--- |
| **`GetCourseByIdAsync`** | `int id, bool includeRelations = true` | הפעולה מקבלת מזהה קורס ודגל האם לכלול נתונים קשורים, ומחזירה את הקורס שהמזהה שלו שווה ל-`id` ובהתאם לדגל האם לטעון גם את הרשומות הקשורות אליו (המורה והתלמידים). אם לא נמצא קורס עם המזהה המבוקש על הפעולה להחזיר `null`. *(שימו לב: פעולת הגישה למסד הנתונים עם נתונים קשורים עלולה לזרוק חריגה — Exception — ולכן יש לטפל בכך).* | דוגמה 1 (`GetTeacherByIdAsync`) |
| **`GetStudentByIdAsync`** | `int id, bool includeCourses = false` | הפעולה מקבלת מזהה תלמיד ודגל האם לכלול את הקורסים שלו, ומחזירה את התלמיד שהמזהה שלו שווה ל-`id` ובהתאם לדגל האם לכלול גם את רשימת הקורסים שאליהם הוא רשום. אם לא נמצא תלמיד עם המזהה המבוקש על הפעולה להחזיר `null`. *(שימו לב: גם כאן טעינה עם נתונים קשורים עלולה לזרוק חריגה כאשר המזהה לא קיים).* | דוגמה 1 (`GetTeacherByIdAsync`) |
| **`GetEnrollmentsAsync`** | ללא | הפעולה אינה מקבלת פרמטרים; היא מוודאת שהמסד מאותחל ומחזירה את רשימת כל רשומות הרישום הקיימות במסד הנתונים, כך שכל רשומת רישום תכלול בתוכה גם את פרטי התלמיד המלאים וגם את פרטי הקורס המלאים המקושרים אליה. | דוגמה 2 (`GetCoursesAsync`) |
| **`UpdateEnrollmentGradeAsync`** | `int enrollmentId, double grade` | הפעולה מקבלת מזהה רישום וציון חדש. היא מאתרת במסד את רשומת הרישום שהמזהה שלה תואם ל-`enrollmentId` — אם לא נמצאה רשומה כזו על הפעולה להחזיר `0`; אחרת עליה לעדכן את הציון של אותה רשומה, לשמור את השינוי במסד הנתונים ולהחזיר את מספר השורות שעודכנו. | דוגמה 3 (בדיקת קיום + שמירה) |
| **`RemoveEnrollmentAsync`** | `int studentId, int courseId` | הפעולה מקבלת מזהה תלמיד ומזהה קורס. היא מחפשת את רשומת הרישום שמחברת בין אותו תלמיד לאותו קורס — אם נמצאה רשומה כזו עליה למחוק אותה ממסד הנתונים ולהחזיר את מספר השורות שנמחקו; אם לא נמצא רישום תואם על הפעולה להחזיר `0`. | דוגמה 3 (איתור לפי זוג מפתחות זרים) |
| **`DeleteStudentAsync`** | `Student student` | הפעולה מקבלת אובייקט תלמיד למחיקה. כדי למנוע רשומות יתומות, עליה לאתר ולמחוק תחילה את כל רשומות הרישום השייכות לאותו תלמיד (מבלי למחוק את הקורסים עצמם!), ורק לאחר מכן למחוק את התלמיד עצמו ממסד הנתונים ולהחזיר את מספר השורות שנמחקו. | דוגמה 4 (`DeleteCourseAsync`) |
| **`DeleteTeacherAsync`** | `Teacher teacher` | הפעולה מקבלת אובייקט מורה למחיקה. כדי למנוע רשומות יתומות, עליה לאתר תחילה את כל הקורסים שהמורה מלמד ולמחוק כל אחד מהם בצורה מסודרת (כך שגם רשומות הרישום של אותם קורסים ינוקו), ורק בסוף למחוק את המורה עצמו ולהחזיר את מספר השורות שנמחקו. | דוגמה 4 (`DeleteCourseAsync`) |

### ✅ בדיקה עצמית בסיום שלב 3:
1. פתחו את `DatabaseServiceTests.cs` והסירו את `Skip = "..."` משתי הבדיקות:
   - **`GetCoursesAsync_WithRelations_LoadsTeacherDetails`**
   - **`EnrollStudentAsync_CreatesNewEnrollment`**
2. הריצו את הבדיקות ב-**Test Explorer** (VS) או ב-**Unit Tests** (Rider) (ראו [00b-How-To-Run-Tests.pdf](./00b-How-To-Run-Tests.pdf)) וודאו ש-**4 בדיקות עוברות בירוק (`succeeded: 4, failed: 0`)**!
3. להשוואה מול הפתרון המוכן: `git checkout step-03-relationships`.

עברו לשלב הבא: [04-Advanced-Queries-Aggregations.pdf](./04-Advanced-Queries-Aggregations.pdf).
