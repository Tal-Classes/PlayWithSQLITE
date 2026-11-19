# 🏫 משימת מסע: בניית מערכת ניהול בית ספר מרובת-טבלאות ב-SQLite ו-.NET MAUI

ברוכים הבאים למשימת הלימוד של מסד נתונים מקומי (`SQLite`) באפליקציית .NET MAUI!  
בפרויקט זה תלמדו מאפס כיצד לתכנן, לבנות, לקשר, לתשאל ולחבר אל ממשק המשתמש מסד נתונים מקומי מרובה-טבלאות (`Multi-Table Database`) לניהול **מורים**, **קורסים**, **תלמידים** ו**רישומים לציונים**.

---

## 🎯 מה כבר מוכן עבורכם ומה עליכם לבנות?

| רכיב בפרויקט | מצב ב-`main` (נקודת הפתיחה) | תפקיד התלמיד |
| :--- | :--- | :--- |
| **מסכי ה-UI (`Views/*.xaml`)** | ✅ מוכנים ומעוצבים עם Bindings | להבין כיצד ה-XAML נקשר ל-Properties ול-`ICommand` |
| **תשתית ה-ViewModels** | ✅ שדות ו-`ObservableCollection` מוכנים | **שלב 5:** ליצור את הפקודות (`ICommand`) ולהפעיל את המסד |
| **החוזה (`IDatabaseService.cs`) ו-DTOs** | ✅ מוכנים ומתועדים | **שלב 2:** להבין את החוזה ומבנה ה-CRUD |
| **חבילות NuGet ב-`.csproj`** | ⬜ טרם הותקנו | **שלב 1:** התקנה ויזואלית (גרסאות Stable בלבד) |
| **4 המודלים (`Models/*.cs`)** | ⬜ מחלקות C# רגילות (POCOs) | **שלב 1:** להבין ERD, קשרים ו-`CascadeOperation` ולהוסיף דקורציות |
| **מחלקת `Constants.cs`** | ⬜ שלד עם שאלות מנחות | **שלב 2:** להגדיר שם קובץ, הרשאות (`Flags`) ונתיב |
| **שירות המסד (`DatabaseService.cs`)** | ⬜ שלד עם שאלות מנחות | **שלבים 2–4:** אתחול פנימי אוטומטי, CRUD, קשרים ושאילתות SQL |
| **ה-DI Container (`MauiProgram.cs`)** | ⬜ חסר רישום שירות המסד | **שלב 5:** לרשום `IDatabaseService` מול `DatabaseService` |
| **בדיקות יחידה (`PlayWithSQLITE.Tests`)** | ✅ מוגדרות ב-**xUnit v3** (עם `Skip`) | **שלבים 2–6:** להסיר `Skip` בהדרגה ולהריץ בדיקות |

---

## 🌳 מפת ה-Branches ב-Git — עבודה ובדיקה עצמית

ענף **`main`** הוא סביבת העבודה שלכם. כל ענף ממוספר מייצג את הפתרון המלא והבדוק לאותו שלב:

```text
main                                    <-- 🚀 כאן אתם עובדים! (נקודת הפתיחה)
 ├── step-01-packages-and-models        <-- 📦 שלב 1: NuGets יציבים + ERD + קישוט 4 המודלים
 ├── step-02-constants-and-basic-crud   <-- 🔌 שלב 2: Constants + אתחול אוטומטי + CRUD בסיסי
 ├── step-03-relationships              <-- 🔗 שלב 3: קשרי 1:N ו-M:N ב-DatabaseService
 ├── step-04-queries-aggregations-join  <-- 📊 שלב 4: JOIN, GROUP BY, COUNT, SUM, AVG, SKIP/TAKE
 ├── step-05-di-and-viewmodels          <-- 🧩 שלב 5: רישום DI ויצירת פקודות ICommand ב-ViewModels
 └── step-06-full-solution              <-- 🏆 שלב 6: פתרון מלא + 13 בדיקות xUnit v3 עוברות
```

> [!TIP]
> **בדיקה עצמית מול ענף פתרון וחזרה לעבודה ב-`main`:**
> ```bash
> git checkout step-01-packages-and-models
> # ... השוואת הקוד ...
> git checkout main
> ```

---

## 🧭 סדר העבודה שלב-אחר-שלב (קישורים לקובצי ה-PDF בתיקיית `Docs`)

> [!NOTE]
> **מדריך חובה לפני שלב 2 — פעם ראשונה עם בדיקות יחידה?**  
> קראו תחילה את [מדריך הרצת בדיקות יחידה ב-xUnit v3 (`00b-How-To-Run-Tests.pdf`)](../00b-How-To-Run-Tests.pdf) כדי ללמוד איך קוראים קובץ בדיקות, איך מסירים `Skip`, ואיך מריצים ומנתחים תוצאות ב-Visual Studio וב-Rider!

| שלב | נושא | מדריך PDF (בתיקיית `Docs`) | תוצר נדרש בסיום השלב |
| :---: | :--- | :--- | :--- |
| **0ב** | הכרת בדיקות יחידה | [00b-How-To-Run-Tests.pdf](../00b-How-To-Run-Tests.pdf) | הבנת מבנה `xUnit v3`, הסרת `Skip`, הרצה וניתוח תוצאות. |
| **1** | NuGets, ERD ומודלים | [01-Nuget-Packages-And-Models.pdf](../01-Nuget-Packages-And-Models.pdf) | התקנה ויזואלית (VS / Rider, גרסת Stable), הבנת [תרשים ERD](../Database-ERD.pdf), `CascadeOperation`, וקישוט 4 המודלים. |
| **2** | `Constants`, ממשק ו-CRUD | [02-Database-Connection-And-Basic-CRUD.pdf](../02-Database-Connection-And-Basic-CRUD.pdf) | בניית `Constants.cs`, הבנת `IDatabaseService`, אתחול פנימי אוטומטי (`InitDBAsync`), ו-CRUD בסיסי. |
| **3** | קשרי גומלין בשירות | [03-Table-Relationships.pdf](../03-Table-Relationships.pdf) | `GetWithChildrenAsync`, `GetAllWithChildrenAsync`, ניהול `Enrollment` ומניעת רשומות יתומות. |
| **4** | שאילתות SQL ודפדוף | [04-Advanced-Queries-Aggregations.pdf](../04-Advanced-Queries-Aggregations.pdf) | פירוק `INNER/LEFT JOIN`, `GROUP BY`, `COUNT`/`AVG`/`MAX`/`MIN`/`SUM`, ודפדוף `Skip`/`Take`. |
| **5** | DI ופקודות ב-ViewModels | [05-DI-And-ViewModel-Commands.pdf](../05-DI-And-ViewModel-Commands.pdf) | רישום `IDatabaseService` ב-`MauiProgram.cs` ויצירת `ICommand` + Pagination בכל 4 ה-ViewModels. |
| **6** | אימות ובדיקות מלאות | [06-Testing-And-Verification.pdf](../06-Testing-And-Verification.pdf) | הרצת כל בדיקות `xUnit v3`, בניית הפרויקט ובדיקה חיה על האמולטור. |

בהצלחה במסע! פתחו כעת את [01-Nuget-Packages-And-Models.pdf](../01-Nuget-Packages-And-Models.pdf) כדי להתחיל בשלב 1.
