using PlayWithSQLITE.Models;
using PlayWithSQLITE.Models.DTOs;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace PlayWithSQLITE.Services;

public class DatabaseService : IDatabaseService
{
    private SQLiteAsyncConnection? _database;
    private readonly string _dbPath;

    public DatabaseService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "SchoolLms.db3");
    }

    // בנאי נוסף המאפשר הזרקת נתיב מותאם (שימושי לבדיקות יחידה בזיכרון או נתיב זמני)
    public DatabaseService(string dbPath)
    {
        _dbPath = dbPath;
    }

    public async Task InitializeAsync()
    {
        if (_database != null)
            return;

        _database = new SQLiteAsyncConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

        // יצירת הטבלאות במסד הנתונים
        await _database.CreateTableAsync<Teacher>();
        await _database.CreateTableAsync<Course>();
        await _database.CreateTableAsync<Student>();
        await _database.CreateTableAsync<Enrollment>();
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
        await _database!.DeleteAllAsync<Enrollment>();
        await _database.DeleteAllAsync<Course>();
        await _database.DeleteAllAsync<Student>();
        await _database.DeleteAllAsync<Teacher>();
    }

    public async Task SeedSampleDataAsync()
    {
        await EnsureInitializedAsync();

        // בדיקה האם כבר קיימים נתונים
        var teacherCount = await _database!.Table<Teacher>().CountAsync();
        if (teacherCount > 0)
            return;

        // 1. הוספת מורים
        var teacher1 = new Teacher { FullName = "ישראל ישראלי", Email = "israel@school.org", Department = "מדעי המחשב" };
        var teacher2 = new Teacher { FullName = "שרה לוי", Email = "sara@school.org", Department = "מתמטיקה" };
        var teacher3 = new Teacher { FullName = "דוד כהן", Email = "david@school.org", Department = "פיזיקה" };

        await _database.InsertAsync(teacher1);
        await _database.InsertAsync(teacher2);
        await _database.InsertAsync(teacher3);

        // 2. הוספת קורסים עם מפתחות זרים של המורים
        var course1 = new Course { Title = "תכנות ב-C# ו-MAUI", Credits = 5, Description = "פיתוח אפליקציות מובייל היברידיות", TeacherId = teacher1.Id };
        var course2 = new Course { Title = "מבני נתונים ואלגוריתמים", Credits = 4, Description = "יסודות תיאורטיים ומעשיים", TeacherId = teacher1.Id };
        var course3 = new Course { Title = "חדו\"א לתלמידי מדעי המחשב", Credits = 4, Description = "חשבון אינפיניטסימלי", TeacherId = teacher2.Id };
        var course4 = new Course { Title = "מכניקה קלאסית וחשמל", Credits = 3, Description = "מעבדות ופיזיקה מעשית", TeacherId = teacher3.Id };

        await _database.InsertAsync(course1);
        await _database.InsertAsync(course2);
        await _database.InsertAsync(course3);
        await _database.InsertAsync(course4);

        // 3. הוספת תלמידים
        var students = new List<Student>
        {
            new() { FirstName = "נועם", LastName = "אברהם", Email = "noam@students.org", GradeLevel = 11, BirthDate = new DateTime(2009, 3, 15) },
            new() { FirstName = "מאיה", LastName = "שלום", Email = "maya@students.org", GradeLevel = 11, BirthDate = new DateTime(2009, 7, 22) },
            new() { FirstName = "יונתן", LastName = "מזרחי", Email = "yonatan@students.org", GradeLevel = 12, BirthDate = new DateTime(2008, 1, 10) },
            new() { FirstName = "תמר", LastName = "גבאי", Email = "tamar@students.org", GradeLevel = 12, BirthDate = new DateTime(2008, 11, 5) },
            new() { FirstName = "אורי", LastName = "אלון", Email = "ori@students.org", GradeLevel = 10, BirthDate = new DateTime(2010, 5, 30) },
            new() { FirstName = "רוני", LastName = "פרץ", Email = "roni@students.org", GradeLevel = 10, BirthDate = new DateTime(2010, 9, 18) }
        };

        foreach (var s in students)
        {
            await _database.InsertAsync(s);
        }

        // 4. רישום תלמידים לקורסים (טבלת Enrollment)
        var enrollments = new List<Enrollment>
        {
            new() { StudentId = students[0].Id, CourseId = course1.Id, Grade = 95.0, IsPaid = true },
            new() { StudentId = students[0].Id, CourseId = course2.Id, Grade = 88.0, IsPaid = true },
            new() { StudentId = students[1].Id, CourseId = course1.Id, Grade = 92.0, IsPaid = true },
            new() { StudentId = students[1].Id, CourseId = course3.Id, Grade = 85.0, IsPaid = true },
            new() { StudentId = students[2].Id, CourseId = course2.Id, Grade = 78.0, IsPaid = true },
            new() { StudentId = students[2].Id, CourseId = course4.Id, Grade = 89.0, IsPaid = true },
            new() { StudentId = students[3].Id, CourseId = course1.Id, Grade = 100.0, IsPaid = true },
            new() { StudentId = students[3].Id, CourseId = course4.Id, Grade = 91.0, IsPaid = true },
            new() { StudentId = students[4].Id, CourseId = course3.Id, Grade = 84.0, IsPaid = false },
            new() { StudentId = students[5].Id, CourseId = course1.Id, Grade = 90.0, IsPaid = true }
        };

        foreach (var e in enrollments)
        {
            await _database.InsertAsync(e);
        }
    }

    // ==========================================
    // פעולות CRUD - מורים
    // ==========================================

    public async Task<List<Teacher>> GetTeachersAsync()
    {
        await EnsureInitializedAsync();
        return await _database!.Table<Teacher>().OrderBy(t => t.FullName).ToListAsync();
    }

    public async Task<Teacher?> GetTeacherByIdAsync(int id, bool includeCourses = false)
    {
        await EnsureInitializedAsync();
        if (includeCourses)
        {
            return await _database!.GetWithChildrenAsync<Teacher>(id);
        }
        return await _database!.Table<Teacher>().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> SaveTeacherAsync(Teacher teacher)
    {
        await EnsureInitializedAsync();
        if (teacher.Id != 0)
        {
            return await _database!.UpdateAsync(teacher);
        }
        return await _database!.InsertAsync(teacher);
    }

    public async Task<int> DeleteTeacherAsync(Teacher teacher)
    {
        await EnsureInitializedAsync();
        // מחיקת קורסים הקשורים למורה במידה ויש
        var courses = await _database!.Table<Course>().Where(c => c.TeacherId == teacher.Id).ToListAsync();
        foreach (var c in courses)
        {
            await DeleteCourseAsync(c);
        }
        return await _database.DeleteAsync(teacher);
    }

    // ==========================================
    // פעולות CRUD - קורסים
    // ==========================================

    public async Task<List<Course>> GetCoursesAsync(bool includeRelations = false)
    {
        await EnsureInitializedAsync();
        if (includeRelations)
        {
            return await _database!.GetAllWithChildrenAsync<Course>();
        }
        return await _database!.Table<Course>().OrderBy(c => c.Title).ToListAsync();
    }

    public async Task<Course?> GetCourseByIdAsync(int id, bool includeRelations = true)
    {
        await EnsureInitializedAsync();
        if (includeRelations)
        {
            return await _database!.GetWithChildrenAsync<Course>(id);
        }
        return await _database!.Table<Course>().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<int> SaveCourseAsync(Course course)
    {
        await EnsureInitializedAsync();
        if (course.Id != 0)
        {
            return await _database!.UpdateAsync(course);
        }
        return await _database!.InsertAsync(course);
    }

    public async Task<int> DeleteCourseAsync(Course course)
    {
        await EnsureInitializedAsync();
        // מחיקת רישומים של הקורס
        var enrollments = await _database!.Table<Enrollment>().Where(e => e.CourseId == course.Id).ToListAsync();
        foreach (var e in enrollments)
        {
            await _database.DeleteAsync(e);
        }
        return await _database.DeleteAsync(course);
    }

    // ==========================================
    // פעולות CRUD ודפדוף - תלמידים (SKIP, TAKE, COUNT)
    // ==========================================

    public async Task<List<Student>> GetStudentsAsync(int skip = 0, int take = 20)
    {
        await EnsureInitializedAsync();
        // שימוש ב-OrderBy, Skip ו-Take לצורך Pagination
        return await _database!.Table<Student>()
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetStudentsCountAsync()
    {
        await EnsureInitializedAsync();
        // פעולת COUNT על טבלת התלמידים
        return await _database!.Table<Student>().CountAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(int id, bool includeCourses = false)
    {
        await EnsureInitializedAsync();
        if (includeCourses)
        {
            return await _database!.GetWithChildrenAsync<Student>(id);
        }
        return await _database!.Table<Student>().FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<int> SaveStudentAsync(Student student)
    {
        await EnsureInitializedAsync();
        if (student.Id != 0)
        {
            return await _database!.UpdateAsync(student);
        }
        return await _database!.InsertAsync(student);
    }

    public async Task<int> DeleteStudentAsync(Student student)
    {
        await EnsureInitializedAsync();
        // מחיקת רישומים
        var enrollments = await _database!.Table<Enrollment>().Where(e => e.StudentId == student.Id).ToListAsync();
        foreach (var e in enrollments)
        {
            await _database.DeleteAsync(e);
        }
        return await _database.DeleteAsync(student);
    }

    // ==========================================
    // קשרי גומלין ורישומים (Enrollments)
    // ==========================================

    public async Task<List<Enrollment>> GetEnrollmentsAsync()
    {
        await EnsureInitializedAsync();
        return await _database!.GetAllWithChildrenAsync<Enrollment>();
    }

    public async Task<int> EnrollStudentAsync(int studentId, int courseId, double? grade = null)
    {
        await EnsureInitializedAsync();
        var existing = await _database!.Table<Enrollment>()
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        if (existing != null)
        {
            existing.Grade = grade;
            return await _database.UpdateAsync(existing);
        }

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            Grade = grade,
            EnrollmentDate = DateTime.Now,
            IsPaid = true
        };
        return await _database.InsertAsync(enrollment);
    }

    public async Task<int> UpdateEnrollmentGradeAsync(int enrollmentId, double grade)
    {
        await EnsureInitializedAsync();
        var enrollment = await _database!.Table<Enrollment>().FirstOrDefaultAsync(e => e.Id == enrollmentId);
        if (enrollment == null)
            return 0;

        enrollment.Grade = grade;
        return await _database.UpdateAsync(enrollment);
    }

    public async Task<int> RemoveEnrollmentAsync(int studentId, int courseId)
    {
        await EnsureInitializedAsync();
        var existing = await _database!.Table<Enrollment>()
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        if (existing != null)
        {
            return await _database.DeleteAsync(existing);
        }
        return 0;
    }

    // ==========================================
    // שאילתות מורכבות: JOIN, GROUP BY, SUM, COUNT, AVG
    // ==========================================

    public async Task<List<CourseGradeSummary>> GetCourseGradeSummariesAsync()
    {
        await EnsureInitializedAsync();

        // שאילתת SQL מרובת טבלאות המשלבת LEFT JOIN עם GROUP BY ופונקציות צבירה (COUNT, AVG, MAX, MIN)
        const string query = @"
            SELECT 
                c.Id AS CourseId,
                c.Title AS CourseTitle,
                IFNULL(t.FullName, 'לא הוקצה מורה') AS TeacherName,
                COUNT(e.Id) AS StudentCount,
                IFNULL(AVG(e.Grade), 0.0) AS AverageGrade,
                IFNULL(MAX(e.Grade), 0.0) AS MaxGrade,
                IFNULL(MIN(e.Grade), 0.0) AS MinGrade
            FROM Courses c
            LEFT JOIN Teachers t ON c.TeacherId = t.Id
            LEFT JOIN Enrollments e ON c.Id = e.CourseId
            GROUP BY c.Id, c.Title, t.FullName
            ORDER BY c.Title;
        ";

        return await _database!.QueryAsync<CourseGradeSummary>(query);
    }

    public async Task<List<StudentTranscriptItem>> GetStudentTranscriptAsync(int studentId)
    {
        await EnsureInitializedAsync();

        // שאילתת JOIN המשלבת שלוש טבלאות (Enrollments, Courses, Teachers) כדי לחלץ גיליון ציונים אישי
        const string query = @"
            SELECT 
                s.Id AS StudentId,
                (s.FirstName || ' ' || s.LastName) AS StudentName,
                c.Title AS CourseTitle,
                c.Credits AS Credits,
                IFNULL(t.FullName, 'לא הוקצה מורה') AS TeacherName,
                e.Grade AS Grade,
                e.IsPaid AS IsPaid
            FROM Enrollments e
            INNER JOIN Students s ON e.StudentId = s.Id
            INNER JOIN Courses c ON e.CourseId = c.Id
            LEFT JOIN Teachers t ON c.TeacherId = t.Id
            WHERE e.StudentId = ?
            ORDER BY c.Title;
        ";

        return await _database!.QueryAsync<StudentTranscriptItem>(query, studentId);
    }

    public async Task<List<GradeLevelCount>> GetStudentDistributionByGradeLevelAsync()
    {
        await EnsureInitializedAsync();

        // שאילתת GROUP BY עם COUNT לחישוב התפלגות תלמידים לפי שכבה
        const string query = @"
            SELECT 
                GradeLevel,
                COUNT(Id) AS StudentCount
            FROM Students
            GROUP BY GradeLevel
            ORDER BY GradeLevel;
        ";

        return await _database!.QueryAsync<GradeLevelCount>(query);
    }

    public async Task<int> GetTotalCreditsForStudentAsync(int studentId)
    {
        await EnsureInitializedAsync();

        // שימוש בפונקציית SUM בשילוב INNER JOIN כדי לחשב סך נקודות זכות (Credits)
        const string query = @"
            SELECT IFNULL(SUM(c.Credits), 0)
            FROM Enrollments e
            INNER JOIN Courses c ON e.CourseId = c.Id
            WHERE e.StudentId = ?;
        ";

        return await _database!.ExecuteScalarAsync<int>(query, studentId);
    }
}
