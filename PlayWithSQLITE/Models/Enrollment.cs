using SQLite;
using SQLiteNetExtensions.Attributes;

namespace PlayWithSQLITE.Models;

[Table("Enrollments")]
public class Enrollment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // מפתח זר המקשר לתלמיד
    [ForeignKey(typeof(Student))]
    public int StudentId { get; set; }

    // מפתח זר המקשר לקורס
    [ForeignKey(typeof(Course))]
    public int CourseId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.Now;

    // ציון בקורס (0-100) - יכול להיות null כל עוד לא הוזן ציון סופי
    public double? Grade { get; set; }

    // האם שולמו דמי הרישום לקורס
    public bool IsPaid { get; set; } = true;

    // ניווט לנתוני הקשר
    [ManyToOne]
    public Student? Student { get; set; }

    [ManyToOne]
    public Course? Course { get; set; }
}
