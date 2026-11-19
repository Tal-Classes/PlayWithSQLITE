namespace PlayWithSQLITE.Models.DTOs;

public class GradeLevelCount
{
    public int GradeLevel { get; set; }
    public int StudentCount { get; set; }

    public string Title => GradeLevel switch
    {
        10 => "שכבה י'",
        11 => "שכבה י\"א",
        12 => "שכבה י\"ב",
        _ => $"שכבה {GradeLevel}"
    };
}
