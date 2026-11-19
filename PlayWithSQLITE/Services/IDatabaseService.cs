using PlayWithSQLITE.Models;
using PlayWithSQLITE.Models.DTOs;

namespace PlayWithSQLITE.Services;

public interface IDatabaseService
{
    // ==========================================
    // אתחול ומסד נתונים
    // ==========================================
    Task InitializeAsync();
    Task SeedSampleDataAsync();
    Task ClearAllDataAsync();

    // ==========================================
    // CRUD בסיסי - מורים (Teachers)
    // ==========================================
    Task<List<Teacher>> GetTeachersAsync();
    Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false);
    Task<int> SaveTeacherAsync(Teacher teacher);
    Task<int> DeleteTeacherAsync(Teacher teacher);

    // ==========================================
    // CRUD וקשרים - קורסים (Courses)
    // ==========================================
    Task<List<Course>> GetCoursesAsync(bool includeRelations = false);
    Task<Course?> GetCourseByIdAsync(int id, bool includeRelations = true);
    Task<int> SaveCourseAsync(Course course);
    Task<int> DeleteCourseAsync(Course course);

    // ==========================================
    // CRUD ודפדוף (SKIP / TAKE) - תלמידים (Students)
    // ==========================================
    Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20);
    Task<int> GetStudentsCountAsync();
    Task<Student?> GetStudentByIdAsync(int id, bool includeCourses = false);
    Task<int> SaveStudentAsync(Student student);
    Task<int> DeleteStudentAsync(Student student);

    // ==========================================
    // קשרי גומלין ורישום (Enrollments)
    // ==========================================
    Task<List<Enrollment>> GetEnrollmentsAsync();
    Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null);
    Task<int> UpdateEnrollmentGradeAsync(int enrollmentId, double grade);
    Task<int> RemoveEnrollmentAsync(int studentId, int courseId);

    // ==========================================
    // שאילתות מתקדמות: JOIN, GROUP BY, SUM, COUNT, AVG
    // ==========================================
    Task<List<CourseGradeSummary>> GetCourseGradeSummariesAsync();
    Task<List<StudentTranscriptItem>> GetStudentTranscriptAsync(int studentId);
    Task<List<GradeLevelCount>> GetStudentDistributionByGradeLevelAsync();
    Task<int> GetTotalCreditsForStudentAsync(int studentId);
}
