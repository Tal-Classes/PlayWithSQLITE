namespace PlayWithSQLITE.Models;

// TODO (Student - שלב 1): לאחר התקנת החבילות, הוסיפו using SQLite; ו-using SQLiteNetExtensions.Attributes;
// TODO (Student - שלב 1): קשטו את המחלקה במאפיין [Table("Enrollments")]
public class Enrollment
{
    // TODO (Student - שלב 1): קשטו את המפתח הראשי ב-[PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // TODO (Student - שלב 1): קשטו כמפתח זר לתלמיד: [ForeignKey(typeof(Student))]
    public int StudentId { get; set; }

    // TODO (Student - שלב 1): קשטו כמפתח זר לקורס: [ForeignKey(typeof(Course))]
    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    // ציון בקורס (0-100) - nullable כל עוד לא הוזן ציון סופי
    public double? Grade { get; set; }

    // האם שולמו דמי רישום
    public bool IsPaid { get; set; } = true;

    // TODO (Student - שלב 3): מאפייני ניווט [ManyToOne] לתלמיד ולקורס
    public Student? Student { get; set; }
    public Course? Course { get; set; }
}
