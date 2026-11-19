namespace PlayWithSQLITE.Models;

// 💡 שלב 1: לאחר התקנת החבילות, אילו using directives יש להוסיף?
// 💡 הגדרת טבלה: כיצד מקשרים מחלקה זו לטבלת "Students" במסד?
public class Student
{
    // 💡 מפתח ראשי: איזה מאפיין מגדיר שדה זה כמפתח ראשי עם מספור אוטומטי?
    public int Id { get; set; }

    // 💡 שלמות נתונים: שמות התלמיד הם שדות חובה באורך של עד 50 תווים. אילו מאפיינים נדרשים?
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    // 💡 ייחודיות ושלמות נתונים: כתובת הדוא"ל של התלמיד היא שדה חובה, בעל אורך מקסימלי של 100 תווים, ואסור שתחזור על עצמה (ייחודית).
    // איזה מאפיין מבטיח שהערך יהיה ייחודי בכל הטבלה (Unique Constraint)?
    public string Email { get; set; } = string.Empty;

    public int GradeLevel { get; set; } = 10;
    public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-16);

    // 💡 שלב 3 - קשר רבים-לרבים (M:N):
    // תלמיד אחד יכול להירשם לקורסים רבים.
    // איזה מאפיין מקשר בין התלמיד לרשימת הקורסים דרך טבלת הרישום המקשרת?
    public List<Course> Courses { get; set; } = new();

    // 💡 שדות תצוגה מחושבים: מאפיינים אלו נועדו לתצוגה ב-UI (XAML) בלבד.
    // באיזה מאפיין יש לסמן אותם כדי ש-SQLite לא ינסה לייצר עבורם עמודות בטבלה?
    public string FullName => $"{FirstName} {LastName}";

    public string GradeLevelHebrew => GradeLevel switch
    {
        10 => "שכבה י'",
        11 => "שכבה י\"א",
        12 => "שכבה י\"ב",
        _ => $"שכבה {GradeLevel}"
    };
}
