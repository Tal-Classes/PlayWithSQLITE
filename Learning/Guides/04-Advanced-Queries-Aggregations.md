# 📊 שלב 4: שאילתות מורכבות — JOIN, GROUP BY, אגרגציות ודפדוף

בשלב זה נלמד כיצד לנצל את מלוא העוצמה של מנוע SQL ב-SQLite כדי להפיק דוחות מורכבים, לבצע חישובים סטטיסטיים, ולממש דפדוף (Pagination) מקצועי.

---

## 1. חיבור טבלאות (JOIN)

פקודת `JOIN` מאפשרת לשלב נתונים ממספר טבלאות שונות על בסיס מפתחות משותפים (`Primary Key` מול `Foreign Key`).

### סוגי ה-JOIN המרכזיים:
- **`INNER JOIN`**: מחזיר רשומות רק כאשר קיימת התאמה מלאה בין שתי הטבלאות.
- **`LEFT JOIN` (או `LEFT OUTER JOIN`)**: מחזיר את כל הרשומות מהטבלה השמאלית, גם אם בטבלה הימנית אין רשומה תואמת (במקרה כזה העמודות מהטבלה הימנית יכילו ערכי `NULL`).

### דוגמה: הפקת גיליון ציונים של תלמיד (חיבור 3 טבלאות)
כדי להציג את רשימת הקורסים של תלמיד יחד עם שמות הקורסים ושמות המורים:
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

בקוד C#:
```csharp
public async Task<List<StudentTranscriptItem>> GetStudentTranscriptAsync(int studentId)
{
    await EnsureInitializedAsync();
    return await _database.QueryAsync<StudentTranscriptItem>(query, studentId);
}
```

---

## 2. קיבוץ ופונקציות צבירה (GROUP BY & Aggregations)

פונקציות צבירה מבצעות חישוב על קבוצת שורות ומחזירות תוצאה מספרית אחת:
- **`COUNT(column)`**: סופר את מספר השורות בקבוצה.
- **`SUM(column)`**: מחשב את סכום הערכים בעמודה.
- **`AVG(column)`**: מחשב את הממוצע החשבוני של הערכים בעמודה.
- **`MAX(column)` / `MIN(column)`**: מוצא את הערך הגבוה ביותר / הנמוך ביותר.

### דוגמה 1: ספירת תלמידים לפי שכבת לימוד (`GROUP BY` + `COUNT`)
```sql
SELECT 
    GradeLevel,
    COUNT(Id) AS StudentCount
FROM Students
GROUP BY GradeLevel
ORDER BY GradeLevel;
```

### דוגמה 2: דוח ציונים מסכם לקורסים (`LEFT JOIN` + `GROUP BY` + `AVG` + `COUNT` + `MAX` + `MIN`)
בדוח זה אנו רוצים לראות עבור כל קורס:
1. כמה תלמידים רשומים אליו.
2. מה ממוצע הציונים בקורס.
3. מה הציון הגבוה ביותר.
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

### דוגמה 3: חישוב סך נקודות הזכות של תלמיד (`SUM`)
כאשר רוצים לקבל ערך מספרי בודד (Scalar) משאילתת SQL, נשתמש ב-`ExecuteScalarAsync`:
```csharp
public async Task<int> GetTotalCreditsForStudentAsync(int studentId)
{
    await EnsureInitializedAsync();
    const string query = @"
        SELECT IFNULL(SUM(c.Credits), 0)
        FROM Enrollments e
        INNER JOIN Courses c ON e.CourseId = c.Id
        WHERE e.StudentId = ?;
    ";
    return await _database.ExecuteScalarAsync<int>(query, studentId);
}
```

---

## 3. דפדוף מהיר באמצעות `SKIP` ו-`TAKE` (Pagination)

במערכות עם מאות או אלפי תלמידים, שליפת כל הרשומות בבת אחת (`SELECT * FROM Students`) מעמיסה על הזיכרון ומאיטה את האפליקציה.  
הפתרון הוא **דפדוף (Pagination)**: שליפת מנה קבועה של רשומות (למשל 5 או 20 תלמידים לעמוד).

### כיצד זה עובד?
- **`Take(pageSize)`**: כמה שורות לקחת (תואם לפקודת `LIMIT` ב-SQL).
- **`Skip(skipCount)`**: כמה שורות לדלג מתחילת הטבלה (תואם לפקודת `OFFSET` ב-SQL).

נוסחת החישוב לדילוג:
```csharp
int skip = (currentPage - 1) * pageSize;
```

בקוד C# בעזרת LINQ:
```csharp
public async Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20)
{
    await EnsureInitializedAsync();
    return await _database.Table<Student>()
                          .OrderBy(s => s.LastName)
                          .ThenBy(s => s.FirstName)
                          .Skip(skip)
                          .Take(take)
                          .ToListAsync();
}
```
וחישוב מספר העמודים הכולל מתבסס על ספירת כל התלמידים:
```csharp
int totalStudents = await _database.Table<Student>().CountAsync();
int totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);
```

---

## 🎓 סיכום נקודות מפתח לבחינה בעל-פה (25% שליטה בקוד)
1. **שאלה:** מדוע השתמשנו ב-`LEFT JOIN` בדוח הקורסים ולא ב-`INNER JOIN`?  
   **תשובה:** כי ייתכנו קורסים שטרם נרשמו אליהם תלמידים או שטרם שובץ להם מורה. ב-`INNER JOIN` קורס כזה לא היה מופיע כלל בדוח! ב-`LEFT JOIN` הקורס יופיע עם 0 תלמידים וממוצע 0, כפי שמצופה בדוח מערכת תקין.
2. **שאלה:** למה חובה להשתמש ב-`OrderBy` לפני שמפעילים `Skip` ו-`Take`?  
   **תשובה:** ב-SQL סדר השורות בטבלה אינו מובטח. אם לא נגדיר מיון מפורש (`ORDER BY`), דילוג בעמודים שונים עלול להחזיר תוצאות לא עקביות או כפולות בין עמודים.
