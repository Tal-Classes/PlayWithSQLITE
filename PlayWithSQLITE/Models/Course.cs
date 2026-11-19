using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PlayWithSQLITE.Models;

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

    // מפתח זר המקשר למורה (Teacher)
    [ForeignKey(typeof(Teacher))]
    public int TeacherId { get; set; }

    // קשר N:1 - לכל קורס יש מורה אחד
    [ManyToOne]
    public Teacher? Teacher { get; set; }

    // קשר M:N - קורס כולל רשימת תלמידים דרך טבלת הקשר Enrollment
    [ManyToMany(typeof(Enrollment), CascadeOperations = CascadeOperation.All)]
    public List<Student> Students { get; set; } = new();

    [Ignore]
    public string TeacherName => Teacher?.FullName ?? $"מורה מזהה #{TeacherId}";
}
