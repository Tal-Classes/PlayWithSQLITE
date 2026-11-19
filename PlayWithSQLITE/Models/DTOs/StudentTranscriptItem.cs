namespace PlayWithSQLITE.Models.DTOs;

public class StudentTranscriptItem
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public double? Grade { get; set; }
    public bool IsPaid { get; set; }

    public string GradeDisplay => Grade.HasValue ? $"{Grade.Value:F0}" : "טרם הוזן";
}
