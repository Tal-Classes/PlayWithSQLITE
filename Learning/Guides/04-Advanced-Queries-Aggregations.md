# 📊 שלב 4: פירוק שאילתות SQL מתקדמות — סדר פקודות, שלבי `JOIN`, `GROUP BY`, אגרגציות ודפדוף

בשלב זה נלמד **לחשוב כמו מנוע SQL**: לא רק "להעתיק שאילתה ארוכה", אלא להבין **איך בונים שאילתת `JOIN` ו-`GROUP BY` שלב אחר שלב**, מהי הטבלה הזמנית שנוצרת בכל שלב, מהו סדר הביצוע האמיתי של הפקודות, וכיצד מממשים דפדוף (`Skip`/`Take`).

---

## 1. סדר הכתיבה מול סדר הביצוע הלוגי ב-SQL (קריטי להבנה!)

אחד המפתחות הכי חשובים להבנת SQL הוא ש**הסדר שבו כותבים את השאילתה אינו הסדר שבו מנוע המסד מבצע אותה!**

| סדר הכתיבה בקוד | **סדר הביצוע האמיתי במנוע SQL** | מה המנוע עושה בשלב זה? |
| :--- | :---: | :--- |
| `SELECT ...` | **6** | בוחר את העמודות הסופיות, מפעיל פונקציות צבירה (`COUNT`, `SUM`, `AVG`) ומעניק כינויים (`AS`). |
| `FROM ...` | **1** | בוחר את טבלת הבסיס הראשונה שממנה מתחילים. |
| `INNER / LEFT JOIN ... ON ...` | **2** | מחבר שורות מטבלאות נוספות לפי תנאי ההתאמה (`ON`) ויוצר **טבלה וירטואלית מורחבת**. |
| `WHERE ...` | **3** | מסנן **שורות בודדות** מהטבלה המורחבת *לפני* הקיבוץ (אי אפשר לשים כאן `AVG`/`COUNT`!). |
| `GROUP BY ...` | **4** | מאחד שורות בעלות ערך זהה (למשל אותו `CourseId` או אותה `GradeLevel`) ל**קבוצות (דליים)**. |
| `HAVING ...` | **5** | מסנן **קבוצות שלמות** *אחרי* הקיבוץ (כאן כן מותר לכתוב למשל `HAVING COUNT(e.Id) >= 2`). |
| `ORDER BY ...` | **7** | ממיין את השורות הסופיות לפי סדר עולה (`ASC`) או יורד (`DESC`). |
| `LIMIT ... OFFSET ...` | **8** | חותך רק את חלון העמוד המבוקש (תואם ל-`Take` ו-`Skip` ב-LINQ). |

> [!TIP]
> **עכשיו מובן למה אי אפשר לכתוב `WHERE AVG(Grade) > 85`!**  
> כי `WHERE` רץ בשלב **3** — הרבה לפני ש-`GROUP BY` (שלב 4) ו-`AVG` (שלב 6) בכלל חושבו! סינון לפי תוצאת אגרגציה מתבצע תמיד ב-`HAVING` (שלב 5).

---

## 2. פירוק שלב-אחר-שלב של `INNER JOIN` ו-`LEFT JOIN` — מה נוצר בכל שלב?

נבנה יחד את שאילתת **גיליון הציונים האישי של התלמיד (`GetStudentTranscriptAsync`)**, ונראה בדיוק איזו טבלה זמנית נוצרת אחרי כל פקודת `JOIN`!

### שלב א' — טבלת הבסיס: `FROM Enrollments e`
טבלת `Enrollments` מחזיקה רק **מזהים מספריים** וציון — עדיין אין בה שמות של תלמידים, קורסים או מורים:

| `e.Id` | `e.StudentId` | `e.CourseId` | `e.Grade` | `e.IsPaid` |
| :---: | :---: | :---: | :---: | :---: |
| 10 | **1** | **101** | 95 | true |
| 11 | **1** | **102** | 88 | true |
| 12 | **2** | **101** | 79 | false |

### שלב ב' — הוספת `INNER JOIN Students s ON e.StudentId = s.Id`
מנוע ה-SQL לוקח כל שורה מ-`Enrollments` ומחפש את השורה התואמת ב-`Students` שבה `e.StudentId == s.Id`.  
בגלל שזה **`INNER JOIN`**, רק שורות שיש להן התאמה בשני הצדדים ממשיכות הלאה.  
**הטבלה הזמנית שנוצרה:**

| `e.Id` | `s.Id` | `s.FirstName` | `s.LastName` | `e.CourseId` | `e.Grade` |
| :---: | :---: | :--- | :--- | :---: | :---: |
| 10 | 1 | נועם | אברהם | **101** | 95 |
| 11 | 1 | נועם | אברהם | **102** | 88 |
| 12 | 2 | מאיה | שלום | **101** | 79 |

### שלב ג' — הוספת `INNER JOIN Courses c ON e.CourseId = c.Id`
כעת מנוע ה-SQL מחבר לטבלה הזמנית את טבלת `Courses` לפי `e.CourseId == c.Id`.  
**הטבלה הזמנית המורחבת שנוצרה:**

| `s.FirstName` | `c.Title` | `c.Credits` | `c.TeacherId` | `e.Grade` |
| :--- | :--- | :---: | :---: | :---: |
| נועם | תכנות ב-C# ו-MAUI | 5 | **7** | 95 |
| נועם | סדנת בחירה (ללא מורה משובץ) | 2 | **0** | 88 |
| מאיה | תכנות ב-C# ו-MAUI | 5 | **7** | 79 |

### שלב ד' — הוספת `LEFT JOIN Teachers t ON c.TeacherId = t.Id` (ולמה דווקא `LEFT`?)
שימו לב לשורה השנייה (`c.TeacherId = 0` — קורס שטרם שובץ לו מורה!):
- אילו היינו כותבים `INNER JOIN Teachers`, השורה של הקורס השני **הייתה נעלמת** מגיליון הציונים של נועם!
- בזכות **`LEFT JOIN Teachers`**, כל השורות מהצד השמאלי נשמרות, ובעמודת `t.FullName` מתקבל `NULL` כאשר אין התאמה:

| `s.FirstName` | `c.Title` | `c.Credits` | `t.FullName` | `e.Grade` |
| :--- | :--- | :---: | :--- | :---: |
| נועם | תכנות ב-C# ו-MAUI | 5 | ישראל ישראלי | 95 |
| נועם | סדנת בחירה | 2 | **`NULL`** ➡️ `IFNULL` הופך ל-'לא הוקצה מורה' | 88 |
| מאיה | תכנות ב-C# ו-MAUI | 5 | ישראל ישראלי | 79 |

### שלב ה' — סינון (`WHERE`), בחירת עמודות (`SELECT ... AS ...`) ומיון (`ORDER BY`)
כעת `WHERE e.StudentId = ?` מסנן רק את השורות של נועם (`Id=1`), ו-`SELECT` ממפה כל עמודה בעזרת `AS` **בדיוק לשמות המאפיינים ב-DTO `StudentTranscriptItem`**:

```sql
SELECT 
    s.Id AS StudentId,
    (s.FirstName || ' ' || s.LastName) AS StudentName,
    c.Title AS CourseTitle,
    c.Credits AS Credits,
    IFNULL(t.FullName, 'לא הוקצה מורה') AS TeacherName,
    e.Grade AS Grade,
    e.IsPaid AS IsPaid
FROM Enrollments e
INNER JOIN Students s ON e.StudentId = s.Id
INNER JOIN Courses c ON e.CourseId = c.Id
LEFT JOIN Teachers t ON c.TeacherId = t.Id
WHERE e.StudentId = ?
ORDER BY c.Title;
```

> [!IMPORTANT]
> **הגנה מפני SQL Injection ומיפוי ל-DTO:**
> - שימו לב לסימן השאלה **`?`** במקום שרשור מחרוזות (`$"WHERE e.StudentId = {studentId}"`)! העברת `studentId` כפרמטר ל-`QueryAsync<StudentTranscriptItem>(query, studentId)` מונעת הזרקת SQL.
> - שמות הכינויים (`AS CourseTitle`, `AS TeacherName` וכו') **חייבים להיות זהים** לשמות ה-Properties במחלקת `StudentTranscriptItem` כדי ש-`sqlite-net-pcl` ידע למלא אותם אוטומטית!

---

## 3. פירוק שלב-אחר-שלב של `GROUP BY` ואגרגציות (`COUNT`, `AVG`, `MAX`, `MIN`, `SUM`)

כעת נפרק את שאילתת **דוח הציונים המסכם לכל קורס (`GetCourseGradeSummariesAsync`)** — השאילתה החשובה ביותר במסך הדוחות!

### א. המטרה
לכל קורס בבית הספר (גם אם עדיין **אף תלמיד לא נרשם אליו!**) נרצה שורה אחת מסכמת המציגה: שם הקורס, שם המורה, **מספר התלמידים הרשומים (`COUNT`)**, **ממוצע (`AVG`)**, **ציון שיא (`MAX`)** ו**ציון מינימלי (`MIN`)**.

### ב. מה נוצר אחרי שלבי `FROM` ו-`LEFT JOIN` (לפני `GROUP BY`)?
```sql
FROM Courses c
LEFT JOIN Teachers t ON c.TeacherId = t.Id
LEFT JOIN Enrollments e ON c.Id = e.CourseId
```
תוצאת ביניים (לפני הקיבוץ — שורה לכל צירוף של קורס + רישום):

| `c.Id` | `c.Title` | `t.FullName` | `e.Id` | `e.Grade` |
| :---: | :--- | :--- | :---: | :---: |
| 101 | תכנות C# | ישראל ישראלי | 10 | 95 |
| 101 | תכנות C# | ישראל ישראלי | 12 | 79 |
| 104 | רובוטיקה *(קורס חדש ללא נרשמים)* | דוד כהן | **`NULL`** | **`NULL`** |

### ג. מה קורה ברגע שמפעילים `GROUP BY c.Id, c.Title, t.FullName`?
מנוע ה-SQL אוסף את כל השורות בעלות אותו `c.Id` לתוך **דלי (קבוצה) אחד**, ואז מפעיל את פונקציות הצבירה על כל דלי בנפרד:
- **דלי קורס 101 (`תכנות C#`):** מכיל 2 שורות (`e.Id = 10, 12`; ציונים `95, 79`):
  * `COUNT(e.Id)` = **2**
  * `AVG(e.Grade)` = **(95 + 79) / 2 = 87.0**
  * `MAX(e.Grade)` = **95.0**, `MIN(e.Grade)` = **79.0**
- **דלי קורס 104 (`רובוטיקה`):** מכיל שורה אחת עם `e.Id = NULL` ו-`e.Grade = NULL`:
  * `COUNT(e.Id)` = **0** (כי `COUNT(column)` מתעלם מ-`NULL`!)
  * `IFNULL(AVG(e.Grade), 0.0)` = **0.0** (בלעדי `IFNULL`, המיפוי של `NULL` לתוך `double` ב-C# היה עלול להיכשל או לבלבל!).

> [!CAUTION]
> **מלכודת קלאסית בבגרות: `COUNT(e.Id)` מול `COUNT(*)` ב-`LEFT JOIN`!**
> - `COUNT(*)` סופר **שורות** (כולל שורות שבהן הצד הימני הוא `NULL`), ולכן על קורס `רובוטיקה` הריק הוא היה מחזיר בטעות **1** תלמיד!
> - `COUNT(e.Id)` סופר רק ערכים שאינם `NULL` בעמודת `e.Id`, ולכן מחזיר במדויק **0** תלמידים!

### ד. השאילתה השלמה והמלאה:
```sql
SELECT 
    c.Id AS CourseId,
    c.Title AS CourseTitle,
    IFNULL(t.FullName, 'לא הוקצה מורה') AS TeacherName,
    COUNT(e.Id) AS StudentCount,
    IFNULL(AVG(e.Grade), 0.0) AS AverageGrade,
    IFNULL(MAX(e.Grade), 0.0) AS MaxGrade,
    IFNULL(MIN(e.Grade), 0.0) AS MinGrade
FROM Courses c
LEFT JOIN Teachers t ON c.TeacherId = t.Id
LEFT JOIN Enrollments e ON c.Id = e.CourseId
GROUP BY c.Id, c.Title, t.FullName
ORDER BY c.Title;
```

---

## 4. עוד שתי אגרגציות חיוניות: התפלגות (`COUNT`) וסכום סקלרי (`SUM`)

### א. התפלגות תלמידים לפי שכבה (`GetStudentDistributionByGradeLevelAsync`)
מקבצים את טבלת `Students` לפי `GradeLevel` וסופרים את `Id` בכל שכבה:
```sql
SELECT 
    GradeLevel,
    COUNT(Id) AS StudentCount
FROM Students
GROUP BY GradeLevel
ORDER BY GradeLevel;
```

### ב. סך נקודות זכות לתלמיד (`GetTotalCreditsForStudentAsync`) — `INNER JOIN` + `SUM` + `ExecuteScalarAsync`
כאשר השאילתה מחזירה **ערך מספרי בודד** (ולא רשימת אובייקטים), משתמשים במתודה **`ExecuteScalarAsync<int>`**:
1. מתחילים מ-`FROM Enrollments e`.
2. מחברים `INNER JOIN Courses c ON e.CourseId = c.Id` כדי להגיע לעמודת `c.Credits`.
3. מסננים `WHERE e.StudentId = ?`.
4. מחשבים `SELECT IFNULL(SUM(c.Credits), 0)` — ואם התלמיד אינו רשום לאף קורס, `IFNULL` מחזיר `0` במקום `NULL`.

---

## 5. דפדוף מהיר (`Pagination`) באמצעות `OrderBy`, `Skip` ו-`Take`

כאשר בטבלה יש מאות או אלפי תלמידים, שליפת כולם בבת אחת מכבידה על הזיכרון ועל ממשק המשתמש. במקום זאת שולפים **עמוד אחד בכל פעם**:
- **`OrderBy(s => s.LastName).ThenBy(s => s.FirstName)`** — **חובה למיין קודם!** ללא מיון מפורש, סדר השורות ב-SQL אינו מובטח ותלמידים עלולים לחזור על עצמם בין עמודים.
- **`Skip(skip)`** — מתורגם ל-`OFFSET` ב-SQL: על כמה שורות לדלג מתחילת התוצאה הממוינת.
- **`Take(take)`** — מתורגם ל-`LIMIT` ב-SQL: כמה שורות לקחת לעמוד הנוכחי.

נוסחת החישוב לעמוד `currentPage` (המתחיל מ-1) בגודל `pageSize`:
```csharp
int skip = (currentPage - 1) * pageSize;
int totalPages = Math.Max(1, (int)Math.Ceiling((double)totalStudents / pageSize));
```

---

## 6. משימתכם המעשית בשלב 4

פתחו את [DatabaseService.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE/Services/DatabaseService.cs) וממשו:
1. דפדוף ב-`GetStudentsAsync(int skip = 0, int take = 20)` וספירה ב-`GetStudentsCountAsync()`.
2. `GetCourseGradeSummariesAsync()` (`LEFT JOIN` + `GROUP BY` + `COUNT(e.Id)` + `AVG` + `MAX` + `MIN`).
3. `GetStudentTranscriptAsync(int studentId)` (חיבור 4 טבלאות עם פרמטר `?`).
4. `GetStudentDistributionByGradeLevelAsync()` (`GROUP BY GradeLevel` + `COUNT`).
5. `GetTotalCreditsForStudentAsync(int studentId)` (`INNER JOIN` + `SUM` + `ExecuteScalarAsync<int>`).

### ✅ בדיקה עצמית בסיום שלב 4:
- הסירו את כל ה-`Skip` שנותרו ב-[DatabaseServiceTests.cs](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE.Tests/DatabaseServiceTests.cs) והריצו `dotnet test` — **כל 9 הבדיקות צריכות לעבור (`succeeded: 9, failed: 0`)!**
- להשוואה מול הפתרון המוכן: `git checkout step-04-queries-aggregations-join`.

עברו לשלב הבא: [05-DI-And-ViewModel-Commands.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/05-DI-And-ViewModel-Commands.md).
