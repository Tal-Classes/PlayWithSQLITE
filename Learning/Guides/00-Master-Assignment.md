# 🏫 משימת מסע: בניית מערכת ניהול בית ספר מרובת-טבלאות ב-SQLite ו-.NET MAUI

ברוכים הבאים למשימת הלימוד של מסד נתונים מקומי (`SQLite`) באפליקציית .NET MAUI!

בפרויקט זה תלמדו מאפס כיצד לתכנן, לבנות, לקשר, לתשאל ולחבר אל ממשק המשתמש מסד נתונים מקומי אמיתי מרובה-טבלאות (`Multi-Table Database`), המשמש לניהול **מורים**, **קורסים**, **תלמידים** ו**רישומים לציונים**.

---

## 🎯 מה כבר מוכן עבורכם ומה עליכם לבנות?

כדי שתוכלו להתמקד לעומק בארכיטקטורת הנתונים, ב-SQLite ובחיבור ה-MVVM ל-DB, הפרדנו את התשתית למשימות ברורות:

| רכיב בפרויקט | מצב ב-`main` (נקודת הפתיחה) | מה התפקיד שלכם? |
| :--- | :--- | :--- |
| **מסכי ה-UI (`Views/*.xaml`)** | ✅ מוכנים ומעוצבים עם Bindings | ללמוד כיצד ה-XAML נקשר ל-Properties ול-`ICommand` |
| **תשתית ה-ViewModels (שדות ו-Collections)** | ✅ מוכנה (`ObservableCollection`, שדות קלט, `IsLoading`) | **שלב 5:** ליצור את הפקודות (`ICommand`) ולהפעיל את `IDatabaseService` |
| **החוזה (`IDatabaseService.cs`) ו-DTOs** | ✅ מוכנים ומתועדים | **שלב 2:** להבין את משמעות הממשק ופעולות ה-CRUD |
| **חבילות NuGet ב-`.csproj`** | ⬜ טרם הותקנו | **שלב 1:** התקנה ויזואלית של הגרסאות היציבות (Stable) |
| **4 המודלים (`Models/*.cs`)** | ⬜ מחלקות C# רגילות (POCOs) עם שאלות מנחות | **שלב 1:** להבין ERD, קשרים ו-`CascadeOperation`, ולהוסיף דקורציות |
| **מחלקת `Constants.cs`** | ⬜ שלד עם שאלות מנחות | **שלב 2:** להגדיר שם קובץ, הרשאות (`Flags`) ונתיב (`DatabasePath`) |
| **שירות המסד (`DatabaseService.cs`)** | ⬜ שלד עם שאלות מנחות | **שלבים 2–4:** לממש חיבור, CRUD, קשרים ושאילתות SQL/LINQ |
| **ה-DI Container (`MauiProgram.cs`)** | ⬜ ViewModels ו-Pages רשומים; שירות המסד חסר | **שלב 5:** לרשום את `IDatabaseService` מול `DatabaseService` במחזור החיים המתאים |
| **בדיקות היחידה (`PlayWithSQLITE.Tests`)** | ✅ מוגדרות ב-**xUnit v3** (עם `Skip` לפי שלבים) | **שלבים 2–6:** להסיר את ה-`Skip` בהדרגה ולוודא שכל הבדיקות עוברות! |

---

## 🌳 מפת ה-Branches ב-Git — עבודה ובדיקה עצמית

במאגר הוכן ענף (Branch) ייעודי לכל שלב. ענף **`main`** הוא סביבת העבודה שלכם, וכל ענף ממוספר מייצג את הפתרון המלא והבדוק לאותו שלב:

```text
main                                      <-- 🚀 כאן אתם עובדים! (נקודת הפתיחה לתלמיד)
 ├── step-01-packages-and-models          <-- 📦 שלב 1: NuGets יציבים + ERD + מודלים עם קשרים ו-CascadeOperation
 ├── step-02-constants-and-basic-crud     <-- 🔌 שלב 2: מחלקת Constants + חיבור אסינכרוני + CRUD בסיסי
 ├── step-03-relationships                <-- 🔗 שלב 3: מימוש קשרי 1:N ו-M:N בשירות (SQLiteNetExtensions.Async)
 ├── step-04-queries-aggregations-join    <-- 📊 שלב 4: שאילתות SQL מתקדמות (JOIN, GROUP BY, COUNT, SUM, SKIP/TAKE)
 ├── step-05-di-and-viewmodels            <-- 🧩 שלב 5: רישום ב-DI Container ויצירת פקודות ICommand ב-ViewModels
 └── step-06-full-solution                <-- 🏆 שלב 6: הפתרון המלא והמאומת (כולל כל בדיקות ה-xUnit v3)
```

> [!TIP]
> **כיצד משווים מול שלב פתרון וחוזרים לעבוד ב-`main`?**
> ```bash
> git checkout step-01-packages-and-models
> # ... בדיקת הפתרון והשוואה ...
> git checkout main
> ```

---

## 🧭 סדר העבודה שלב-אחר-שלב (6 המדריכים)

| שלב | נושא | מדריך מפורט | מה לומדים ומממשים? |
| :--- | :--- | :--- | :--- |
| **שלב 1** | NuGets, תכנון ERD, קשרים ומודלים | [01-Nuget-Packages-And-Models.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/01-Nuget-Packages-And-Models.md) | התקנה ויזואלית (VS / Rider, גרסת Stable בלבד), הבנת תרשים ERD, מה מוגדר בכל צד של קשר 1:N ו-M:N, בחירת `CascadeOperation`, וקישוט 4 המודלים. |
| **שלב 2** | `Constants`, הבנת `IDatabaseService` ו-CRUD | [02-Database-Connection-And-Basic-CRUD.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/02-Database-Connection-And-Basic-CRUD.md) | בניית המחלקה הסטטית `Constants` (מיקום והרשאות), הבנת החוזה `IDatabaseService`, פתיחת `SQLiteAsyncConnection`, ומימוש מלא של CRUD (`Create`, `Read`, `Update`, `Delete`). |
| **שלב 3** | עבודה מעשית עם קשרי גומלין בשירות | [03-Table-Relationships.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/03-Table-Relationships.md) | שליפת ישויות עם הילדים שלהן (`GetWithChildrenAsync`, `GetAllWithChildrenAsync`), ניהול טבלת הגישור `Enrollment`, ומניעת רשומות יתומות במחיקה. |
| **שלב 4** | פירוק שאילתות SQL מתקדמות ודפדוף | [04-Advanced-Queries-Aggregations.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/04-Advanced-Queries-Aggregations.md) | סדר הביצוע הלוגי ב-SQL, הדמיה טבלאית של מה שנוצר בכל שלב של `INNER JOIN` ו-`LEFT JOIN`, `GROUP BY`, `COUNT`/`SUM`/`AVG`/`MAX`/`MIN`, ודפדוף עם `Skip` ו-`Take`. |
| **שלב 5** | רישום ב-DI Container ופקודות ב-ViewModels | [05-DI-And-ViewModel-Commands.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/05-DI-And-ViewModel-Commands.md) | רישום `IDatabaseService` ב-`MauiProgram.cs` (`Singleton` מול `Transient`), ויצירת פקודות `ICommand` עם `CanExecute` ו-`IsLoading` בכל 4 ה-ViewModels להבאת הנתונים למסך. |
| **שלב 6** | בדיקות יחידה ב-xUnit v3 ואימות מערכת | [06-Testing-And-Verification.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/06-Testing-And-Verification.md) | הפעלת בדיקות ה-xUnit v3 (`Microsoft.Testing.Platform`), הרצת `Audit-DI.ps1`, ובדיקה מלאה של כל המסכים באפליקציה. |

---

## 🖼️ מבט על מסכי האפליקציה המוכנים עבורכם

ארבעת המסכים כבר מוכנים וממתינים שתזרימו אליהם נתונים אמיתיים ממסד הנתונים שלכם:

- **מסך התלמידים (`StudentsPage`)**: רשימת תלמידים עם דפדוף (`Skip`/`Take`), הוספת תלמיד חדש, וגיליון ציונים אישי עם סך נ"ז (`SUM`).
- **מסך הקורסים (`CoursesPage`)**: יצירת קורס עם שיוך למורה (`1:N`), והצגת רשימת קורסים יחד עם שם המורה (`ManyToOne`).
- **מסך המורים (`TeachersPage`)**: ניהול סגל מורים והצגת רשימת הקורסים של כל מורה (`OneToMany`).
- **מסך הדוחות (`ReportsPage`)**: הפקת דוחות סטטיסטיים בזמן אמת (`LEFT JOIN`, `GROUP BY`, `COUNT`, `AVG`, `MAX`, `MIN`), לצד כפתורי זריעת נתונים ואיפוס המסד.

בהצלחה במסע! עברו כעת לקובץ [01-Nuget-Packages-And-Models.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/01-Nuget-Packages-And-Models.md) כדי להתחיל בשלב 1.
