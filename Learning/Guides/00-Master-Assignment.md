# 🏫 משימת מסע: בניית מערכת ניהול בית ספר מרובת-טבלאות ב-SQLite ו-.NET MAUI

ברוכים הבאים למשימת הלימוד של מסד נתונים מקומי (`SQLite`) באפליקציית .NET MAUI!

בפרויקט זה תלמדו מאפס כיצד לתכנן, לבנות, לקשר ולתשאל מסד נתונים מקומי אמיתי מרובה-טבלאות (`Multi-Table Database`), המשמש לניהול מורים, קורסים, תלמידים ורישומים לציונים.

---

## 🎯 מטרות הפרויקט והלמידה
1. הבנת מסד נתונים יחסי (Relational DB) בטלפון הנייד: מפתחות ראשיים (`Primary Key`), מפתחות זרים (`Foreign Key`), ואינדקסים.
2. הגדרת ישויות וקישוטים (Decorations / Attributes) של `sqlite-net-pcl` ו-`SQLiteNetExtensions`.
3. מימוש קשרי גומלין:
   - **קשר 1:N (יחיד לרבים):** מורה אחד מלמד מספר קורסים (`Teacher` -> `Course`).
   - **קשר M:N (רבים לרבים):** תלמיד רשום למספר קורסים, ובכל קורס רשומים מספר תלמידים, דרך טבלת ביניים (`Student` <-> `Course` דרך `Enrollment`).
4. מימוש שכבת שירות מסד נתונים (`DatabaseService`) מבודדת באמצעות ממשק (`IDatabaseService`) ו-Dependency Injection.
5. שליטה מלאה בפעולות CRUD (`Create`, `Read`, `Update`, `Delete`) אסינכרוניות ללא חסימת ממשק המשתמש (UI).
6. שליטה בשאילתות מתקדמות:
   - **JOIN:** חיבור 3 טבלאות להפקת גיליון ציונים אישי של תלמיד.
   - **GROUP BY + אגרגציות:** חישוב ממוצע ציונים (`AVG`), כמות תלמידים (`COUNT`), וציון שיא (`MAX`) לכל קורס.
   - **SUM:** חישוב סך נקודות זכות אקדמיות עבור תלמיד.
   - **SKIP & TAKE:** דפדוף (Pagination) ברשימות ארוכות כדי לחסוך בזיכרון ולשפר ביצועים.

---

## 🌳 מפת ה-Branches ב-Git — כיצד לעבוד ולבדוק את עצמכם?

במאגר זה הוכנו עבורכם ענפים ייעודיים לכל שלב. ענף ה-`main` הוא סביבת העבודה שלכם (ה-Starter Workspace), וכל ענף מייצג את הפתרון המלא והבדוק לאותו שלב:

```text
main                                    <-- 🚀 כאן אתם עובדים! (תשתית UI מוכנה, מקום למימוש ה-DB)
 ├── step-01-packages-and-models        <-- 📦 שלב 1: התקנת חבילות והגדרת 4 הישויות
 ├── step-02-db-connection-and-basic-crud <-- 🔌 שלב 2: התחברות ו-CRUD בסיסי
 ├── step-03-relationships              <-- 🔗 שלב 3: קשרי 1:N ו-M:N עם SQLiteNetExtensions
 ├── step-04-queries-aggregations-join  <-- 📊 שלב 4: שאילתות מורכבות (JOIN, GROUP BY, COUNT, SUM, SKIP)
 └── step-05-full-solution              <-- 🏆 הפתרון המלא והסופי (קוד מלא + בדיקות יחידה)
```

> **טיפ מעשי במעבר בין ענפים:**
> אם נתקעתם או תרצו לראות כיצד הקוד צריך להיראות בסוף שלב מסוים, ניתן לפתוח מסוף ולהריץ:
> ```bash
> git checkout step-01-packages-and-models
> ```
> וכשתרצו לחזור להמשך העבודה שלכם ב-`main`:
> ```bash
> git checkout main
> ```

---

## 🧭 סדר העבודה שלב-אחר-שלב

| שלב | נושא | קובץ הדרכה מפורט | מה עליכם לממש? |
| :--- | :--- | :--- | :--- |
| **שלב 1** | חבילות ומודלים | [01-Nuget-Packages-And-Models.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/01-Nuget-Packages-And-Models.md) | התקנת החבילות היציבות (Stable) דרך ממשק ה-NuGet הגרפי (Manage NuGet Packages) ב-Visual Studio או ב-Rider (ללא Prerelease), והגדרת הקישוטים (Attributes) במודלים `Teacher`, `Course`, `Student`, `Enrollment`. |
| **שלב 2** | חיבור ו-CRUD | [02-Database-Connection-And-Basic-CRUD.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/02-Database-Connection-And-Basic-CRUD.md) | יצירת `DatabaseService`, הגדרת נתיב הקובץ ב-`AppDataDirectory`, יצירת הטבלאות, ומימוש שמירה, שליפה, עדכון ומחיקה של מורים ותלמידים. |
| **שלב 3** | קשרי גומלין | [03-Table-Relationships.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/03-Table-Relationships.md) | שימוש ב-`SQLiteNetExtensions.Async`: שליפת קורס יחד עם פרטי המורה שלו (`ManyToOne`), שליפת מורה עם רשימת הקורסים (`OneToMany`), ורישום תלמיד לקורס (`Enrollment`). |
| **שלב 4** | שאילתות ואגרגציה | [04-Advanced-Queries-Aggregations.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/04-Advanced-Queries-Aggregations.md) | כתיבת שאילתות SQL ו-LINQ המשלבות `JOIN`, `GROUP BY`, `COUNT`, `SUM`, `AVG`, וכן מימוש דפדוף בעזרת `Skip()` ו-`Take()`. |
| **שלב 5** | אימות ובדיקות | בדיקה והרצה | הרצת בדיקות היחידה (`dotnet test`) והפעלת האפליקציה במסך הדוחות לווידוא שהכל פועל ומציג נתונים אמיתיים! |

בהצלחה במסע! עברו כעת לקובץ [01-Nuget-Packages-And-Models.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/01-Nuget-Packages-And-Models.md) כדי להתחיל בשלב 1.
