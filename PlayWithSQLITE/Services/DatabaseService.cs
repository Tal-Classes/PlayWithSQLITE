using PlayWithSQLITE.Models;
using PlayWithSQLITE.Models.DTOs;

namespace PlayWithSQLITE.Services;

/// <summary>
/// מחלקת שירות מסד הנתונים - זוהי נקודת הפתיחה שלכם ב-main!
/// 
/// 🚀 כיצד לעבוד?
/// 1. פתחו את קובץ ההדרכה הראשי: Learning/Guides/00-Master-Assignment.md
/// 2. בצעו את שלב 1: התקינו את החבילות sqlite-net-pcl ו-SQLiteNetExtensions.Async דרך ממשק ה-NuGet הגרפי
///    והשלימו את הקישוטים (Attributes) במחלקות בתיקיית Models.
/// 3. בצעו את שלב 2: הוסיפו כאן using לספריית SQLite ואתחלו את הקשר למסד הנתונים.
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
    // 💡 שלב 2: לאחר התקנת החבילות, איזה using directive עליכם להוסיף בראש הקובץ לעבודה עם SQLite?
    // וכיצד מגדירים כאן שדה עבור אובייקט החיבור האסינכרוני למסד? (חשבו: מהו טיפוס הנתונים שמספקת החבילה לחיבור אסינכרוני?)
    // רמז: טיפוס החיבור מכיל את המילה Async בשמו.

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
        // 💡 אתחול מסד הנתונים (שלב 2):
        // 1. מניעת אתחול כפול: כיצד נוודא שאם החיבור כבר אותחל, לא נבצע אתחול מחדש?
        // 2. יצירת החיבור: כיצד מאתחלים את אובייקט החיבור האסינכרוני עם נתיב הקובץ ודגלי הפתיחה המתאימים (קריאה/כתיבה, יצירה אם לא קיים, ומטמון משותף)?
        // 3. יצירת הטבלאות: באיזו מתודה אסינכרונית גנרית יוצרים את ארבע הטבלאות (Teacher, Course, Student, Enrollment)?
        await Task.CompletedTask;
    }

    public async Task ClearAllDataAsync()
    {
        // 💡 ניקוי נתונים:
        // באיזו מתודה אסינכרונית של החיבור מוחקים את כל הרשומות מטבלה מסוימת?
        await Task.CompletedTask;
    }

    public async Task SeedSampleDataAsync()
    {
        // 💡 זריעת נתוני הדגמה (שלב 2 ושלב 3):
        // כיצד מייצרים נתוני פתיחה (מורים, קורסים, תלמידים ורישומים) ושומרים אותם במסד כך שלמערכת יהיו נתונים מוכנים להצגה?
        await Task.CompletedTask;
    }

    // ==========================================
    // שלב 2: CRUD בסיסי - מורים (ראו מדריך 02)
    // ==========================================

    public async Task<List<Teacher>> GetTeachersAsync()
    {
        // 💡 שליפת רשימה ממוינת:
        // באיזו מתודה ניגשים לטבלת המורים מתוך החיבור?
        // כיצד ממיינים את התוצאות לפי שם המורה (FullName) ומחזירים רשימה אסינכרונית?
        return await Task.FromResult(new List<Teacher>());
    }

    public async Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false)
    {
        // 💡 שליפה לפי מזהה יחיד:
        // שלב 2: באיזו מתודה שולפים רשומה בודדת על פי המפתח הראשי שלה?
        // שלב 3 (קשרים): אם התבקשנו לטעון גם את הקורסים של המורה (includeCourses == true),
        // באיזו מתודת הרחבה ייעודית של SQLiteNetExtensions משתמשים לטעינת הישות יחד עם הילדים שלה?
        return await Task.FromResult<Teacher?>(null);
    }

    public async Task<int> SaveTeacherAsync(Teacher teacher)
    {
        // 💡 יצירה לעומת עדכון (Upsert Pattern):
        // כיצד מבדילים בין מורה חדש שטרם נשמר (מה יהיה ערך ה-Id שלו?) לבין מורה קיים שערכנו?
        // באיזו מתודה משתמשים להכנסת רשומה חדשה ובאיזו לעדכון רשומה קיימת?
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteTeacherAsync(Teacher teacher)
    {
        // 💡 מחיקה:
        // באיזו מתודה אסינכרונית מוחקים את הישות מהמסד?
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 2 ושלב 3: CRUD וקשרים - קורסים (ראו מדריך 02 ומדריך 03)
    // ==========================================

    public async Task<List<Course>> GetCoursesAsync(bool includeRelations = false)
    {
        // 💡 שליפת קורסים וקשרי גומלין:
        // שלב 2: כיצד שולפים את כל הקורסים הפשוטים?
        // שלב 3: אם includeRelations שווה ל-true, באיזו מתודת הרחבה משתמשים כדי לטעון יחד איתם גם את המורה ואת התלמידים הרשומים?
        return await Task.FromResult(new List<Course>());
    }

    public async Task<Course?> GetCourseByIdAsync(int id, bool includeRelations = true)
    {
        // 💡 שליפת קורס בודד:
        // כיצד שולפים קורס ספציפי לפי ה-Id שלו, כולל או ללא קשרי הגומלין?
        return await Task.FromResult<Course?>(null);
    }

    public async Task<int> SaveCourseAsync(Course course)
    {
        // 💡 שמירה/עדכון קורס:
        // יישמו דפוס Upsert לפי ה-Id של הקורס.
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteCourseAsync(Course course)
    {
        // 💡 מחיקת קורס:
        // מחקו את הקורס מהמסד באופן אסינכרוני.
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 2 ושלב 4: CRUD ודפדוף - תלמידים (ראו מדריך 02 ומדריך 04)
    // ==========================================

    public async Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20)
    {
        // 💡 חלוקה לעמודים (Pagination - שלב 4):
        // אילו שתי מתודות LINQ מאפשרות "לדלג" על מספר רשומות מסוים (skip) ו"לקחת" רק כמות מבוקשת של רשומות (take)?
        return await Task.FromResult(new List<Student>());
    }

    public async Task<int> GetStudentsCountAsync()
    {
        // 💡 ספירת רשומות:
        // באיזו מתודת צבירה אסינכרונית סופרים את סך כל התלמידים הקיימים בטבלה?
        return await Task.FromResult(0);
    }

    public async Task<Student?> GetStudentByIdAsync(int id, bool includeCourses = false)
    {
        // 💡 שליפת תלמיד בודד:
        // שלפו את התלמיד לפי מזהה, וטפלו בטעינת קורסים לפי הצורך.
        return await Task.FromResult<Student?>(null);
    }

    public async Task<int> SaveStudentAsync(Student student)
    {
        // 💡 שמירה/עדכון תלמיד:
        // יישמו דפוס Upsert לפי ה-Id של התלמיד.
        return await Task.FromResult(0);
    }

    public async Task<int> DeleteStudentAsync(Student student)
    {
        // 💡 מחיקת תלמיד:
        // מחקו את התלמיד מהמסד.
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 3: קשרי גומלין ורישומים (Enrollments - ראו מדריך 03)
    // ==========================================

    public async Task<List<Enrollment>> GetEnrollmentsAsync()
    {
        // 💡 שליפת רישומים עם פרטי התלמיד והקורס:
        // באיזו מתודה טוענים את כל רשומות הרישום כולל ישויות הניווט (Student ו-Course)?
        return await Task.FromResult(new List<Enrollment>());
    }

    public async Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null)
    {
        // 💡 רישום תלמיד לקורס:
        // כיצד יוצרים רשומת גישור חדשה של Enrollment עם ה-Foreign Keys המתאימים ושומרים אותה במסד?
        return await Task.FromResult(0);
    }

    public async Task<int> UpdateEnrollmentGradeAsync(int enrollmentId, double grade)
    {
        // 💡 עדכון ציון:
        // כיצד שולפים את רשומת הרישום לפי המזהה שלה, מעדכנים את שדה הציון ושומרים את השינוי?
        return await Task.FromResult(0);
    }

    public async Task<int> RemoveEnrollmentAsync(int studentId, int courseId)
    {
        // 💡 ביטול רישום:
        // כיצד מאתרים את רשומת הרישום המחברת בין ה-studentId ל-courseId ומוחקים אותה?
        return await Task.FromResult(0);
    }

    // ==========================================
    // שלב 4: שאילתות מורכבות: JOIN, GROUP BY, SUM, COUNT, AVG (ראו מדריך 04)
    // ==========================================

    public async Task<List<CourseGradeSummary>> GetCourseGradeSummariesAsync()
    {
        // 💡 שאילתת קיבוץ וצירוף טבלאות (שלב 4):
        // כדי לקבל סטטיסטיקה לכל קורס (ממוצע ציונים, ציון מקסימלי, מינימלי ומספר רשומים):
        // 1. באיזה סוג של JOIN יש להשתמש כדי שגם קורס ללא נרשמים יופיע בתוצאות (INNER או LEFT)?
        // 2. לפי איזה שדה יש לקבץ את התוצאות (GROUP BY)?
        // 3. באילו פונקציות צבירה (Aggregate Functions) ב-SQL נשתמש לחישוב ממוצע, מינימום, מקסימום וספירה?
        // 4. באיזו מתודה של החיבור מריצים שאילתת SQL ישירה המחזירה אובייקטים מסוג DTO (ראו: QueryAsync)?
        return await Task.FromResult(new List<CourseGradeSummary>());
    }

    public async Task<List<StudentTranscriptItem>> GetStudentTranscriptAsync(int studentId)
    {
        // 💡 חיבור שלוש טבלאות (Multi-Table JOIN):
        // גיליון הציונים של התלמיד דורש שילוב נתונים מ-Enrollments, Courses, ו-Teachers.
        // כיצד מנסחים שאילתת SELECT המחברת את 3 הטבלאות הללו ומסננת לפי תעודת הזהות/מזהה התלמיד?
        return await Task.FromResult(new List<StudentTranscriptItem>());
    }

    public async Task<List<GradeLevelCount>> GetStudentDistributionByGradeLevelAsync()
    {
        // 💡 התפלגות תלמידים לפי שכבה:
        // כיצד כותבים שאילתת GROUP BY הסופרת בעזרת COUNT כמה תלמידים קיימים בכל שכבת גיל (GradeLevel)?
        return await Task.FromResult(new List<GradeLevelCount>());
    }

    public async Task<int> GetTotalCreditsForStudentAsync(int studentId)
    {
        // 💡 חישוב סכום (SUM):
        // כיצד מחשבים את סך כל נקודות הזכות (Credits) של הקורסים בהם רשום תלמיד מסוים?
        // באילו שתי טבלאות נדרש לעשות שימוש ובאיזו פונקציית צבירה?
        return await Task.FromResult(0);
    }
}
