using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PlayWithSQLITE.Models;

[Table("Teachers")]
public class Teacher
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Department { get; set; } = string.Empty;

    // קשר 1:N - למורה אחד יש רשימת קורסים אותם הוא מלמד
    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Course> Courses { get; set; } = new();

    [Ignore]
    public string DisplayInfo => $"{FullName} ({Department})";
}
