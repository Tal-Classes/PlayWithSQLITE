namespace PlayWithSQLITE.Models;

// TODO (Student - שלב 1): לאחר התקנת חבילת sqlite-net-pcl, הוסיפו: using SQLite;
// TODO (Student - שלב 1): לאחר התקנת חבילת SQLiteNetExtensions.Async, הוסיפו: using SQLiteNetExtensions.Attributes;
// TODO (Student - שלב 1): קשטו את המחלקה במאפיין [Table("Teachers")]
public class Teacher
{
    // TODO (Student - שלב 1): קשטו את המפתח הראשי ב-[PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // TODO (Student - שלב 1): הגדירו שדה חובה באורך מקסימלי: [NotNull, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    // TODO (Student - שלב 1): הגדירו אורך מקסימלי: [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    // TODO (Student - שלב 1): הגדירו אורך מקסימלי: [MaxLength(50)]
    public string Department { get; set; } = string.Empty;

    // TODO (Student - שלב 3): הגדירו קשר 1:N מול קורסים באמצעות [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Course> Courses { get; set; } = new();

    // TODO (Student - שלב 1): קשטו ב-[Ignore] כדי שהשדה המחושב לא יישמר כעמודה במסד
    public string DisplayInfo => $"{FullName} ({Department})";
}
