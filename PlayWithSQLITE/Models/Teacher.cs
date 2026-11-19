namespace PlayWithSQLITE.Models;

// 💡 שלב 1 (ראו מדריך 01 ותרשים ERD):
// - אילו שני מרחבי שמות (using directives) יש לייבא לאחר התקנת החבילות היציבות?
// - כיצד נסמן ל-SQLite שמחלקה זו מייצגת טבלה ייעודית בשם "Teachers"?
public class Teacher
{
    // 💡 מפתח ראשי: איזה מאפיין מזהה רשומה באופן חד-ערכי, וכיצד נורה למסד למספר אותו אוטומטית?
    public int Id { get; set; }

    // 💡 שלמות נתונים: שם המורה הוא שדה חובה (אינו יכול להיות null) ומוגבל ל-100 תווים. אילו מאפיינים נדרשים?
    public string FullName { get; set; } = string.Empty;

    // 💡 הגבלת אורך: כיצד מגבילים את אורך כתובת הדוא"ל לעד 100 תווים?
    public string Email { get; set; } = string.Empty;

    // 💡 הגבלת אורך: כיצד מגבילים את שם המחלקה/המגמה לעד 50 תווים?
    public string Department { get; set; } = string.Empty;

    // 💡 קשר 1:N — באיזה צד של הקשר נמצאת טבלת Teachers (צד ה-1 או צד ה-N)?
    // איזה מאפיין של SQLiteNetExtensions מגדירים בצד ההורה על רשימת הקורסים?
    // חשוב: איזה ערך של CascadeOperation מתאים כאן כדי שטעינה/שמירה/מחיקה של מורה תטפל בקורסיו?
    public List<Course> Courses { get; set; } = new();

    // 💡 שדה מחושב לתצוגה בלבד (UI):
    // כיצד מורים ל-SQLite להתעלם ממאפיין זה ולא לייצר עבורו עמודה בטבלה?
    public string DisplayInfo => $"{FullName} ({Department})";
}
