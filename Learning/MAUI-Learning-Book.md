# ספר הלמידה: מסד נתונים מקומי SQLite מרובה-טבלאות ב-.NET MAUI

ספר למידה זה מלווה את פרויקט `PlayWithSQLITE`. הוא מרכז את עקרונות הנדסת התוכנה, תכנון ERD, עיצוב הנתונים, פקודות ה-SQL, הזרקת תלויות (DI) וחיבור MVVM שנלמדו במסגרת פרויקט הבגרות ברמת 5 יחידות לימוד.

---

## 📚 פרק 1: התקנה ויזואלית, תרשים ERD, קשרי גומלין, `CascadeOperation` ומודלים
- **מה נבנה:** התקנת החבילות הרשמיות והיציבות (`Stable` בלבד, ללא Prerelease) `sqlite-net-pcl` ו-`SQLiteNetExtensions.Async` דרך ממשק `Manage NuGet Packages` ב-Visual Studio וב-Rider; תכנון תרשים **ERD** ל-4 טבלאות (`Teachers`, `Courses`, `Students`, `Enrollments`); והגדרת הדקורציות והקשרים בשני צדי הקשר (`1:N` ו-`M:N`).
- **מושגים שנלמדו:** תרשים ERD, מפתח ראשי (`PK`), מפתח זר (`FK`) בצד ה-N, טבלת גישור (`Junction Table`), `CascadeOperation` (`None`, `CascadeRead`, `CascadeInsert`, `CascadeDelete`, `All`), ודקורציות (`[Table]`, `[PrimaryKey]`, `[AutoIncrement]`, `[NotNull]`, `[Unique]`, `[MaxLength]`, `[Ignore]`, `[ForeignKey]`, `[OneToMany]`, `[ManyToOne]`, `[ManyToMany]`).
- **מדריך מפורט:** [01-Nuget-Packages-And-Models.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/01-Nuget-Packages-And-Models.md).

---

## 📚 פרק 2: מחלקת `Constants`, הבנת `IDatabaseService`, חיבור אסינכרוני ו-CRUD
- **מה נבנה:** מחלקה סטטית ייעודית `Constants.cs` המרכזת את שם קובץ המסד (`DatabaseFilename`), הרשאות הפתיחה (`SQLiteOpenFlags`) ונתיב השמירה (`FileSystem.AppDataDirectory`); הבנת הממשק `IDatabaseService`; ומימוש החיבור האסינכרוני (`SQLiteAsyncConnection`) ופעולות ה-CRUD המלאות ב-`DatabaseService`.
- **למה זה נחוץ:** מקור אמת יחיד להגדרות המסד, הפרדת שכבות (Loose Coupling) ושמירה על תהליכון ה-UI חופשי בעזרת `async / await`.
- **מושגים שנלמדו:** `Constants.Flags`, `CreateTableAsync` (אידמפוטנטיות), `InsertAsync`, `Table<T>().ToListAsync()`, `FirstOrDefaultAsync`, `UpdateAsync` (ודפוס Upsert), `DeleteAsync`, `DeleteAllAsync`.
- **מדריך מפורט:** [02-Database-Connection-And-Basic-CRUD.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/02-Database-Connection-And-Basic-CRUD.md).

---

## 📚 פרק 3: עבודה מעשית עם קשרי גומלין בשכבת השירות (`1:N` ו-`M:N`)
- **מה נבנה:** טעינת אובייקטים מקושרים באמצעות `SQLiteNetExtensions.Async` (`GetWithChildrenAsync`, `GetAllWithChildrenAsync`), ניהול טבלת הגישור `Enrollment` (רישום ללא כפילויות, עדכון ציון, ביטול רישום), ומחיקה מבוקרת המונעת רשומות יתומות (`Orphan Records`).
- **מדריך מפורט:** [03-Table-Relationships.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/03-Table-Relationships.md).

---

## 📚 פרק 4: פירוק שאילתות SQL מתקדמות — סדר ביצוע, `JOIN`, `GROUP BY`, אגרגציות ודפדוף
- **מה נבנה:** הבנת סדר הביצוע הלוגי ב-SQL (`FROM` ➡️ `JOIN` ➡️ `WHERE` ➡️ `GROUP BY` ➡️ `HAVING` ➡️ `SELECT` ➡️ `ORDER BY` ➡️ `LIMIT/OFFSET`), פירוק שלב-אחר-שלב של הטבלה הנוצרת בכל שלב של `INNER JOIN` ו-`LEFT JOIN`, חישובי `COUNT(e.Id)` מול `COUNT(*)`, `AVG`, `MAX`, `MIN`, `SUM` עם `ExecuteScalarAsync<int>`, ודפדוף עם `OrderBy` + `Skip` + `Take`.
- **מדריך מפורט:** [04-Advanced-Queries-Aggregations.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/04-Advanced-Queries-Aggregations.md).

---

## 📚 פרק 5: רישום ב-DI Container (`MauiProgram.cs`) ויצירת פקודות `ICommand` ב-ViewModels
- **מה נבנה:** רישום `IDatabaseService` מול `DatabaseService` כ-`Singleton` ב-DI Container לצד `Transient` עבור ViewModels ו-Pages; ויצירת כל הפקודות (`ICommand`, `Command<T>`, `CanExecute`, `ChangeCanExecute`, הגנת `IsLoading`) בכל 4 ה-ViewModels (`TeachersViewModel`, `CoursesViewModel`, `StudentsViewModel`, `ReportsViewModel`) להזרמת נתונים מהמסד אל מסכי ה-XAML.
- **מדריך מפורט:** [05-DI-And-ViewModel-Commands.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/05-DI-And-ViewModel-Commands.md).

---

## 📚 פרק 6: בדיקות יחידה ב-xUnit v3, אימות DI ומפת הענפים
- **מה נבנה:** פרויקט בדיקות מודרני ב-**xUnit v3** על גבי `Microsoft.Testing.Platform` (`global.json`, `<OutputType>Exe</OutputType>`, `IAsyncLifetime` עם `ValueTask`) ואימות אוטומטי באמצעות `Audit-DI.ps1` ו-`Validate-Tests.ps1`.
- **מדריך בדיקות ואימות:** [06-Testing-And-Verification.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/06-Testing-And-Verification.md).
- **מדריך המשימה הראשי:** [00-Master-Assignment.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/00-Master-Assignment.md).
