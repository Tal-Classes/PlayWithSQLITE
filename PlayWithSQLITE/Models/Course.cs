namespace PlayWithSQLITE.Models;

// 💡 שלב 1 (ראו מדריך 01 ותרשים ERD):
// - אילו מרחבי שמות (using directives) דרושים כאן?
// - כיצד מגדירים שמחלקה זו תמופה לטבלה בשם "Courses"?
public class Course
{
    // 💡 מפתח ראשי עם מספור אוטומטי: אילו שני מאפיינים נדרשים?
    public int Id { get; set; }

    // 💡 שדה חובה באורך מקסימלי של 100 תווים: אילו מאפיינים מגנים על תקינות הכותרת?
    public string Title { get; set; } = string.Empty;

    public int Credits { get; set; } = 3;

    // 💡 הגבלת אורך: כיצד מגבילים את התיאור לעד 200 תווים?
    public string Description { get; set; } = string.Empty;

    // 💡 קשר 1:N — טבלת Courses היא צד ה-N בקשר מול המורה!
    // איזה מאפיין מסמן עמודה זו כמפתח זר (Foreign Key), ולאיזה טיפוס ישות עליו להצביע?
    public int TeacherId { get; set; }

    // 💡 מאפיין ניווט בצד ה-N (רבים-ליחיד):
    // איזה מאפיין של SQLiteNetExtensions מאפשר לטעון את אובייקט המורה המלא כאשר שולפים קורס?
    public Teacher? Teacher { get; set; }

    // 💡 קשר M:N (רבים-לרבים) מול תלמידים:
    // איזה מאפיין מגדיר קשר רבים-לרבים, איזו טבלת גישור (Junction Table) מציינים בו,
    // ואיזה CascadeOperation תבחרו כאן (שימו לב: מה יקרה אם נמחק קורס עם CascadeDelete)?
    public List<Student> Students { get; set; } = new();

    // 💡 מאפיין מחושב לתצוגה ב-XAML — זכרו לסמן אותו כך שלא יישמר כעמודה בטבלה!
    public string TeacherName => Teacher?.FullName ?? $"מורה מזהה #{TeacherId}";
}
