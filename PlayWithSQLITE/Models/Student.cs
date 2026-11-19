using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PlayWithSQLITE.Models;

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

    // שכבת גיל: 10 = י', 11 = י"א, 12 = י"ב
    public int GradeLevel { get; set; } = 10;

    public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-16);

    // קשר M:N - תלמיד רשום למספר קורסים דרך טבלת הקשר Enrollment
    [ManyToMany(typeof(Enrollment), CascadeOperations = CascadeOperation.All)]
    public List<Course> Courses { get; set; } = new();

    [Ignore]
    public string FullName => $"{FirstName} {LastName}";

    [Ignore]
    public string GradeLevelHebrew => GradeLevel switch
    {
        10 => "שכבה י'",
        11 => "שכבה י\"א",
        12 => "שכבה י\"ב",
        _ => $"שכבה {GradeLevel}"
    };
}
