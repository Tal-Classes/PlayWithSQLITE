# ספר הלמידה: מסד נתונים מקומי SQLite מרובה-טבלאות ב-.NET MAUI

ספר למידה זה מלווה את פרויקט `PlayWithSQLITE`. הוא מרכז את עקרונות הנדסת התוכנה, עיצוב הנתונים, ופקודות ה-SQL שנלמדו במסגרת פרויקט הבגרות ברמת 5 יחידות לימוד.

---

## 📚 פרק 1: תשתית מסד הנתונים ועיצוב הישויות (Models & SQLite Attributes)
- **מה נבנה:** הוספנו את החבילות `sqlite-net-pcl` ו-`SQLiteNetExtensions.Async`, ובנינו 4 מחלקות מודל: `Teacher`, `Course`, `Student`, `Enrollment`.
- **למה זה נחוץ:** ייצוג מדויק של עולם התוכן (בית ספר) במסד נתונים יחסי (Relational DB) המאפשר שמירה מקומית על גבי המכשיר הנייד ללא תלות ברשת.
- **מושגים שנלמדו:** `[Table]`, `[PrimaryKey]`, `[AutoIncrement]`, `[NotNull]`, `[Unique]`, `[Ignore]`, `[ForeignKey]`, `[OneToMany]`, `[ManyToMany]`.
- **מדריך מפורט:** [01-Nuget-Packages-And-Models.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/01-Nuget-Packages-And-Models.md).

---

## 📚 פרק 2: התחברות אסינכרונית ופעולות CRUD
- **מה נבנה:** מחלקת שירות `DatabaseService` המממשת את `IDatabaseService`. החיבור מנוהל באמצעות `SQLiteAsyncConnection` עם נתיב שמור ב-`FileSystem.AppDataDirectory`.
- **למה זה נחוץ:** הפרדת שכבות (Separation of Concerns) בין תצוגה (Views/ViewModels) לבין מסד הנתונים (Data Persistence). שמירה על תהליכון ה-UI חופשי ע"י קריאות `async/await`.
- **מושגים שנלמדו:** `CreateTableAsync`, `InsertAsync`, `UpdateAsync`, `DeleteAsync`, `Table<T>().ToListAsync()`.
- **מדריך מפורט:** [02-Database-Connection-And-Basic-CRUD.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/02-Database-Connection-And-Basic-CRUD.md).

---

## 📚 פרק 3: קשרי גומלין במסד נתונים (1:N ו-M:N)
- **מה נבנה:** 
  - קשר **1:N (יחיד לרבים):** מורה אחד מקושר למספר קורסים (`Teacher` -> `Courses`).
  - קשר **M:N (רבים לרבים):** תלמיד רשום למספר קורסים וקורס מכיל מספר תלמידים דרך טבלת קישור `Enrollment`.
- **למה זה נחוץ:** נרמול מסד נתונים ומניעת כפילויות, לצד יכולת לשמור מידע אודות הקשר עצמו (ציון, תאריך רישום, תשלום).
- **מושגים שנלמדו:** `GetWithChildrenAsync`, `GetAllWithChildrenAsync`, מפתחות זרים (`Foreign Key`), טבלאות ביניים (`Junction Table`).
- **מדריך מפורט:** [03-Table-Relationships.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/03-Table-Relationships.md).

---

## 📚 פרק 4: שאילתות מתקדמות, אגרגציות ודפדוף (JOIN, GROUP BY, SUM, SKIP)
- **מה נבנה:** 
  - שאילתת `INNER JOIN` ו-`LEFT JOIN` המשלבת 3 טבלאות להפקת גיליון ציונים אישי.
  - שאילתת `GROUP BY` עם פונקציות צבירה (`COUNT`, `AVG`, `MAX`, `MIN`) להפקת דוח קורסים מסכם.
  - שאילתת `SUM` לחישוב סך נקודות זכות אקדמיות.
  - מנגנון דפדוף (Pagination) באמצעות `Skip()` ו-`Take()` ב-LINQ.
- **למה זה נחוץ:** הפקת תובנות ודוחות מערכת מורכבים בפנייה מרוכזת ויעילה למסד הנתונים, ושיפור זמני טעינה וצריכת זיכרון ב-UI.
- **מדריך מפורט:** [04-Advanced-Queries-Aggregations.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/04-Advanced-Queries-Aggregations.md).

---

## 📚 פרק 5: מפת הענפים ב-Git ושלבי המשימה
מדריך המשימה המלא המנחה את התלמיד שלב אחר שלב נמצא בקובץ:
[00-Master-Assignment.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/00-Master-Assignment.md).
