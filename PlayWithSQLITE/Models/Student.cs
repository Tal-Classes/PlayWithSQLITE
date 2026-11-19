namespace PlayWithSQLITE.Models;

// 💡 שלב 1 (ראו מדריך 01 ותרשים ERD):
// - אילו מרחבי שמות (using directives) יש לייבא לאחר התקנת החבילות?
// - כיצד ממפים מחלקה זו לטבלה בשם "Students"?
public class Student
{
    // 💡 מפתח ראשי ומספור אוטומטי: אילו מאפיינים נדרשים?
    public int Id { get; set; }

    // 💡 שדות חובה באורך מקסימלי של 50 תווים כל אחד:
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    // 💡 אילוץ ייחודיות + שדה חובה + הגבלת אורך (100):
    // איזה מאפיין מונע יצירת שני תלמידים בעלי אותה כתובת דוא"ל בטבלה?
    public string Email { get; set; } = string.Empty;

    public int GradeLevel { get; set; } = 10;
    public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-16);

    // 💡 קשר M:N (רבים-לרבים) מול קורסים:
    // איזה מאפיין מקשר בין התלמיד לרשימת הקורסים שלו דרך טבלת הגישור, ואיזה CascadeOperation מתאים כאן?
    public List<Course> Courses { get; set; } = new();

    // 💡 מאפיינים מחושבים לתצוגה בממשק (XAML) בלבד:
    // איזה מאפיין מורה ל-SQLite להתעלם מכל אחד מהם? (שימו לב: יש שני מאפיינים מחושבים!)
    public string FullName => $"{FirstName} {LastName}";

    public string GradeLevelHebrew => GradeLevel switch
    {
        10 => "שכבה י'",
        11 => "שכבה י\"א",
        12 => "שכבה י\"ב",
        _ => $"שכבה {GradeLevel}"
    };
}
