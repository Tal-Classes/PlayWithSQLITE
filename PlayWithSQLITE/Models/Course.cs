namespace PlayWithSQLITE.Models;

// TODO (Student - שלב 1): לאחר התקנת החבילות, הוסיפו using SQLite; ו-using SQLiteNetExtensions.Attributes;
// TODO (Student - שלב 1): קשטו את המחלקה במאפיין [Table("Courses")]
public class Course
{
    // TODO (Student - שלב 1): קשטו את המפתח הראשי ב-[PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // TODO (Student - שלב 1): הגדירו שדה חובה באורך מקסימלי: [NotNull, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    public int Credits { get; set; } = 3;

    // TODO (Student - שלב 1): הגדירו אורך מקסימלי: [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    // TODO (Student - שלב 1): קשטו כמפתח זר למורה: [ForeignKey(typeof(Teacher))]
    public int TeacherId { get; set; }

    // TODO (Student - שלב 3): הגדירו קשר N:1 בעזרת [ManyToOne]
    public Teacher? Teacher { get; set; }

    // TODO (Student - שלב 3): הגדירו קשר M:N בעזרת [ManyToMany(typeof(Enrollment), CascadeOperations = CascadeOperation.All)]
    public List<Student> Students { get; set; } = new();

    // שדה נוחות לתצוגה ב-XAML
    public string TeacherName => Teacher?.FullName ?? $"מורה מזהה #{TeacherId}";
}
