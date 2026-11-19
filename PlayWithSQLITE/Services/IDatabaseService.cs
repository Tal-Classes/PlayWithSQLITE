using PlayWithSQLITE.Models;
using PlayWithSQLITE.Models.DTOs;

namespace PlayWithSQLITE.Services;

/// <summary>
/// החוזה (Interface) של שכבת מסד הנתונים.
/// ממשק זה מגדיר אילו פעולות המערכת דורשת מהמסד (CRUD, קשרי גומלין, ושאילתות דוחות)
/// מבלי לחשוף למסכים (ViewModels) כיצד הנתונים נשמרים או כיצד החיבור מאותחל.
///
/// שימו לב: אתחול החיבור ויצירת הטבלאות (EnsureInitializedAsync) מתבצעים באופן פנימי ואוטומטי
/// בתוך DatabaseService — ה-ViewModels והבדיקות לעולם אינם צריכים לקרוא ל-InitializeAsync!
/// </summary>
public interface IDatabaseService
{
    // ==========================================
    // ניהול נתוני הדגמה ואיפוס (שלב 2)
    // ==========================================

    /// <summary>זורע נתוני הדגמה ראשוניים (מורים, קורסים, תלמידים ורישומים) אם המסד ריק.</summary>
    Task SeedSampleDataAsync();

    /// <summary>מוחק את כל הרשומות מכל הטבלאות בסדר המכבד תלויות ומפתחות זרים.</summary>
    Task ClearAllDataAsync();

    // ==========================================
    // CRUD בסיסי - מורים (Teachers) (שלב 2 + שלב 3)
    // ==========================================

    /// <summary>Read: מחזיר את רשימת כל המורים ממוינת לפי שם.</summary>
    Task<List<Teacher>> GetTeachersAsync();

    /// <summary>Read (By Id): מחזיר מורה בודד לפי מזהה; כאשר includeCourses=true טוען גם את קורסיו (1:N).</summary>
    Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false);

    /// <summary>Create / Update (Upsert): מוסיף מורה חדש (אם Id==0) או מעדכן מורה קיים.</summary>
    Task<int> SaveTeacherAsync(Teacher teacher);

    /// <summary>Delete: מוחק מורה מהמסד (ובשלב 3 מטפל גם בקורסים התלויים בו).</summary>
    Task<int> DeleteTeacherAsync(Teacher teacher);

    // ==========================================
    // CRUD וקשרים - קורסים (Courses) (שלב 2 + שלב 3)
    // ==========================================

    /// <summary>Read: מחזיר את כל הקורסים; כאשר includeRelations=true טוען גם את המורה והתלמידים המקושרים.</summary>
    Task<List<Course>> GetCoursesAsync(bool includeRelations = false);

    /// <summary>Read (By Id): מחזיר קורס בודד לפי מזהה (עם או ללא קשריו).</summary>
    Task<Course?> GetCourseByIdAsync(int id, bool includeRelations = true);

    /// <summary>Create / Update (Upsert): שומר קורס חדש או מעדכן קורס קיים.</summary>
    Task<int> SaveCourseAsync(Course course);

    /// <summary>Delete: מוחק קורס מהמסד (ובשלב 3 מנקה את הרישומים אליו).</summary>
    Task<int> DeleteCourseAsync(Course course);

    // ==========================================
    // CRUD ודפדוף (SKIP / TAKE) - תלמידים (Students) (שלב 2 + שלב 4)
    // ==========================================

    /// <summary>Read + Pagination: מחזיר עמוד של תלמידים ממוינים באמצעות Skip ו-Take.</summary>
    Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20);

    /// <summary>Aggregation (COUNT): מחזיר את המספר הכולל של התלמידים לצורך חישוב מספר העמודים.</summary>
    Task<int> GetStudentsCountAsync();

    /// <summary>Read (By Id): מחזיר תלמיד בודד לפי מזהה; כאשר includeCourses=true טוען את קורסיו (M:N).</summary>
    Task<Student?> GetStudentByIdAsync(int id, bool includeCourses = false);

    /// <summary>Create / Update (Upsert): שומר תלמיד חדש או מעדכן תלמיד קיים.</summary>
    Task<int> SaveStudentAsync(Student student);

    /// <summary>Delete: מוחק תלמיד מהמסד (ובשלב 3 מנקה את רישומיו).</summary>
    Task<int> DeleteStudentAsync(Student student);

    // ==========================================
    // קשרי גומלין ורישום (Enrollments) (שלב 3)
    // ==========================================

    /// <summary>Read (M:N): מחזיר את כל הרישומים כולל פרטי התלמיד והקורס בכל רישום.</summary>
    Task<List<Enrollment>> GetEnrollmentsAsync();

    /// <summary>Create / Update (M:N): רושם תלמיד לקורס (או מעדכן ציון אם כבר רשום).</summary>
    Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null);

    /// <summary>Update (M:N): מעדכן ציון של רישום קיים לפי מזהה הרישום.</summary>
    Task<int> UpdateEnrollmentGradeAsync(int enrollmentId, double grade);

    /// <summary>Delete (M:N): מסיר רישום של תלמיד מקורס מסוים.</summary>
    Task<int> RemoveEnrollmentAsync(int studentId, int courseId);

    // ==========================================
    // שאילתות מתקדמות: JOIN, GROUP BY, SUM, COUNT, AVG (שלב 4)
    // ==========================================

    /// <summary>LEFT JOIN + GROUP BY + COUNT/AVG/MAX/MIN: דוח סיכום ציונים לכל קורס.</summary>
    Task<List<CourseGradeSummary>> GetCourseGradeSummariesAsync();

    /// <summary>Multi-Table JOIN (Enrollments + Students + Courses + Teachers): גיליון ציונים לתלמיד.</summary>
    Task<List<StudentTranscriptItem>> GetStudentTranscriptAsync(int studentId);

    /// <summary>GROUP BY + COUNT: התפלגות מספר התלמידים בכל שכבת גיל.</summary>
    Task<List<GradeLevelCount>> GetStudentDistributionByGradeLevelAsync();

    /// <summary>INNER JOIN + SUM + ExecuteScalarAsync: חישוב סך נקודות הזכות של תלמיד.</summary>
    Task<int> GetTotalCreditsForStudentAsync(int studentId);
}
