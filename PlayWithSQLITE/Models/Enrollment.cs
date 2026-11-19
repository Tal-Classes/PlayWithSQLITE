namespace PlayWithSQLITE.Models;

// 💡 שלב 1: לאחר התקנת החבילות, אילו using directives יש להוסיף לעבודה עם SQLite ומאפייני מפתחות זרים?
// 💡 הגדרת טבלת גישור: מחלקה זו מייצגת את טבלת הרישום המחברת בין תלמידים לקורסים. כיצד נקשר אותה לטבלת "Enrollments"?
public class Enrollment
{
    // 💡 מפתח ראשי: איזה מאפיין מגדיר שדה זה כמזהה ייחודי בעל מספור אוטומטי של רשומת הרישום?
    public int Id { get; set; }

    // 💡 מפתח זר (Foreign Key לתלמיד):
    // שדה זה מצביע על התלמיד שנרשם. איזה מאפיין מגדיר אותו כמפתח זר ואיזה טיפוס יעד מציינים בסוגריים?
    public int StudentId { get; set; }

    // 💡 מפתח זר (Foreign Key לקורס):
    // שדה זה מצביע על הקורס אליו התלמיד נרשם. איזה מאפיין מגדיר אותו כמפתח זר ואיזה טיפוס יעד מציינים בסוגריים?
    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    // ציון בקורס (0-100) - טיפוס nullable שכן הציון ניתן רק בסיום הסמסטר
    public double? Grade { get; set; }

    // סטטוס תשלום דמי רישום לקורס
    public bool IsPaid { get; set; } = true;

    // 💡 שלב 3 - מאפייני ניווט (Navigation Properties):
    // כל רשומת רישום שייכת לתלמיד יחיד ולקורס יחיד (רבים-ליחיד).
    // איזה מאפיין ניווט יקשר את מופע התלמיד ואת מופע הקורס כדי שנוכל לטעון את פרטיהם המלאים יחד עם הרישום?
    public Student? Student { get; set; }
    public Course? Course { get; set; }
}
