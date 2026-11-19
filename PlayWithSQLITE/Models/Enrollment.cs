namespace PlayWithSQLITE.Models;

// 💡 שלב 1 (ראו מדריך 01 ותרשים ERD):
// - אילו מרחבי שמות (using directives) יש לייבא?
// - מחלקה זו היא טבלת הגישור (Junction Table) השוברת את קשר ה-M:N בין Students ל-Courses.
//   כיצד ממפים אותה לטבלה בשם "Enrollments"?
public class Enrollment
{
    // 💡 מפתח ראשי ומספור אוטומטי של רשומת הרישום:
    public int Id { get; set; }

    // 💡 מפתח זר ראשון (Foreign Key):
    // מצביע לטבלת התלמידים. איזה מאפיין מסמן אותו כמפתח זר ולאיזה טיפוס ישות?
    public int StudentId { get; set; }

    // 💡 מפתח זר שני (Foreign Key):
    // מצביע לטבלת הקורסים. איזה מאפיין מסמן אותו כמפתח זר ולאיזה טיפוס ישות?
    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    // ציון בקורס (0-100) — nullable כל עוד טרם הוזן ציון סופי
    public double? Grade { get; set; }

    // האם שולמו דמי הרישום לקורס
    public bool IsPaid { get; set; } = true;

    // 💡 מאפייני ניווט בצד ה-N של שני הקשרים (רבים-ליחיד):
    // כל רשומת רישום שייכת לתלמיד אחד ולקורס אחד.
    // איזה מאפיין של SQLiteNetExtensions מגדירים מעל כל אחד משני המאפיינים הבאים?
    public Student? Student { get; set; }
    public Course? Course { get; set; }
}
