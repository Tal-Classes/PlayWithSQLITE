namespace PlayWithSQLITE.Models;

// TODO (Student - שלב 1): לאחר התקנת החבילות, הוסיפו using SQLite; ו-using SQLiteNetExtensions.Attributes;
// TODO (Student - שלב 1): קשטו את המחלקה במאפיין [Table("Students")]
public class Student
{
    // TODO (Student - שלב 1): קשטו את המפתח הראשי ב-[PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // TODO (Student - שלב 1): שדה חובה באורך מקסימלי: [NotNull, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    // TODO (Student - שלב 1): שדה חובה באורך מקסימלי: [NotNull, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    // TODO (Student - שלב 1): שדה חובה וחד-ערכי: [NotNull, Unique, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    public int GradeLevel { get; set; } = 10;
    public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-16);

    // TODO (Student - שלב 3): הגדירו קשר M:N בעזרת [ManyToMany(typeof(Enrollment), CascadeOperations = CascadeOperation.All)]
    public List<Course> Courses { get; set; } = new();

    // TODO (Student - שלב 1): קשטו ב-[Ignore] כדי שהשדות המחושבים לא יישמרו במסד
    public string FullName => $"{FirstName} {LastName}";

    public string GradeLevelHebrew => GradeLevel switch
    {
        10 => "שכבה י'",
        11 => "שכבה י\"א",
        12 => "שכבה י\"ב",
        _ => $"שכבה {GradeLevel}"
    };
}
