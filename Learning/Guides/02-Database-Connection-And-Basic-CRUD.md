# 🔌 שלב 2: מחלקת `Constants`, הבנת `IDatabaseService`, חיבור אסינכרוני ומימוש CRUD

בשלב זה נבנה את **הלב הפועם של שכבת הנתונים**:
1. נרכז את הגדרות הקובץ וההרשאות של SQLite במחלקה סטטית ייעודית בשם **`Constants`**.
2. נבין את המבנה והתפקיד הארכיטקטוני של הממשק **`IDatabaseService`**.
3. נפתח חיבור אסינכרוני למסד הנתונים (`SQLiteAsyncConnection`) וניצור את ארבע הטבלאות.
4. נלמד לעומק מה חייב לכלול **CRUD** מלא, ונממש אותו במחלקת **`DatabaseService`**.

---

## 1. ניהול הגדרות SQLite במחלקה סטטית נפרדת (`Constants.cs`)

בהתאם להנחיות הרשמיות של Microsoft ל-.NET MAUI, **אין לפזר** את שם קובץ מסד הנתונים, את נתיב השמירה או את דגלי הגישה בתוך שורות הקוד של השירותים.  
במקום זאת, מרכזים את כולם ב**מקור אמת יחיד (Single Source of Truth)** — מחלקה סטטית בשם **`Constants`** בקובץ [Constants.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE/Constants.cs):

```csharp
using SQLite;

namespace PlayWithSQLITE;

public static class Constants
{
    public const string DatabaseFilename = "SchoolLms.db3";

    public const SQLiteOpenFlags Flags =
        // פתיחת המסד לקריאה ולכתיבה
        SQLiteOpenFlags.ReadWrite |
        // יצירת קובץ המסד אוטומטית אם אינו קיים עדיין
        SQLiteOpenFlags.Create |
        // הפעלת מטמון משותף לגישה בטוחה ממספר תהליכונים (Multi-Threaded)
        SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}
```

### מה תפקיד כל איבר ב-`Constants`?
1. **`DatabaseFilename`**: שם הקובץ המקומי (סיומת `.db3` או `.db`). אם נרצה לשנות את שם הקובץ בעתיד — משנים במקום אחד בלבד!
2. **`Flags` (`SQLiteOpenFlags`)**: משלב שלושה דגלי הרשאה באמצעות אופרטור Bitwise OR (`|`):
   - `ReadWrite` — מאפשר גם לשלוף נתונים (`SELECT`) וגם לשנותם (`INSERT`, `UPDATE`, `DELETE`).
   - `Create` — אם האפליקציה רצה לראשונה והקובץ עדיין לא קיים במכשיר, SQLite יוצר אותו אוטומטית במקום לזרוק שגיאה.
   - `SharedCache` — מאפשר פעולה אסינכרונית בטוחה ויעילה ממספר משימות במקביל.
3. **`DatabasePath`**: מחבר בעזרת `Path.Combine` בין **`FileSystem.AppDataDirectory`** (תיקיית ה-Sandbox המבודדת והמאובטחת של האפליקציה ב-Android / iOS / Windows / macOS) לבין `DatabaseFilename`.

> [!WARNING]
> **לעולם אין להשתמש בנתיב קשיח** כגון `C:\SchoolLms.db3` או `/data/SchoolLms.db3`! נתיבים קשיחים יתרסקו מיד בהפעלה על מכשיר Android או iOS אחר.

---

## 2. הבנת הממשק `IDatabaseService` — למה בכלל צריך Interface?

פתחו את הקובץ [IDatabaseService.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE/Services/IDatabaseService.cs) והתבוננו בחתימות המתודות שבו.  
**`IDatabaseService`** הוא **החוזה (Contract)** של שכבת הנתונים: הוא מגדיר *מה* המערכת יכולה לבקש ממסד הנתונים, מבלי לגלות ל-ViewModels *איך* המידע נשמר בפועל (SQLite, שרת ענן או קובץ JSON).

### מדוע ההפרדה בין `IDatabaseService` ל-`DatabaseService` קריטית?
1. **הפרדת אחריויות (Separation of Concerns):** ה-ViewModels תלויים רק ב-`IDatabaseService`. הם אינם יודעים דבר על `SQLiteAsyncConnection` או על טבלאות SQL.
2. **הזרקת תלויות (Dependency Injection):** בשלב 5 נרשום ב-`MauiProgram.cs` את הממשק `IDatabaseService` מול המימוש `DatabaseService`, ו-MAUI יזריק אותו אוטומטית לכל ViewModel.
3. **בדיקתיות (Testability):** ניתן להחליף את המימוש בשירות דמה (`Mock`) בבדיקות יחידה, או להעביר קובץ DB זמני בבדיקות אינטגרציה.

### מבנה החוזה ב-`IDatabaseService` (ארבע משפחות של מתודות):
| משפחת מתודות | דוגמאות מהממשק | ערך החזרה ומשמעותו |
| :--- | :--- | :--- |
| **אתחול ותחזוקה** | `InitializeAsync()`, `SeedSampleDataAsync()`, `ClearAllDataAsync()` | `Task` — פעולה אסינכרונית שאינה מחזירה ערך. |
| **שליפה מרובה (`Read All`)** | `GetTeachersAsync()`, `GetCoursesAsync(...)`, `GetStudentsAsync(skip, take)` | `Task<List<T>>` — רשימת אובייקטים מהטבלה. |
| **שליפה בודדת (`Read By Id`)** | `GetTeacherByIdAsync(id)`, `GetCourseByIdAsync(id)`, `GetStudentByIdAsync(id)` | `Task<T?>` — האובייקט שנמצא, או `null` אם המזהה לא קיים! |
| **כתיבה ומחיקה (`C / U / D`)** | `SaveTeacherAsync(t)`, `DeleteTeacherAsync(t)`, `EnrollStudentAsync(...)` | `Task<int>` — מספר השורות שהושפעו במסד (לרוב `1` בהצלחה, `0` אם לא נמצא). |

---

## 3. מה חייב לכלול CRUD תקין ואיך מממשים אותו ב-`DatabaseService`?

ראשי התיבות **CRUD** מתארים את 4 פעולות היסוד על כל טבלה במסד נתונים. בכל ישות (`Teacher`, `Course`, `Student`) על השירות לספק מימוש שלם, אסינכרוני ובטוח לכל אחת מהפעולות:

### שלב מקדים — אתחול עצלן (`Lazy Initialization`) ויצירת טבלאות
לפני כל פעולת CRUD, עלינו לוודא שהחיבור `_database` כבר פתוח ושהטבלאות קיימות!  
נעשה זאת בעזרת שתי מתודות ב-`DatabaseService`:
- **`InitializeAsync()`**:
  1. אם `_database != null` ➡️ יוצאים מיד (מניעת פתיחה כפולה).
  2. פותחים חיבור עם `_dbPath` ו-**`Constants.Flags`**:
     ```csharp
     _database = new SQLiteAsyncConnection(_dbPath, Constants.Flags);
     ```
  3. מפעילים `await _database.CreateTableAsync<T>()` לכל אחת מ-4 הטבלאות (`Teacher`, `Course`, `Student`, `Enrollment`).
     *(פקודת `CreateTableAsync` היא **אידמפוטנטית** — אם הטבלה כבר קיימת היא לא מוחקת נתונים ולא זורקת שגיאה!)*
- **`EnsureInitializedAsync()`** (מתודת עזר פרטית): קוראת ל-`InitializeAsync()` אם `_database == null`, ונקראת בתחילת כל מתודת CRUD כדי שלעולם לא נקבל `NullReferenceException`!

> [!NOTE]
> **שימו לב לשני הבנאים ב-`DatabaseService`:**
> - הבנאי ברירת המחדל `public DatabaseService()` מציב `_dbPath = Constants.DatabasePath;` (לשימוש האפליקציה האמיתית במכשיר).
> - הבנאי `public DatabaseService(string dbPath)` מקבל נתיב מותאם אישית — ומשמש את פרויקט הבדיקות `PlayWithSQLITE.Tests` כדי שכל בדיקה תעבוד מול קובץ זמני נקי!

### ארבע פעולות ה-CRUD במבט מלא:

| פעולה | משמעות | מתודה ב-`SQLiteAsyncConnection` | התנהגות חשובה שחובה להכיר |
| :--- | :--- | :--- | :--- |
| **C — Create** | הוספת רשומה חדשה לטבלה | `await _database.InsertAsync(entity)` | אם לשדה `Id` יש `[PrimaryKey, AutoIncrement]`, SQLite ממספר אותו אוטומטית **ומעדכן את `entity.Id` בזיכרון**! |
| **R — Read (All)** | שליפת רשימת רשומות (עם מיון/סינון) | `await _database.Table<T>().OrderBy(...).ToListAsync()` | תומך בביטויי LINQ (`Where`, `OrderBy`, `Skip`, `Take`) המתורגמים ישירות ל-SQL. |
| **R — Read (By Id)** | שליפת רשומה אחת לפי מפתח ראשי | `await _database.Table<T>().FirstOrDefaultAsync(x => x.Id == id)` | מחזיר `null` (ולא זורק חריגה) אם הרשומה לא קיימת! |
| **U — Update** | עדכון רשומה קיימת בטבלה | `await _database.UpdateAsync(entity)` | מאתר את השורה במסד לפי ה-`PrimaryKey` (`entity.Id`) ומעדכן את שאר עמודותיה. |
| **D — Delete** | מחיקת רשומה בודדת או טבלה שלמה | `await _database.DeleteAsync(entity)` / `DeleteAllAsync<T>()` | מוחק לפי `PrimaryKey`. בניקוי מלא (`ClearAllDataAsync`) מוחקים קודם טבלאות ילדים (`Enrollment`, `Course`) ורק אחר כך הורים (`Student`, `Teacher`)! |

### דפוס Upsert (`SaveXxxAsync`) — איחוד Create ו-Update
במקום לבלבל את המסך בשאלה "האם זו ישות חדשה או עריכה?", מאחדים יצירה ועדכון למתודה אחת:
- אם `entity.Id != 0` ➡️ הרשומה כבר קיימת במסד ➡️ מפעילים `UpdateAsync(entity)`.
- אחרת (`entity.Id == 0`) ➡️ זו רשומה חדשה שטרם קיבלה מזהה ➡️ מפעילים `InsertAsync(entity)`.

---

## 4. כלל הברזל של ביצועים ב-.NET MAUI: אסינכרוניות בלבד!
- ✅ **חובה** להשתמש ב-`async / await` ובשיטות האסינכרוניות (`InsertAsync`, `ToListAsync`, `UpdateAsync`, `DeleteAsync`).
- ❌ **אסור בהחלט** לחסום את תהליכון ממשק המשתמש (UI Thread) באמצעות `.Result` או `.Wait()` — הדבר גורם לקפיאת האפליקציה (ANR) ואף לקיפאון הדדי (Deadlock).

---

## 5. משימתכם המעשית בשלב 2

1. פתחו את [Constants.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE/Constants.cs) והשלימו את `DatabaseFilename`, `Flags` ו-`DatabasePath`.
2. פתחו את [DatabaseService.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE/Services/DatabaseService.cs) וממשו:
   - את השדה `private SQLiteAsyncConnection? _database;` ואת השימוש ב-`Constants.DatabasePath` בבנאי ברירת המחדל.
   - את `InitializeAsync()`, `EnsureInitializedAsync()`, `ClearAllDataAsync()` ו-`SeedSampleDataAsync()`.
   - את פעולות ה-CRUD הבסיסיות עבור **מורים** (`GetTeachersAsync`, `GetTeacherByIdAsync`, `SaveTeacherAsync`, `DeleteTeacherAsync`), **קורסים** (`GetCoursesAsync`, `GetCourseByIdAsync`, `SaveCourseAsync`, `DeleteCourseAsync`), ו**תלמידים** (`GetStudentsAsync`, `GetStudentsCountAsync`, `GetStudentByIdAsync`, `SaveStudentAsync`, `DeleteStudentAsync`).

### ✅ בדיקה עצמית בסיום שלב 2:
- פתחו את [DatabaseServiceTests.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE.Tests/DatabaseServiceTests.cs), הסירו את `Skip = ...` מהבדיקה `GetTeachersAsync_ReturnsSeededTeachers` והריצו `dotnet test` — ודאו שהבדיקה עוברת (`succeeded: 2`)!
- להשוואה מול הפתרון המוכן: `git checkout step-02-constants-and-basic-crud`.

עברו לשלב הבא: [03-Table-Relationships.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/03-Table-Relationships.md).
