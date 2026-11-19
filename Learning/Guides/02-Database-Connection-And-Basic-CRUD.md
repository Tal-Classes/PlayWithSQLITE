# 🔌 שלב 2: מחלקת `Constants`, הבנת `IDatabaseService`, אתחול פנימי אוטומטי ומימוש CRUD שלב-אחר-שלב

בשלב זה נבנה את **שכבת הגישה למסד הנתונים** צעד אחר צעד:
1. נמלא את המחלקה הסטטית **`Constants.cs`** (מיקום הקובץ והרשאות הפתיחה).
2. נבין את הממשק **`IDatabaseService.cs`** ומדוע האתחול מתבצע באופן **פנימי ואוטומטי** בתוך השירות בלבד.
3. ננתח ונכתוב יחד את **`EnsureInitializedAsync()`**, **`ClearAllDataAsync()`**, **`SeedSampleDataAsync()`**, ואת 4 פעולות ה-CRUD המלאות עבור **מורים (`Teacher`)** — ותוכלו להעתיק חלקים אלו לתוך `DatabaseService.cs` שלכם!
4. לבסוף, תקבלו מפרט מדויק לכל פעולה שעליכם להשלים באופן דומה עבור **קורסים (`Course`)** ו**תלמידים (`Student`)**.

---

## חלק 1: בניית המחלקה הסטטית `Constants.cs` (מיקום והרשאות SQLite)

**מטרה:** ריכוז שם הקובץ, הרשאות הפתיחה ונתיב השמירה ב**מקום אחד בלבד** (Single Source of Truth), בהתאם להנחיות Microsoft ל-.NET MAUI.

פתחו את `PlayWithSQLITE/Constants.cs` והשלימו אותו כך:
```csharp
using SQLite;

namespace PlayWithSQLITE;

public static class Constants
{
    // 1. שם קובץ מסד הנתונים המקומי
    public const string DatabaseFilename = "SchoolLms.db3";

    // 2. הרשאות ודגלי פתיחה (משולבים באופרטור Bitwise OR: |)
    public const SQLiteOpenFlags Flags =
        SQLiteOpenFlags.ReadWrite |    // מאפשר גם קריאה (SELECT) וגם כתיבה (INSERT/UPDATE/DELETE)
        SQLiteOpenFlags.Create |       // יוצר את הקובץ אוטומטית בהפעלה הראשונה אם אינו קיים
        SQLiteOpenFlags.SharedCache;   // מאפשר גישה אסינכרונית בטוחה ממספר משימות במקביל

    // 3. הנתיב המלא בתוך תיקיית ה-Sandbox המאובטחת של האפליקציה במכשיר
    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}
```
> [!WARNING]
> **למה `FileSystem.AppDataDirectory` ולא `C:\SchoolLms.db3`?**  
> כי ב-Android וב-iOS לכל אפליקציה יש תיקייה מבודדת משלה, ונתיבים קשיחים של Windows פשוט קורסים על טלפון!

---

## חלק 2: הבנת החוזה `IDatabaseService` — ומהם 4 עמודי ה-CRUD?

פתחו את `PlayWithSQLITE/Services/IDatabaseService.cs`. ממשק זה הוא **החוזה (Contract)** שבין המסכים (ViewModels) לבין שכבת הנתונים:
- המסכים מכירים **רק** את `IDatabaseService` — הם לא יודעים שמתחת למכסה המנוע פועל SQLite!
- **שימו לב:** בממשק `IDatabaseService` **אין** מתודה `InitializeAsync()`!  
  מדוע? כי פתיחת החיבור ויצירת הטבלאות הן **אחריות פנימית של השירות (`DatabaseService`) בלבד** — ה-ViewModel מבקש *"תביא לי את המורים"*, והשירות דואג אוטומטית לפתוח את החיבור בפעם הראשונה אם עדיין לא נפתח!

### מה חייב לכלול CRUD מלא על כל טבלה?
| אות | משמעות | מה היא מקבלת ומחזירה? | מתודה ב-`SQLiteAsyncConnection` |
| :---: | :--- | :--- | :--- |
| **C** | **Create** (יצירה) | מקבלת אובייקט חדש (`Id == 0`), מחזירה `Task<int>` (מספר שורות שנוספו) ומעדכנת את `entity.Id` | `await _database!.InsertAsync(entity)` |
| **R** | **Read All** (שליפת רשימה) | מחזירה `Task<List<T>>` ממוינת | `await _database!.Table<T>().OrderBy(...).ToListAsync()` |
| **R** | **Read By Id** (שליפת יחיד) | מקבלת `int id`, מחזירה `Task<T?>` (`null` אם לא נמצא!) | `await _database!.Table<T>().FirstOrDefaultAsync(x => x.Id == id)` |
| **U** | **Update** (עדכון) | מקבלת אובייקט קיים (`Id != 0`), מחזירה `Task<int>` | `await _database!.UpdateAsync(entity)` |
| **D** | **Delete** (מחיקה) | מקבלת אובייקט למחיקה, מחזירה `Task<int>` | `await _database!.DeleteAsync(entity)` |

---

## חלק 3: שלב-אחר-שלב בבניית `DatabaseService.cs` (דוגמה מלאה לניתוח והעתקה)

פתחו את `PlayWithSQLITE/Services/DatabaseService.cs`. נבנה כעת את השלד המרכזי של המחלקה וננתח **כל שורה**:

### צעד 3.1 — שדה החיבור והבנאים (Constructors)
הוסיפו `using SQLite;` בראש הקובץ, הגדירו את שדה החיבור האסינכרוני ועדכנו את הבנאי להשתמש ב-`Constants.DatabasePath`:
```csharp
private SQLiteAsyncConnection? _database; // ? מציין שבתחילה (לפני הפנייה הראשונה) החיבור עדיין null
private readonly string _dbPath;

// בנאי ברירת המחדל לשימוש האפליקציה (משתמש במחלקה הסטטית Constants!)
public DatabaseService()
{
    _dbPath = Constants.DatabasePath;
}

// בנאי נוסף המשמש את בדיקות היחידה (xUnit v3) לעבודה מול קובץ זמני מבודד
public DatabaseService(string dbPath)
{
    _dbPath = dbPath;
}
```

### צעד 3.2 — אתחול פנימי אוטומטי (`private async Task EnsureInitializedAsync()`)
**מטרה:** מוודא שהחיבור פתוח וש-4 הטבלאות קיימות לפני כל פעולה. פקודת `CreateTableAsync<T>()` היא **אידמפוטנטית** (אם הטבלה כבר קיימת היא לא מוחקת נתונים ולא זורקת שגיאה!).
```csharp
private async Task EnsureInitializedAsync()
{
    // 1. אם החיבור כבר נפתח בעבר — יוצאים מיד (מניעת פתיחה כפולה)
    if (_database != null)
        return;

    // 2. פתיחת חיבור אסינכרוני לפי הנתיב וההרשאות מ-Constants
    _database = new SQLiteAsyncConnection(_dbPath, Constants.Flags);

    // 3. יצירת 4 הטבלאות אם עדיין אינן קיימות
    await _database.CreateTableAsync<Teacher>();
    await _database.CreateTableAsync<Course>();
    await _database.CreateTableAsync<Student>();
    await _database.CreateTableAsync<Enrollment>();
}
```

> [!NOTE]
> **מהו סימן הקריאה (`!`) בביטוי `_database!.InsertAsync(...)` (נקרא "Bang" / Null-Forgiving Operator)?**  
> מכיוון שהגדרנו את השדה כ-`SQLiteAsyncConnection?` (יכול להיות `null` לפני הקריאה הראשונה), המהדר של C# מזהיר בכל שורה שבה ניגש אליו: *"אולי `_database` הוא `null`?"*.  
> אבל מיד אחרי שהרצנו `await EnsureInitializedAsync();` — **אנחנו יודעים בוודאות ש-`_database` כבר מאותחל ואינו `null`!**  
> הסימן **`!`** (`_database!.`) מודיע למהדר: *"סמוך עליי, בשורה הזו המשתנה כבר בוודאות לא `null`"*.

### צעד 3.3 — ניקוי נתונים (`ClearAllDataAsync`) וזריעת נתוני הדגמה (`SeedSampleDataAsync`)
- **`ClearAllDataAsync()`**: לא מקבלת פרמטרים, מחזירה `Task`. מוחקת את כל השורות בסדר **ילדים לפני הורים** (`Enrollment` ➡️ `Course` ➡️ `Student` ➡️ `Teacher`):
```csharp
public async Task ClearAllDataAsync()
{
    await EnsureInitializedAsync();
    await _database!.DeleteAllAsync<Enrollment>();
    await _database.DeleteAllAsync<Course>();
    await _database.DeleteAllAsync<Student>();
    await _database.DeleteAllAsync<Teacher>();
}
```
- **`SeedSampleDataAsync()`**: בודקת תחילה אם כבר קיימים מורים (`CountAsync() > 0`) כדי **לא לשכפל נתונים** אם תיקרא פעמיים! לאחר מכן יוצרת 3 מורים, 4 קורסים (עם `TeacherId` שנוצר אוטומטית ע"י `InsertAsync`!), 6 תלמידים ו-10 רישומים (העתיקו את גוף המתודה במלואו כדי שבדיקות היחידה יעברו מול אותם שמות צפויים, כגון `"ישראל ישראלי"`):
```csharp
public async Task SeedSampleDataAsync()
{
    await EnsureInitializedAsync();

    // מניעת זריעה כפולה אם המסד כבר מכיל נתונים
    var teacherCount = await _database!.Table<Teacher>().CountAsync();
    if (teacherCount > 0)
        return;

    // 1. הוספת 3 מורים — לאחר InsertAsync כל מורה מקבל Id אוטומטי מהמסד!
    var teacher1 = new Teacher { FullName = "ישראל ישראלי", Email = "israel@school.org", Department = "מדעי המחשב" };
    var teacher2 = new Teacher { FullName = "שרה לוי", Email = "sara@school.org", Department = "מתמטיקה" };
    var teacher3 = new Teacher { FullName = "דוד כהן", Email = "david@school.org", Department = "פיזיקה" };
    await _database.InsertAsync(teacher1);
    await _database.InsertAsync(teacher2);
    await _database.InsertAsync(teacher3);

    // 2. הוספת 4 קורסים — קישור למורה באמצעות teacher.Id שנוצר הרגע!
    var course1 = new Course { Title = "תכנות ב-C# ו-MAUI", Credits = 5, Description = "פיתוח אפליקציות מובייל חוצות-פלטפורמות", TeacherId = teacher1.Id };
    var course2 = new Course { Title = "מבני נתונים ואלגוריתמים", Credits = 4, Description = "יסודות תיאורטיים ומעשיים", TeacherId = teacher1.Id };
    var course3 = new Course { Title = "חדו\"א לתלמידי מדעי המחשב", Credits = 4, Description = "חשבון אינפיניטסימלי", TeacherId = teacher2.Id };
    var course4 = new Course { Title = "מכניקה קלאסית וחשמל", Credits = 3, Description = "מעבדות ופיזיקה מעשית", TeacherId = teacher3.Id };
    await _database.InsertAsync(course1);
    await _database.InsertAsync(course2);
    await _database.InsertAsync(course3);
    await _database.InsertAsync(course4);

    // 3. הוספת 6 תלמידים (ב-3 שכבות גיל שונות לצורך דוחות ודפדוף)
    var students = new List<Student>
    {
        new() { FirstName = "נועם", LastName = "אברהם", Email = "noam@students.org", GradeLevel = 11, BirthDate = new DateTime(2009, 3, 15) },
        new() { FirstName = "מאיה", LastName = "שלום", Email = "maya@students.org", GradeLevel = 11, BirthDate = new DateTime(2009, 7, 22) },
        new() { FirstName = "יונתן", LastName = "מזרחי", Email = "yonatan@students.org", GradeLevel = 12, BirthDate = new DateTime(2008, 1, 10) },
        new() { FirstName = "תמר", LastName = "גבאי", Email = "tamar@students.org", GradeLevel = 12, BirthDate = new DateTime(2008, 11, 5) },
        new() { FirstName = "אורי", LastName = "אלון", Email = "ori@students.org", GradeLevel = 10, BirthDate = new DateTime(2010, 5, 30) },
        new() { FirstName = "רוני", LastName = "פרץ", Email = "roni@students.org", GradeLevel = 10, BirthDate = new DateTime(2010, 9, 18) }
    };
    foreach (var s in students) await _database.InsertAsync(s);

    // 4. הוספת 10 רישומים (Enrollments) עם ציונים
    var enrollments = new List<Enrollment>
    {
        new() { StudentId = students[0].Id, CourseId = course1.Id, Grade = 95, IsPaid = true },
        new() { StudentId = students[0].Id, CourseId = course2.Id, Grade = 88, IsPaid = true },
        new() { StudentId = students[1].Id, CourseId = course1.Id, Grade = 92, IsPaid = true },
        new() { StudentId = students[1].Id, CourseId = course3.Id, Grade = 85, IsPaid = true },
        new() { StudentId = students[2].Id, CourseId = course1.Id, Grade = 78, IsPaid = true },
        new() { StudentId = students[2].Id, CourseId = course4.Id, Grade = 91, IsPaid = false },
        new() { StudentId = students[3].Id, CourseId = course2.Id, Grade = 97, IsPaid = true },
        new() { StudentId = students[3].Id, CourseId = course3.Id, Grade = 94, IsPaid = true },
        new() { StudentId = students[4].Id, CourseId = course4.Id, Grade = 82, IsPaid = true },
        new() { StudentId = students[5].Id, CourseId = course1.Id, Grade = 89, IsPaid = true }
    };
    foreach (var e in enrollments) await _database.InsertAsync(e);
}
```

### צעד 3.4 — דוגמה מלאה של 4 פעולות CRUD עבור **מורה (`Teacher`)** (העתיקו ולמדו את הדפוס!)
```csharp
// Read All: לא מקבלת פרמטרים; מחזירה את כל המורים ממוינים א-ב לפי FullName
public async Task<List<Teacher>> GetTeachersAsync()
{
    await EnsureInitializedAsync();
    return await _database!.Table<Teacher>()
                           .OrderBy(t => t.FullName)
                           .ToListAsync();
}

// Read By Id: מקבלת id של מורה; מחזירה את המורה או null אם לא קיים
// (בשלב 3 נשדרג אותה כדי שתתמוך גם בפרמטר includeCourses)
public async Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false)
{
    await EnsureInitializedAsync();
    return await _database!.Table<Teacher>()
                           .FirstOrDefaultAsync(t => t.Id == id);
}

// Create / Update (דפוס Upsert): מקבלת Teacher; אם Id != 0 מעדכנת, אחרת מוסיפה כרשומה חדשה!
public async Task<int> SaveTeacherAsync(Teacher teacher)
{
    await EnsureInitializedAsync();
    if (teacher.Id != 0)
        return await _database!.UpdateAsync(teacher);
    return await _database!.InsertAsync(teacher);
}

// Delete: מקבלת Teacher למחיקה; מוחקת לפי PrimaryKey ומחזירה מספר שורות שנמחקו
public async Task<int> DeleteTeacherAsync(Teacher teacher)
{
    await EnsureInitializedAsync();
    return await _database!.DeleteAsync(teacher);
}
```

---

## חלק 4: ✍️ משימתכם המעשית בשלב 2 (יישום עצמאי עבור `Course` ו-`Student`)

לאחר שהשלמתם את `Constants.cs`, אתחול השירות, הזריעה ו-4 פעולות המורים (לפי חלקים 1–3 למעלה), **ממשו בעצמכם** את הפעולות המקבילות עבור `Course` ו-`Student` בתוך `DatabaseService.cs`:

| מתודה למימוש | מה היא מקבלת? | מה היא צריכה לבצע ולהחזיר? |
| :--- | :--- | :--- |
| `GetCoursesAsync(bool includeRelations = false)` | דגל קשרים (יופעל בשלב 3) | לוודא אתחול (`EnsureInitializedAsync`), לשלוף את כל ה-`Course` ממוינים לפי `Title`, ולהחזיר `List<Course>`. |
| `GetCourseByIdAsync(int id, bool includeRelations = true)` | `int id` | לשלוף קורס בודד לפי `c => c.Id == id` בעזרת `FirstOrDefaultAsync`, ולהחזיר `Course?`. |
| `SaveCourseAsync(Course course)` | `Course course` | דפוס Upsert: אם `course.Id != 0` ➡️ `UpdateAsync`, אחרת `InsertAsync`. |
| `DeleteCourseAsync(Course course)` | `Course course` | למחוק את הקורס עם `DeleteAsync(course)` ולהחזיר `int`. |
| `GetStudentsAsync(int skip = 0, int take = 20)` | `skip`, `take` (יופעלו בשלב 4) | לשלוף את כל ה-`Student` ממוינים לפי `LastName` ואז `ThenBy(s => s.FirstName)`. |
| `GetStudentsCountAsync()` | ללא | לספור את סך כל התלמידים בטבלה בעזרת `_database!.Table<Student>().CountAsync()`. |
| `GetStudentByIdAsync(int id, bool includeCourses = false)` | `int id` | לשלוף תלמיד בודד לפי `s => s.Id == id` בעזרת `FirstOrDefaultAsync`. |
| `SaveStudentAsync(Student student)` | `Student student` | דפוס Upsert: אם `student.Id != 0` ➡️ `UpdateAsync`, אחרת `InsertAsync`. |
| `DeleteStudentAsync(Student student)` | `Student student` | למחוק את התלמיד עם `DeleteAsync(student)` ולהחזיר `int`. |

### ✅ בדיקה עצמית בסיום שלב 2 (בדיקת יחידה ראשונה!):
1. היעזרו במדריך [00b-How-To-Run-Tests.pdf](./00b-How-To-Run-Tests.pdf).
2. פתחו את `PlayWithSQLITE.Tests/DatabaseServiceTests.cs`, הסירו את `Skip = "..."` מהבדיקה **`GetTeachersAsync_ReturnsSeededTeachers`**, והריצו את הבדיקות.
3. ודאו ש-**2 בדיקות עוברות בירוק (`succeeded: 2, failed: 0`)**!
4. להשוואה מול הפתרון המוכן: `git checkout step-02-constants-and-basic-crud`.

עברו לשלב הבא: [03-Table-Relationships.pdf](./03-Table-Relationships.pdf).
