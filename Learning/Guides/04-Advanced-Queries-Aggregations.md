# 📊 שלב 4: פירוק שאילתות SQL מתקדמות — סדר פקודות, שלבי `JOIN`, `GROUP BY`, אגרגציות ודפדוף

בשלב זה נלמד **לחשוב כמו מנוע SQL**: בכל שאילתה נתחיל מהצגת **טבלת היעד הסופית** שאנו רוצים לקבל, נבחן את **טבלאות הבסיס** (`SELECT *`), ואז נראה **צעד-אחר-צעד** איך כל `JOIN`, `WHERE` ו-`GROUP BY` בונה את התוצאה!

---

## 1. סדר הכתיבה מול סדר הביצוע הלוגי במנוע SQL

**כלל מפתח:** הסדר שבו *כותבים* את השאילתה בקוד אינו הסדר שבו מנוע SQLite *מבצע* אותה!

| פקודה בקוד (סדר כתיבה) | **סדר ביצוע אמיתי** | מה המנוע עושה בשלב זה? |
| :--- | :---: | :--- |
| `SELECT ...` | **6** | בוחר את העמודות הסופיות, מחשב פונקציות צבירה (`COUNT`, `SUM`, `AVG`) ומעניק כינויים (`AS`). |
| `FROM ...` | **1** | טוען את טבלת הבסיס הראשונה. |
| `INNER / LEFT JOIN ... ON ...` | **2** | מחבר שורות מטבלאות נוספות לפי מפתח משותף ויוצר **טבלה וירטואלית מורחבת**. |
| `WHERE ...` | **3** | מסנן **שורות בודדות** *לפני* הקיבוץ (אי אפשר לכתוב כאן `AVG` או `COUNT`!). |
| `GROUP BY ...` | **4** | מאחד שורות בעלות ערך זהה (למשל אותו `CourseId`) ל**קבוצות (דליים)**. |
| `HAVING ...` | **5** | מסנן **קבוצות שלמות** *אחרי* הקיבוץ (כאן כן מותר `HAVING COUNT(e.Id) >= 2`). |
| `ORDER BY ...` | **7** | ממיין את השורות הסופיות (`ASC` / `DESC`). |
| `LIMIT ... OFFSET ...` | **8** | חותך רק את עמוד התוצאות המבוקש (מקביל ל-`Take` ו-`Skip` ב-LINQ). |

---

## 2. פירוק שאילתת `JOIN` מרובת-טבלאות (`GetStudentTranscriptAsync`)

### 🎯 שלב 0 — הטבלה הסופית שאנחנו רוצים לקבל (עבור `studentId = 1`, נועם אברהם)
זהו גיליון הציונים האישי שמוצג במסך התלמידים (ממופה ל-DTO `StudentTranscriptItem`):

| `StudentId` | `StudentName` | `CourseTitle` | `Credits` | `TeacherName` | `Grade` | `IsPaid` |
| :---: | :--- | :--- | :---: | :--- | :---: | :---: |
| 1 | נועם אברהם | סדנת בחירה | 2 | לא הוקצה מורה | 88 | true |
| 1 | נועם אברהם | תכנות ב-C# ו-MAUI | 5 | ישראל ישראלי | 95 | true |

### 📋 שלב 1 — חומרי הגלם: טבלאות הבסיס (`SELECT *`)
הנתונים מפוזרים ב-4 טבלאות נפרדות:

**`SELECT * FROM Enrollments`:**
| `Id` | `StudentId` | `CourseId` | `Grade` | `IsPaid` |
| :---: | :---: | :---: | :---: | :---: |
| 10 | **1** | **101** | 95 | true |
| 11 | **1** | **102** | 88 | true |
| 12 | **2** | **101** | 79 | false |

**`SELECT * FROM Students`:**
| `Id` | `FirstName` | `LastName` |
| :---: | :--- | :--- |
| **1** | נועם | אברהם |
| **2** | מאיה | שלום |

**`SELECT * FROM Courses`:**
| `Id` | `Title` | `Credits` | `TeacherId` |
| :---: | :--- | :---: | :---: |
| **101** | תכנות ב-C# ו-MAUI | 5 | **7** |
| **102** | סדנת בחירה | 2 | **0** *(ללא מורה)* |

**`SELECT * FROM Teachers`:**
| `Id` | `FullName` |
| :---: | :--- |
| **7** | ישראל ישראלי |

---

### 🪜 שלב 2 — בניית ה-`JOIN` צעד-אחר-צעד (מה נוצר בכל שלב!)

#### צעד א': `FROM Enrollments e INNER JOIN Students s ON e.StudentId = s.Id`
מחברים כל רישום לשורת התלמיד המתאימה. רק שורות שיש להן התאמה בשני הצדדים ממשיכות (`INNER JOIN`).  
**הטבלה הזמנית שנוצרה:**
| `e.Id` | `s.Id` | `s.FirstName` | `s.LastName` | `e.CourseId` | `e.Grade` |
| :---: | :---: | :--- | :--- | :---: | :---: |
| 10 | 1 | נועם | אברהם | **101** | 95 |
| 11 | 1 | נועם | אברהם | **102** | 88 |
| 12 | 2 | מאיה | שלום | **101** | 79 |

#### צעד ב': הוספת `INNER JOIN Courses c ON e.CourseId = c.Id`
כעת מוסיפים לטבלה הזמנית את שם הקורס, נקודות הזכות ומזהה המורה (`c.TeacherId`):
| `s.FirstName` | `c.Title` | `c.Credits` | `c.TeacherId` | `e.Grade` |
| :--- | :--- | :---: | :---: | :---: |
| נועם | תכנות ב-C# ו-MAUI | 5 | **7** | 95 |
| נועם | סדנת בחירה | 2 | **0** | 88 |
| מאיה | תכנות ב-C# ו-MAUI | 5 | **7** | 79 |

#### צעד ג': הוספת `LEFT JOIN Teachers t ON c.TeacherId = t.Id` (מדוע `LEFT` ולא `INNER`?)
שימו לב לקורס "סדנת בחירה" (`c.TeacherId = 0`, אין לו מורה תואם ב-`Teachers`):
- ב-`INNER JOIN` השורה הזו **הייתה נמחקת ונעלמת** מהגיליון של נועם!
- ב-**`LEFT JOIN`** השורה **נשמרת**, ובעמודת `t.FullName` מתקבל `NULL` — שאותו אנו מחליפים באמצעות `IFNULL(t.FullName, 'לא הוקצה מורה')`:

| `s.FirstName` | `c.Title` | `c.Credits` | `t.FullName` | `e.Grade` |
| :--- | :--- | :---: | :--- | :---: |
| נועם | תכנות ב-C# ו-MAUI | 5 | ישראל ישראלי | 95 |
| נועם | סדנת בחירה | 2 | `NULL` ➡️ `'לא הוקצה מורה'` | 88 |
| מאיה | תכנות ב-C# ו-MAUI | 5 | ישראל ישראלי | 79 |

#### צעד ד': סינון (`WHERE e.StudentId = ?`), בחירת עמודות (`SELECT ... AS`) ומיון (`ORDER BY c.Title`)
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
> **הגנה מ-SQL Injection ומיפוי ל-DTO:**  
> מעבירים את `studentId` כפרמטר בטוח לסימן השאלה (`?`): `await _database!.QueryAsync<StudentTranscriptItem>(query, studentId);` — ושמות הכינויים (`AS CourseTitle` וכו') חייבים להתאים בדיוק לשמות ה-Properties במחלקת `StudentTranscriptItem`!

---

## 3. פירוק שאילתת `GROUP BY` ואגרגציות (`GetCourseGradeSummariesAsync`)

### 🎯 שלב 0 — הטבלה הסופית שאנחנו רוצים לקבל (דוח הקורסים במסך הדוחות)
שורה אחת מסכמת לכל קורס (כולל קורסים שעדיין אין בהם אף תלמיד!):

| `CourseId` | `CourseTitle` | `TeacherName` | `StudentCount` | `AverageGrade` | `MaxGrade` | `MinGrade` |
| :---: | :--- | :--- | :---: | :---: | :---: | :---: |
| 104 | רובוטיקה | דוד כהן | **0** | 0.0 | 0.0 | 0.0 |
| 101 | תכנות C# | ישראל ישראלי | **2** | 87.0 | 95.0 | 79.0 |

### 🪜 שלב 1 — מה נוצר אחרי `FROM Courses c LEFT JOIN Teachers t ... LEFT JOIN Enrollments e ...` (**לפני** `GROUP BY`)?
| `c.Id` | `c.Title` | `t.FullName` | `e.Id` | `e.Grade` |
| :---: | :--- | :--- | :---: | :---: |
| 101 | תכנות C# | ישראל ישראלי | 10 | 95 |
| 101 | תכנות C# | ישראל ישראלי | 12 | 79 |
| 104 | רובוטיקה *(אין עדיין נרשמים)* | דוד כהן | **`NULL`** | **`NULL`** |

### 🪜 שלב 2 — מה עושה `GROUP BY c.Id, c.Title, t.FullName`?
המנוע מאחד את השורות לפי קורס לשני "דליים" ומריץ עליהם את פונקציות הצבירה:
- **דלי `101` (`תכנות C#`)**: 2 שורות (`e.Id = 10, 12`, ציונים `95, 79`) ➡️ `COUNT(e.Id) = 2`, `AVG = 87.0`, `MAX = 95.0`, `MIN = 79.0`.
- **דלי `104` (`רובוטיקה`)**: שורה אחת שבה `e.Id = NULL` ➡️ `COUNT(e.Id) = 0`, `IFNULL(AVG(e.Grade), 0.0) = 0.0`.

> [!CAUTION]
> **מלכודת קלאסית בבגרות: מדוע `COUNT(e.Id)` ולא `COUNT(*)` ב-`LEFT JOIN`?**  
> - `COUNT(*)` סופר **שורות** (כולל שורה שבה הרישום הוא `NULL`), ולכן היה מחזיר על `רובוטיקה` **1** במקום **0**!  
> - `COUNT(e.Id)` סופר רק ערכים שאינם `NULL`, ולכן מחזיר במדויק **0** תלמידים!

### השאילתה השלמה:
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

## 4. שתי שאילתות אגרגציה נוספות + דפדוף (`Skip` & `Take`)

### א. התפלגות תלמידים לפי שכבה (`GetStudentDistributionByGradeLevelAsync`)
- **היעד:** טבלה עם 2 עמודות (`GradeLevel`, `StudentCount`) — שורה אחת לכל שכבה (`10`, `11`, `12`).
- **השאילתה:** `SELECT GradeLevel, COUNT(Id) AS StudentCount FROM Students GROUP BY GradeLevel ORDER BY GradeLevel;` מופעלת עם `QueryAsync<GradeLevelCount>(query)`.

### ב. חישוב סך נקודות זכות לתלמיד (`GetTotalCreditsForStudentAsync`) — ערך סקלרי בודד (`SUM`)
- **היעד:** מספר שלם אחד (`int`), למשל `9` (5 נ"ז + 4 נ"ז).
- **השלבים:** `FROM Enrollments e` ➡️ `INNER JOIN Courses c ON e.CourseId = c.Id` ➡️ `WHERE e.StudentId = ?` ➡️ `SELECT IFNULL(SUM(c.Credits), 0)`.
- **איך מריצים ב-C#?** כשרוצים לקבל **ערך בודד אחד** (לא רשימה!), משתמשים ב-**`await _database!.ExecuteScalarAsync<int>(query, studentId)`**!

### ג. דפדוף (`Pagination`) באמצעות `OrderBy`, `Skip` ו-`Take`
כדי לא לטעון אלפי תלמידים בבת אחת, שולפים עמוד אחד בכל פעם:
```csharp
public async Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20)
{
    await EnsureInitializedAsync();
    return await _database!.Table<Student>()
        .OrderBy(s => s.LastName)     // חובה למיין לפני Skip/Take כדי שהעמודים יהיו עקביים!
        .ThenBy(s => s.FirstName)
        .Skip(skip)                   // מתורגם ל-OFFSET ב-SQL
        .Take(take)                   // מתורגם ל-LIMIT ב-SQL
        .ToListAsync();
}
```

---

## 5. ✍️ משימתכם המעשית בשלב 4
פתחו את `DatabaseService.cs` והשלימו את 5 המתודות הבאות:
1. הוסיפו `.Skip(skip).Take(take)` לתוך `GetStudentsAsync`.
2. ממשו את `GetCourseGradeSummariesAsync()` עם `QueryAsync<CourseGradeSummary>`.
3. ממשו את `GetStudentTranscriptAsync(int studentId)` עם `QueryAsync<StudentTranscriptItem>`.
4. ממשו את `GetStudentDistributionByGradeLevelAsync()` עם `QueryAsync<GradeLevelCount>`.
5. ממשו את `GetTotalCreditsForStudentAsync(int studentId)` עם `ExecuteScalarAsync<int>`.

### ✅ בדיקה עצמית בסיום שלב 4:
- הסירו את כל ה-`Skip = "..."` שנותרו ב-`DatabaseServiceTests.cs` והריצו את הבדיקות (ראו [00b-How-To-Run-Tests.pdf](./00b-How-To-Run-Tests.pdf)).
- **כל 9 הבדיקות צריכות לעבור בירוק (`succeeded: 9, failed: 0`)!**
- להשוואה מול הפתרון המוכן: `git checkout step-04-queries-aggregations-join`.

עברו לשלב הבא: [05-DI-And-ViewModel-Commands.pdf](./05-DI-And-ViewModel-Commands.pdf).
