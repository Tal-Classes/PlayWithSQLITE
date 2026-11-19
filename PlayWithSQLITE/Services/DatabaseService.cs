using PlayWithSQLITE.Models;
using PlayWithSQLITE.Models.DTOs;

namespace PlayWithSQLITE.Services;

/// <summary>
/// מחלקת שירות מסד הנתונים המממשת את החוזה IDatabaseService — זוהי נקודת הפתיחה שלכם ב-main!
/// 
/// 🚀 כיצד לעבוד?
/// 1. פתחו את קובץ ההדרכה הראשי: Learning/Guides/00-Master-Assignment.md
/// 2. שלב 1: התקינו את החבילות היציבות דרך Manage NuGet Packages, למדו את תרשים ה-ERD
///    וקשטו את 4 המחלקות בתיקיית Models (כולל קשרי גומלין ו-CascadeOperation).
/// 3. שלב 2: השלימו את מחלקת Constants, והגדירו כאן את החיבור האסינכרוני ואת פעולות ה-CRUD הבסיסיות.
/// 4. שלב 3: ממשו את פעולות קשרי הגומלין (1:N ו-M:N) באמצעות SQLiteNetExtensions.Async.
/// 5. שלב 4: ממשו את השאילתות המתקדמות (JOIN, GROUP BY, COUNT, SUM, AVG, SKIP/TAKE).
/// 6. שלב 5: רשמו את השירות ב-DI Container (MauiProgram.cs) וצרו את הפקודות ב-ViewModels!
/// 
/// 💡 ענפי הפתרון לבדיקה עצמית ב-Git:
/// - git checkout step-01-packages-and-models
/// - git checkout step-02-constants-and-basic-crud
/// - git checkout step-03-relationships
/// - git checkout step-04-queries-aggregations-join
/// - git checkout step-05-di-and-viewmodels
/// - git checkout step-06-full-solution
/// </summary>
public class DatabaseService : IDatabaseService
{
    // 💡 שלב 2: לאחר התקנת החבילות, אילו using directives יש להוסיף בראש הקובץ?
    // הגדירו כאן שדה פרטי עבור אובייקט החיבור האסינכרוני של sqlite-net-pcl (למשל _database).

    private readonly string _dbPath;

    public DatabaseService()
    {
        // 💡 שלב 2: לאחר שתשלימו את מחלקת Constants, החליפו את השורה הזו כך שתשתמש ב-Constants.DatabasePath!
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "SchoolLms.db3");
    }

    // בנאי נוסף להזרקת נתיב מותאם — משמש את בדיקות היחידה (xUnit v3) לעבודה מול קובץ זמני מבודד
    public DatabaseService(string dbPath)
    {
        _dbPath = dbPath;
    }

    // ==========================================
    // שלב 2: אתחול פנימי אוטומטי (EnsureInitializedAsync — ראו מדריך 02)
    // ==========================================

    private async Task EnsureInitializedAsync()
    {
        // 💡 אתחול פנימי אוטומטי (שלב 2):
        // אתחול המסד הוא אחריות פנימית של השירות בלבד — ה-ViewModels לעולם לא קוראים למתודה זו ישירות!
        // 1. מניעת אתחול כפול: אם החיבור כבר פתוח (_database != null), חזרו מיד.
        // 2. פתיחת החיבור: אתחלו את החיבור האסינכרוני בעזרת _dbPath ו-Constants.Flags.
        // 3. יצירת הטבלאות: צרו באופן אידמפוטנטי את 4 הטבלאות (Teacher, Course, Student, Enrollment).
        // 4. קראו ל-await EnsureInitializedAsync() בתחילת כל פעולת CRUD / שאילתה בשירות!
        await Task.CompletedTask;
    }

    public async Task ClearAllDataAsync()
    {
        // 💡 ניקוי נתונים (שלב 2):
        // באיזו מתודה מוחקים את כל הרשומות מטבלה? ומדוע חשוב למחוק תחילה את Enrollments ורק בסוף את Teachers?
        await Task.CompletedTask;
    }

    public async Task SeedSampleDataAsync()
    {
        // 💡 זריעת נתוני הדגמה (שלב 2 ושלב 3):
        // 1. כיצד נבדוק שהמסד עדיין ריק כדי לא לשכפל נתונים בכל הפעלה?
        // 2. הוסיפו מורים, קורסים (עם TeacherId), תלמידים ורישומים (Enrollments) עם ציונים.
        await Task.CompletedTask;
    }

    // ==========================================
    // שלב 2: CRUD בסיסי - מורים (ראו מדריך 02)
    // ==========================================

    public async Task<List<Teacher>> GetTeachersAsync()
    {
        // 💡 Read: כיצד שולפים את כל המורים מהטבלה, ממוינים לפי FullName, באופן אסינכרוני?
        return await Task.FromResult(new List<Teacher>());
    }

    public async Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false)
    {
        // 💡 Read By Id:
        // בשלב 2: שליפת מורה יחיד לפי Id.
        // בשלב 3: כאשר includeCourses == true, באיזו מתודת הרחבה נשתמש לטעינת המורה יחד עם קורסיו (1:N)?
        return await Task.FromResult<Teacher?>(null);
    }

    public async Task<int> SaveTeacherAsync(Teacher teacher)
    {
        // 💡 Create / Update (Upsert):
        // כיצד מבדילים בין מורה חדש למורה קיים לפי ה-PrimaryKey שלו, ואיזו מתודה מפעילים בכל מקרה?
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteTeacherAsync(Teacher teacher)
    {
        // 💡 Delete:
        // בשלב 2: מחיקת המורה מהטבלה.
        // בשלב 3: כיצד מונעים רשומות יתומות (Orphan Records) של קורסים שהיו משויכים למורה?
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 2 ושלב 3: CRUD וקשרים - קורסים (ראו מדריכים 02 ו-03)
    // ==========================================

    public async Task<List<Course>> GetCoursesAsync(bool includeRelations = false)
    {
        // 💡 שלב 2: שליפת כל הקורסים ממוינים לפי Title.
        // 💡 שלב 3: כאשר includeRelations == true, טעינת הקורסים יחד עם המורה והתלמידים המקושרים.
        return await Task.FromResult(new List<Course>());
    }

    public async Task<Course?> GetCourseByIdAsync(int id, bool includeRelations = true)
    {
        // 💡 שליפת קורס בודד לפי Id (עם או ללא קשרי גומלין).
        return await Task.FromResult<Course?>(null);
    }

    public async Task<int> SaveCourseAsync(Course course)
    {
        // 💡 שמירת קורס חדש או עדכון קורס קיים (Upsert).
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteCourseAsync(Course course)
    {
        // 💡 מחיקת קורס (ובשלב 3 — ניקוי רשומות ה-Enrollment המקושרות אליו).
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 2 ושלב 4: CRUD ודפדוף - תלמידים (ראו מדריכים 02 ו-04)
    // ==========================================

    public async Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20)
    {
        // 💡 שלב 2: שליפת תלמידים ממוינים לפי שם משפחה ושם פרטי.
        // 💡 שלב 4 (דפדוף / Pagination): הוספת Skip ו-Take לאחר המיון (מדוע המיון חייב לבוא לפני Skip?).
        return await Task.FromResult(new List<Student>());
    }

    public async Task<int> GetStudentsCountAsync()
    {
        // 💡 ספירה (COUNT): באיזו מתודה אסינכרונית סופרים את סך כל התלמידים בטבלה?
        return await Task.FromResult(0);
    }

    public async Task<Student?> GetStudentByIdAsync(int id, bool includeCourses = false)
    {
        // 💡 שליפת תלמיד לפי Id (ועם קורסיו כאשר includeCourses == true).
        return await Task.FromResult<Student?>(null);
    }

    public async Task<int> SaveStudentAsync(Student student)
    {
        // 💡 שמירת תלמיד חדש או עדכון תלמיד קיים (Upsert).
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteStudentAsync(Student student)
    {
        // 💡 מחיקת תלמיד (ובשלב 3 — מחיקת רישומיו כדי למנוע רשומות יתומות).
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 3: קשרי גומלין ורישומים (Enrollments - ראו מדריך 03)
    // ==========================================

    public async Task<List<Enrollment>> GetEnrollmentsAsync()
    {
        // 💡 שליפת כל הרישומים כולל ישויות הניווט (Student ו-Course).
        return await Task.FromResult(new List<Enrollment>());
    }

    public async Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null)
    {
        // 💡 רישום תלמיד לקורס (טבלת גישור M:N):
        // כיצד בודקים קודם אם התלמיד כבר רשום לאותו קורס (כדי לעדכן ציון במקום ליצור כפילות)?
        return await Task.FromResult(0);
    }

    public async Task<int> UpdateEnrollmentGradeAsync(int enrollmentId, double grade)
    {
        // 💡 עדכון ציון של רישום קיים לפי enrollmentId.
        return await Task.FromResult(0);
    }

    public async Task<int> RemoveEnrollmentAsync(int studentId, int courseId)
    {
        // 💡 הסרת רישום של תלמיד מקורס מסוים.
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 4: שאילתות מורכבות: JOIN, GROUP BY, SUM, COUNT, AVG (ראו מדריך 04)
    // ==========================================

    public async Task<List<CourseGradeSummary>> GetCourseGradeSummariesAsync()
    {
        // 💡 פירוק שאילתת דוח קורסים (ראו מדריך 04):
        // 1. איזה JOIN מבטיח שגם קורס ללא תלמידים יופיע בדוח?
        // 2. מדוע סופרים COUNT(e.Id) ולא COUNT(*)?
        // 3. לפי אילו עמודות מקבצים ב-GROUP BY, ואיך ממפים את התוצאה ל-DTO בעזרת QueryAsync?
        return await Task.FromResult(new List<CourseGradeSummary>());
    }

    public async Task<List<StudentTranscriptItem>> GetStudentTranscriptAsync(int studentId)
    {
        // 💡 גיליון ציונים אישי (חיבור 4 טבלאות):
        // חברו בין Enrollments, Students, Courses ו-Teachers (שימו לב מתי INNER JOIN ומתי LEFT JOIN),
        // וסננו לפי studentId באמצעות פרמטר בטוח (?) נגד SQL Injection.
        return await Task.FromResult(new List<StudentTranscriptItem>());
    }

    public async Task<List<GradeLevelCount>> GetStudentDistributionByGradeLevelAsync()
    {
        // 💡 התפלגות שכבות (GROUP BY + COUNT):
        // קבצו את טבלת Students לפי GradeLevel וספרו כמה תלמידים יש בכל שכבה.
        return await Task.FromResult(new List<GradeLevelCount>());
    }

    public async Task<int> GetTotalCreditsForStudentAsync(int studentId)
    {
        // 💡 חישוב סכום סקלרי (INNER JOIN + SUM + ExecuteScalarAsync):
        // חברו בין Enrollments ל-Courses, סננו לפי studentId, וחשבו את סכום ה-Credits (עם IFNULL למקרה של 0 קורסים).
        return await Task.FromResult(0);
    }
}
