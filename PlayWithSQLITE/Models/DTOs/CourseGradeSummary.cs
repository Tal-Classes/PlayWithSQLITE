namespace PlayWithSQLITE.Models.DTOs;

public class CourseGradeSummary
{
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public double AverageGrade { get; set; }
    public double MaxGrade { get; set; }
    public double MinGrade { get; set; }

    public string FormattedSummary => $"{CourseTitle} | מורה: {TeacherName} | {StudentCount} תלמידים | ממוצע: {AverageGrade:F1}";
}
