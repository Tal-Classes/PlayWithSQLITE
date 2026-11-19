using PlayWithSQLITE.Models;
using PlayWithSQLITE.Models.DTOs;

namespace PlayWithSQLITE.Services;

/// <summary>
/// מחלקת שירות מסד הנתונים - זוהי נקודת הפתיחה שלכם ב-main!
/// 
/// 🚀 כיצד לעבוד?
/// 1. פתחו את קובץ ההדרכה הראשי: Learning/Guides/00-Master-Assignment.md
/// 2. בצעו את שלב 1: התקינו את החבילות sqlite-net-pcl ו-SQLiteNetExtensions.Async
///    והוסיפו את הקישוטים (Attributes) במחלקות בתיקיית Models.
/// 3. בצעו את שלב 2: הוסיפו כאן using SQLite; ואתחלו את הקשר למסד הנתונים בעזרת SQLiteAsyncConnection.
/// 4. המשיכו שלב אחר שלב לפי קובצי ההדרכה ב-Learning/Guides/.
/// 
/// 💡 בדיקה עצמית:
/// בכל שלב תוכלו לבדוק את הפתרון שלכם מול הענף המתאים ב-Git:
/// - git checkout step-01-packages-and-models
/// - git checkout step-02-db-connection-and-basic-crud
/// - git checkout step-03-relationships
/// - git checkout step-04-queries-aggregations-join
/// - git checkout step-05-full-solution
/// </summary>
public class DatabaseService : IDatabaseService
{
    // TODO (Student - שלב 2): לאחר התקנת sqlite-net-pcl, הוסיפו בראש הקובץ: using SQLite;
    // והגדירו כאן שדה מסוג חיבור אסינכרוני למסד:
    // private SQLiteAsyncConnection? _database;

    private readonly string _dbPath;

    public DatabaseService()
    {
        // קביעת נתיב הקובץ בתיקיית האפליקציה המבודדת במכשיר
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
        // TODO (Student - שלב 2):
        // 1. בדקו אם _database כבר קיים (אם כן, חזרו מיד).
        // 2. אתחלו את _database באמצעות:
        //    _database = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        // 3. צרו את 4 הטבלאות באמצעות:
        //    await _database.CreateTableAsync<Teacher>();
        //    await _database.CreateTableAsync<Course>();
        //    await _database.CreateTableAsync<Student>();
        //    await _database.CreateTableAsync<Enrollment>();
        await Task.CompletedTask;
    }

    public async Task ClearAllDataAsync()
    {
        // TODO (Student - שלב 2): מחיקת כל הנתונים בעזרת DeleteAllAsync על כל אחת מהטבלאות
        await Task.CompletedTask;
    }

    public async Task SeedSampleDataAsync()
    {
        // TODO (Student - שלב 2/3): יצירת נתוני הדגמה למורים, קורסים, תלמידים ורישומים
        await Task.CompletedTask;
    }

    // ==========================================
    // שלב 2: CRUD בסיסי - מורים (ראו מדריך 02)
    // ==========================================

    public async Task<List<Teacher>> GetTeachersAsync()
    {
        // TODO (Student - שלב 2): החזירו את כל המורים מהמסד ממוינים לפי שם:
        // return await _database.Table<Teacher>().OrderBy(t => t.FullName).ToListAsync();
        return await Task.FromResult(new List<Teacher>());
    }

    public async Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false)
    {
        // TODO (Student - שלב 2): שליפת מורה לפי מזהה Id
        // בשלב 3, אם includeCourses הוא true:
        // return await _database.GetWithChildrenAsync<Teacher>(id);
        return await Task.FromResult<Teacher?>(null);
    }

    public async Task<int> SaveTeacherAsync(Teacher teacher)
    {
        // TODO (Student - שלב 2):
        // אם teacher.Id != 0 בצעו UpdateAsync(teacher)
        // אחרת בצעו InsertAsync(teacher)
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteTeacherAsync(Teacher teacher)
    {
        // TODO (Student - שלב 2): מחיקת המורה בעזרת DeleteAsync(teacher)
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 2 ושלב 3: CRUD וקשרים - קורסים (ראו מדריך 02 ומדריך 03)
    // ==========================================

    public async Task<List<Course>> GetCoursesAsync(bool includeRelations = false)
    {
        // TODO (Student - שלב 2): שליפת קורסים בעזרת Table<Course>().ToListAsync()
        // בשלב 3 (קשרים): אם includeRelations הוא true השתמשו ב-GetAllWithChildrenAsync<Course>()
        return await Task.FromResult(new List<Course>());
    }

    public async Task<Course?> GetCourseByIdAsync(int id, bool includeRelations = true)
    {
        // TODO (Student - שלב 2): שליפת קורס בודד לפי מזהה Id
        return await Task.FromResult<Course?>(null);
    }

    public async Task<int> SaveCourseAsync(Course course)
    {
        // TODO (Student - שלב 2): הוספה או עדכון קורס
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteCourseAsync(Course course)
    {
        // TODO (Student - שלב 2): מחיקת קורס
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 2 ושלב 4: CRUD ודפדוף - תלמידים (ראו מדריך 02 ומדריך 04)
    // ==========================================

    public async Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20)
    {
        // TODO (Student - שלב 2): שליפת תלמידים בסיסית.
        // TODO (Student - שלב 4): דפדוף (Pagination) באמצעות OrderBy, Skip(skip) ו-Take(take)
        return await Task.FromResult(new List<Student>());
    }

    public async Task<int> GetStudentsCountAsync()
    {
        // TODO (Student - שלב 4): ספירת סך כל התלמידים במסד בעזרת CountAsync()
        return await Task.FromResult(0);
    }

    public async Task<Student?> GetStudentByIdAsync(int id, bool includeCourses = false)
    {
        // TODO (Student - שלב 2): שליפת תלמיד לפי מזהה
        return await Task.FromResult<Student?>(null);
    }

    public async Task<int> SaveStudentAsync(Student student)
    {
        // TODO (Student - שלב 2): שמירת תלמיד חדש או עדכון קיים
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteStudentAsync(Student student)
    {
        // TODO (Student - שלב 2): מחיקת תלמיד מהמסד
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 3: קשרי גומלין ורישומים (Enrollments - ראו מדריך 03)
    // ==========================================

    public async Task<List<Enrollment>> GetEnrollmentsAsync()
    {
        // TODO (Student - שלב 3): שליפת רישומים בעזרת GetAllWithChildrenAsync<Enrollment>()
        return await Task.FromResult(new List<Enrollment>());
    }

    public async Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null)
    {
        // TODO (Student - שלב 3): רישום תלמיד לקורס ע"י הוספת רשומת Enrollment חדשה
        return await Task.FromResult(0);
    }

    public async Task<int> UpdateEnrollmentGradeAsync(int enrollmentId, double grade)
    {
        // TODO (Student - שלב 3): עדכון ציון של תלמיד בקורס קיים
        return await Task.FromResult(0);
    }

    public async Task<int> RemoveEnrollmentAsync(int studentId, int courseId)
    {
        // TODO (Student - שלב 3): הסרת רישום של תלמיד מקורס
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 4: שאילתות מורכבות: JOIN, GROUP BY, SUM, COUNT, AVG (ראו מדריך 04)
    // ==========================================

    public async Task<List<CourseGradeSummary>> GetCourseGradeSummariesAsync()
    {
        // TODO (Student - שלב 4): כתיבת שאילתת LEFT JOIN המקבצת (GROUP BY) לפי קורס ומחשבת AVG, MAX, MIN, COUNT
        return await Task.FromResult(new List<CourseGradeSummary>());
    }

    public async Task<List<StudentTranscriptItem>> GetStudentTranscriptAsync(int studentId)
    {
        // TODO (Student - שלב 4): כתיבת שאילתת JOIN המחברת 3 טבלאות לחילוץ גיליון ציונים אישי
        return await Task.FromResult(new List<StudentTranscriptItem>());
    }

    public async Task<List<GradeLevelCount>> GetStudentDistributionByGradeLevelAsync()
    {
        // TODO (Student - שלב 4): שאילתת GROUP BY עם COUNT לחישוב התפלגות תלמידים לפי שכבה
        return await Task.FromResult(new List<GradeLevelCount>());
    }

    public async Task<int> GetTotalCreditsForStudentAsync(int studentId)
    {
        // TODO (Student - שלב 4): שאילתת SUM לחישוב סך כל נקודות הזכות של תלמיד
        return await Task.FromResult(0);
    }
}
