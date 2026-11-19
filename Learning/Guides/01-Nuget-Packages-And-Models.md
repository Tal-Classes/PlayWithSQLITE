# 📦 שלב 1: הוספת חבילות NuGet, הגדרת ישויות ודקורציות SQLite

בשלב זה נלמד כיצד להוסיף את חבילות ה-NuGet הסטנדרטיות הנדרשות לעבודה עם SQLite ב-.NET MAUI, וכיצד לעצב את מחלקות המודל (`Models`) תוך שימוש בדקורציות (Attributes) המגדירות את מבנה הטבלאות.

---

## 1. חבילות ה-NuGet המאושרות

בפרויקט נשתמש בשתי חבילות בלבד (עפ"י הנחיות משרד החינוך ו-Microsoft):
1. **`sqlite-net-pcl`**: חבילת הבסיס הקלה והאסינכרונית לניהול מסד נתונים מקומי. מספקת את המחלקה הראשית `SQLiteAsyncConnection` ודקורציות בסיסיות של טבלה ועמודות.
2. **`SQLiteNetExtensions.Async`**: הרחבה שמתלבשת על `sqlite-net-pcl` ומאפשרת לנהל קשרי גומלין (1:1, 1:N, M:N) בקלות באמצעות דקורציות ושיטות עזר כגון `GetWithChildrenAsync`.

### כיצד להתקין את החבילות באופן ויזואלי (Manage NuGet Packages)?

> [!IMPORTANT]
> **הקפידו להתקין אך ורק גרסה יציבה (Stable Release)!**
> ודאו שתיבת הסימון **"Include prerelease"** **איננה מסומנת**. התקנת גרסאות בטא/תצוגה מקדימה עלולה לגרום לשגיאות תאימות.

#### אפשרות א': Visual Studio (Windows / VS 2026 / VS 2022)
1. **פתיחת מנהל החבילות לפתרון:**
   - בתפריט העליון בחרו: `Tools` ➡️ `NuGet Package Manager` ➡️ **`Manage NuGet Packages for Solution...`**  
   *(או: קליק ימני על שם ה-Solution ב-Solution Explorer ובחירה ב-`Manage NuGet Packages for Solution...`)*.
2. **מעבר לעיון:** לחצו על הלשונית **Browse** (עיון).
3. **בדיקת תיבת Prerelease:** ודאו שתיבת **"Include prerelease"** לצד שורת החיפוש **מכובה (Unchecked)**.
4. **התקנת חבילת הבסיס:**
   - בשורת החיפוש הקלידו: `sqlite-net-pcl`
   - בחרו בחבילה הרשמית מאת `Frank A. Krueger` / `Eric Sink`.
   - בחלונית הימנית, סמנו ב-V את פרויקט האפליקציה **`PlayWithSQLITE`** (אין צורך לסמן את פרויקט הבדיקות).
   - לחצו על כפתור **Install** ואשרו את חלון השינויים והרישיון.
5. **התקנת חבילת הקשרים:**
   - בשורת החיפוש הקלידו: `SQLiteNetExtensions.Async`
   - בחרו בחבילה הרשמית והיציבה (גרסה `2.1.0`).
   - סמנו את פרויקט **`PlayWithSQLITE`** ולחצו על **Install**.

#### אפשרות ב': JetBrains Rider (macOS / Mac)
1. **פתיחת חלון ה-NuGet:**
   - קליק ימני על שם הפרויקט **`PlayWithSQLITE`** ב-Solution Explorer ➡️ בחרו ב-**`Manage NuGet Packages`**  
   *(או בתפריט העליון: `Tools` ➡️ `NuGet` ➡️ `Manage NuGet Packages for Solution`)*.
2. **בדיקת תיבת Prerelease:** ודאו שהמתג **"Prerelease"** לצד שדה החיפוש **כבוי (אינו מסומן)**.
3. **התקנת החבילות:**
   - חפשו `sqlite-net-pcl`, סמנו את הפרויקט ולחצו על סימן הפלוס (`+`) או **Install**.
   - חפשו `SQLiteNetExtensions.Async` ולחצו על **Install**.

---

## 2. מילון הדקורציות המרכזיות (Attributes)

| דקורציה | מקור | משמעות ותפקיד |
| :--- | :--- | :--- |
| `[Table("TableName")]` | `SQLite` | מגדירה את שם הטבלה במסד הנתונים (מונע שמות ברירת מחדל לא רצויים). |
| `[PrimaryKey]` | `SQLite` | מסמנת את השדה כמפתח ראשי המזהה באופן חד-ערכי כל רשומה. |
| `[AutoIncrement]` | `SQLite` | קובעת שהמפתח הראשי ימוספר אוטומטית (1, 2, 3...) ע"י מסד הנתונים. |
| `[NotNull]` | `SQLite` | קובעת שהשדה הוא שדה חובה במסד ולא יכול להכיל ערך `NULL`. |
| `[Unique]` | `SQLite` | מבטיחה שלא יהיו שני ערכים זהים בעמודה (למשל כתובת דוא"ל). |
| `[MaxLength(N)]` | `SQLite` | מגבילה את אורך המחרוזת המרבי (למשל 50 או 100 תווים). |
| `[Ignore]` | `SQLite` | מורה למסד הנתונים **לא** לשמור שדה זה כעמודה בטבלה (שימושי לשדות מחושבים כגון `FullName`). |
| `[ForeignKey(typeof(T))]` | `SQLiteNetExtensions.Attributes` | מציינת ששדה מסוים הוא מפתח זר המצביע לטבלה אחרת. |
| `[OneToMany]` | `SQLiteNetExtensions.Attributes` | מגדירה קשר 1:N (יחיד לרבים) עבור רשימת אובייקטים מקושרים. |
| `[ManyToOne]` | `SQLiteNetExtensions.Attributes` | מגדירה קשר N:1 (רבים ליחיד) עבור אובייקט הורה בודד. |
| `[ManyToMany(typeof(LinkTable))]`| `SQLiteNetExtensions.Attributes` | מגדירה קשר M:N דרך טבלת קישור ייעודית. |

---

## 3. מבנה ארבעת הישויות במערכת בית הספר

### א. ישות המורה (`Teacher.cs`)
מייצגת מורה בבית הספר. לכל מורה יש מזהה, שם, אימייל, מחלקה, ורשימת קורסים אותם הוא מלמד (קשר 1:N):
```csharp
[Table("Teachers")]
public class Teacher
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Department { get; set; } = string.Empty;

    // קשר 1:N - למורה אחד יש מספר קורסים
    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Course> Courses { get; set; } = new();

    [Ignore]
    public string DisplayInfo => $"{FullName} ({Department})";
}
```

### ב. ישות הקורס (`Course.cs`)
מייצגת קורס נלמד. הקורס מקושר למורה האחראי עליו (`TeacherId` כמפתח זר), ומקושר לרשימת התלמידים הרשומים אליו (קשר M:N):
```csharp
[Table("Courses")]
public class Course
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public int Credits { get; set; } = 3;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    // מפתח זר המקשר למורה
    [ForeignKey(typeof(Teacher))]
    public int TeacherId { get; set; }

    // קשר N:1 - לכל קורס יש מורה אחד
    [ManyToOne]
    public Teacher? Teacher { get; set; }

    // קשר M:N - תלמידים הרשומים לקורס
    [ManyToMany(typeof(Enrollment), CascadeOperations = CascadeOperation.All)]
    public List<Student> Students { get; set; } = new();
}
```

### ג. ישות התלמיד (`Student.cs`)
מייצגת תלמיד. כוללת פרטים אישיים, שכבת גיל (`GradeLevel`), ורשימת הקורסים אליהם התלמיד רשום (קשר M:N):
```csharp
[Table("Students")]
public class Student
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [NotNull, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [NotNull, Unique, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    public int GradeLevel { get; set; } = 10;
    public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-16);

    // קשר M:N - רשימת הקורסים שהתלמיד לומד
    [ManyToMany(typeof(Enrollment), CascadeOperations = CascadeOperation.All)]
    public List<Course> Courses { get; set; } = new();

    [Ignore]
    public string FullName => $"{FirstName} {LastName}";
}
```

### ד. ישות הרישום (`Enrollment.cs`) — טבלת הקישור (Associative Table)
טבלה המקשרת בין תלמיד לקורס ומחזיקה נתונים אודות הקשר עצמו:
- `StudentId`: מפתח זר לתלמיד.
- `CourseId`: מפתח זר לקורס.
- `Grade`: הציון שהתלמיד קיבל בקורס (ערך מטיפוס `double?` המאפשר `null` כל עוד לא הוזן ציון).
- `IsPaid`: האם שולם שכר הלימוד.

```csharp
[Table("Enrollments")]
public class Enrollment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ForeignKey(typeof(Student))]
    public int StudentId { get; set; }

    [ForeignKey(typeof(Course))]
    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    public double? Grade { get; set; }
    public bool IsPaid { get; set; } = true;

    [ManyToOne]
    public Student? Student { get; set; }

    [ManyToOne]
    public Course? Course { get; set; }
}
```

---

## 🔍 שאלת הבנה לבחינה בעל-פה (25% שליטה בקוד)
**שאלה:** מדוע השדה `FullName` במחלקת `Student` מסומן ב-`[Ignore]`?  
**תשובה:** מכיוון ש-`FullName` הוא שדה מחושב (`getter`) המשלב בין `FirstName` ל-`LastName`. אין צורך לשמור אותו כעמודה נפרדת במסד הנתונים כדי למנוע כפילות נתונים (Redundancy) ולשמור על עקרונות הנרמול של מסדי נתונים.

עברו לשלב הבא: [02-Database-Connection-And-Basic-CRUD.md](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/Learning/Guides/02-Database-Connection-And-Basic-CRUD.md).
