using PlayWithSQLITE.Models;
using PlayWithSQLITE.Models.DTOs;
using SQLite;

namespace PlayWithSQLITE.Services;

/// <summary>
/// מחלקת שירות מסד הנתונים - זוהי סביבת העבודה הראשית שלכם ב-main!
/// פעלו לפי קובץ ההדרכה הראשי Learning/Guides/00-Master-Assignment.md
/// ומלאו את המתודות שלב אחר שלב.
/// אם תרצו לבדוק את עצמכם, תוכלו להריץ בדיקות יחידה: `dotnet test`
/// או לעבור ל-Branch של השלב המתאים (לדוגמה: `git checkout step-02-db-connection-and-basic-crud`).
/// </summary>
public class DatabaseService : IDatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        // נתיב הקובץ במכשיר
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "SchoolLms.db3");
    }

    public DatabaseService(string dbPath)
    {
        _dbPath = dbPath;
    }

    // ==========================================
    // שלב 2: אתחול וחיבור אסינכרוני (ראו מדריך 02)
    // ==========================================

    public async Task InitializeAsync()
    {
        if (_database != null)
            return;

        // TODO (Student): 1. צרו מופע חדש של SQLiteAsyncConnection עם _dbPath
        // TODO (Student): 2. קראו ל-CreateTableAsync עבור 4 הטבלאות: Teacher, Course, Student, Enrollment
        await Task.CompletedTask;
    }

    private async Task EnsureInitializedAsync()
    {
        if (_database == null)
        {
            await InitializeAsync();
        }
    }

    public async Task ClearAllDataAsync()
    {
        await EnsureInitializedAsync();
        // TODO (Student): מחיקת כל הנתונים מהטבלאות בעזרת DeleteAllAsync
        await Task.CompletedTask;
    }

    public async Task SeedSampleDataAsync()
    {
        await EnsureInitializedAsync();
        // TODO (Student): יצירת נתוני הדגמה למורים, קורסים, תלמידים ורישומים
        await Task.CompletedTask;
    }

    // ==========================================
    // שלב 2: פעולות CRUD - מורים (ראו מדריך 02)
    // ==========================================

    public async Task<List<Teacher>> GetTeachersAsync()
    {
        await EnsureInitializedAsync();
        // TODO (Student): שליפת כל המורים ממוינים לפי FullName בעזרת Table<Teacher>().ToListAsync()
        return new List<Teacher>();
    }

    public async Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false)
    {
        await EnsureInitializedAsync();
        // TODO (Student): שליפת מורה לפי Id. אם includeCourses == true טענו גם קורסים (ראו מדריך 03)
        return null;
    }

    public async Task<int> SaveTeacherAsync(Teacher teacher)
    {
        await EnsureInitializedAsync();
        // TODO (Student): אם teacher.Id != 0 בצעו UpdateAsync, אחרת בצעו InsertAsync
        return 0;
    }

    public async Task<int> DeleteTeacherAsync(Teacher teacher)
    {
        await EnsureInitializedAsync();
        // TODO (Student): מחיקת המורה בעזרת DeleteAsync
        return 0;
    }

    // ==========================================
    // שלב 2 ושלב 3: פעולות CRUD וקשרים - קורסים (ראו מדריך 02 ומדריך 03)
    // ==========================================

    public async Task<List<Course>> GetCoursesAsync(bool includeRelations = false)
    {
        await EnsureInitializedAsync();
        // TODO (Student): שליפת קורסים. אם includeRelations == true השתמשו ב-GetAllWithChildrenAsync
        return new List<Course>();
    }

    public async Task<Course?> GetCourseByIdAsync(int id, bool includeRelations = true)
    {
        await EnsureInitializedAsync();
        // TODO (Student): שליפת קורס לפי Id
        return null;
    }

    public async Task<int> SaveCourseAsync(Course course)
    {
        await EnsureInitializedAsync();
        // TODO (Student): שמירה או עדכון של קורס
        return 0;
    }

    public async Task<int> DeleteCourseAsync(Course course)
    {
        await EnsureInitializedAsync();
        // TODO (Student): מחיקת קורס
        return 0;
    }

    // ==========================================
    // שלב 2 ושלב 4: פעולות CRUD ודפדוף - תלמידים (ראו מדריך 02 ומדריך 04)
    // ==========================================

    public async Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20)
    {
        await EnsureInitializedAsync();
        // TODO (Student): שליפת תלמידים תוך שימוש ב-OrderBy, Skip(skip) ו-Take(take) לדפדוף מהיר
        return new List<Student>();
    }

    public async Task<int> GetStudentsCountAsync()
    {
        await EnsureInitializedAsync();
        // TODO (Student): ספירת כל התלמידים בטבלה בעזרת CountAsync()
        return 0;
    }

    public async Task<Student?> GetStudentByIdAsync(int id, bool includeCourses = false)
    {
        await EnsureInitializedAsync();
        // TODO (Student): שליפת תלמיד בודד לפי מזהה
        return null;
    }

    public async Task<int> SaveStudentAsync(Student student)
    {
        await EnsureInitializedAsync();
        // TODO (Student): שמירת תלמיד חדש או עדכון קיים
        return 0;
    }

    public async Task<int> DeleteStudentAsync(Student student)
    {
        await EnsureInitializedAsync();
        // TODO (Student): מחיקת תלמיד
        return 0;
    }

    // ==========================================
    // שלב 3: קשרי גומלין ורישומים (Enrollments - ראו מדריך 03)
    // ==========================================

    public async Task<List<Enrollment>> GetEnrollmentsAsync()
    {
        await EnsureInitializedAsync();
        // TODO (Student): שליפת כל הרישומים כולל ילדים בעזרת GetAllWithChildrenAsync
        return new List<Enrollment>();
    }

    public async Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null)
    {
        await EnsureInitializedAsync();
        // TODO (Student): רישום תלמיד לקורס (יצירת רשומת Enrollment חדשה או עדכון ציון)
        return 0;
    }

    public async Task<int> UpdateEnrollmentGradeAsync(int enrollmentId, double grade)
    {
        await EnsureInitializedAsync();
        // TODO (Student): עדכון שדה הציון (Grade) של רישום קיים
        return 0;
    }

    public async Task<int> RemoveEnrollmentAsync(int studentId, int courseId)
    {
        await EnsureInitializedAsync();
        // TODO (Student): מחיקת רישום של תלמיד מקורס
        return 0;
    }

    // ==========================================
    // שלב 4: שאילתות מורכבות: JOIN, GROUP BY, SUM, COUNT, AVG (ראו מדריך 04)
    // ==========================================

    public async Task<List<CourseGradeSummary>> GetCourseGradeSummariesAsync()
    {
        await EnsureInitializedAsync();
        // TODO (Student): כתיבת שאילתת LEFT JOIN המקבצת (GROUP BY) לפי קורס ומחשבת AVG, MAX, MIN, COUNT
        return new List<CourseGradeSummary>();
    }

    public async Task<List<StudentTranscriptItem>> GetStudentTranscriptAsync(int studentId)
    {
        await EnsureInitializedAsync();
        // TODO (Student): כתיבת שאילתת JOIN המחברת 3 טבלאות לחילוץ גיליון ציונים
        return new List<StudentTranscriptItem>();
    }

    public async Task<List<GradeLevelCount>> GetStudentDistributionByGradeLevelAsync()
    {
        await EnsureInitializedAsync();
        // TODO (Student): שאילתת GROUP BY עם COUNT לחישוב התפלגות תלמידים לפי שכבה
        return new List<GradeLevelCount>();
    }

    public async Task<int> GetTotalCreditsForStudentAsync(int studentId)
    {
        await EnsureInitializedAsync();
        // TODO (Student): שאילתת SUM לחישוב סך כל נקודות הזכות של תלמיד
        return 0;
    }
}
