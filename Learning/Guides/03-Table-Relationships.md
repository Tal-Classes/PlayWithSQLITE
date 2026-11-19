# 🔗 שלב 3: מימוש קשרי גומלין במסד נתונים (1:N ו-M:N)

במערכות אמיתיות, נתונים אינם שמורים בטבלה בודדת מנותקת. בשלב זה נלמד כיצד לממש שני סוגי קשרים מרכזיים באמצעות `SQLiteNetExtensions.Async`:
1. **קשר אחד-לרבים (1:N):** מורה אחד מלמד מספר קורסים.
2. **קשר רבים-לרבים (M:N):** תלמיד רשום למספר קורסים, ובכל קורס רשומים מספר תלמידים, באמצעות טבלת ביניים.

---

## 1. קשר 1:N (One-to-Many) — מורה וקורסים

במודל שלנו:
- לכל `Course` יש עמודת `TeacherId` שהיא מפתח זר (`[ForeignKey(typeof(Teacher))]`).
- לכל `Course` יש מאפיין ניווט `[ManyToOne] public Teacher Teacher { get; set; }`.
- לכל `Teacher` יש מאפיין ניווט של רשימה `[OneToMany] public List<Course> Courses { get; set; }`.

### שליפת מורה יחד עם כל הקורסים שלו:
אם נשתמש ב-`Table<Teacher>().FirstOrDefaultAsync(t => t.Id == id)`, רשימת `Courses` תחזור ריקה (מכיוון ש-SQLite הבסיסי אינו טוען אוטומטית אובייקטים מקושרים).  
כדי לטעון את הקורסים המקושרים, נשתמש ב-`GetWithChildrenAsync`:

```csharp
using SQLiteNetExtensionsAsync.Extensions;

// טעינת המורה וכל הקורסים המקושרים אליו בפקודה בודדת
public async Task<Teacher?> GetTeacherWithCoursesAsync(int teacherId)
{
    await EnsureInitializedAsync();
    return await _database.GetWithChildrenAsync<Teacher>(teacherId);
}
```

### שליפת כל הקורסים כולל פרטי המורה של כל קורס:
```csharp
public async Task<List<Course>> GetCoursesWithTeachersAsync()
{
    await EnsureInitializedAsync();
    return await _database.GetAllWithChildrenAsync<Course>();
}
```

---

## 2. קשר M:N (Many-to-Many) — תלמידים וקורסים

במסד נתונים יחסי, לא ניתן לחבר ישירות קשר M:N בין שתי טבלאות. הפתרון התקני הוא **טבלת קישור** (Junction Table / Associative Table).

במערכת שלנו, טבלת הקישור היא `Enrollments`:
```text
┌──────────────┐          ┌────────────────┐          ┌──────────────┐
│   Students   │ 1      N │  Enrollments   │ N      1 │   Courses    │
│──────────────│──────────│────────────────│──────────│──────────────│
│ Id (PK)      │          │ Id (PK)        │          │ Id (PK)      │
│ FirstName    │          │ StudentId (FK) │          │ Title        │
│ LastName     │          │ CourseId (FK)  │          │ TeacherId(FK)│
│ Email        │          │ Grade          │          │ Credits      │
│ GradeLevel   │          │ EnrollmentDate │          │              │
└──────────────┘          └────────────────┘          └──────────────┘
```

### הגדרת הקשר במודלים:
במחלקת `Student.cs`:
```csharp
[ManyToMany(typeof(Enrollment), CascadeOperations = CascadeOperation.All)]
public List<Course> Courses { get; set; } = new();
```
ובמחלקת `Course.cs`:
```csharp
[ManyToMany(typeof(Enrollment), CascadeOperations = CascadeOperation.All)]
public List<Student> Students { get; set; } = new();
```

### רישום תלמיד לקורס:
כדי לרשום תלמיד לקורס (כולל הזנת ציון ראשוני), מוסיפים רשומה לטבלת `Enrollment`:
```csharp
public async Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null)
{
    await EnsureInitializedAsync();

    // בדיקה האם התלמיד כבר רשום לקורס
    var existing = await _database.Table<Enrollment>()
        .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

    if (existing != null)
    {
        // עדכון ציון אם כבר רשום
        existing.Grade = grade;
        return await _database.UpdateAsync(existing);
    }

    var enrollment = new Enrollment
    {
        StudentId = studentId,
        CourseId = courseId,
        Grade = grade,
        EnrollmentDate = DateTime.Now,
        IsPaid = true
    };

    return await _database.InsertAsync(enrollment);
}
```

---

## 3. מחיקה מדורגת (Cascading Delete)

כאשר מוחקים ישות הורה (למשל מורה), מה קורה לקורסים שהיו משויכים אליו?
- אם לא נמחק את הקורסים או לא נעדכן אותם, נישאר עם רשומות "יתומות" (`Orphan Records`) שמצביעות למפתח זר של מורה שאינו קיים.
- לכן ב-`DeleteTeacherAsync` מחקנו תחילה את הקורסים או השתמשנו ב-`CascadeOperations = CascadeOperation.All`.

עברו לשלב הבא: [04-Advanced-Queries-Aggregations.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/04-Advanced-Queries-Aggregations.md).
