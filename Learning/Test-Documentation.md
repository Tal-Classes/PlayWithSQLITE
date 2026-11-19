# תיעוד בדיקות יחידה (xUnit v3 Test Documentation) — הכנה להגנה על הפרויקט

מסמך זה מרכז את כל בדיקות היחידה (`xUnit v3` על גבי `Microsoft.Testing.Platform`) בפרויקט `PlayWithSQLITE.Tests`.
הפרויקט עושה שימוש בגרסה העדכנית ביותר של xUnit (גרסה 3), הכוללת הרצה ישירה כקובץ הפעלה (Executable), מחזור חיים אסינכרוני מבוסס `ValueTask` (`IAsyncLifetime`), ואינטגרציה מלאה ל-SDK של .NET 10.
הוא מתעד מה כל בדיקה מאמתת לפי תבנית **Arrange – Act – Assert (AAA)**, אילו רכיבי לוגיקה (`DatabaseService`, `Models`, `DTOs`, `ViewModels`) מכוסים, ואיך להסביר כל בדיקה לבוחן בבחינה בעל-פה (25% "שליטה בקוד").

---

## 1. טבלת ריכוז בדיקות היחידה בפרויקט (`PlayWithSQLITE.Tests`)

| # | מחלקת הבדיקה (`Test Class`) | שם הבדיקה (`Test Method`) | הרכיב הנבדק (`Target Component`) | מי כתב? (`Agent` / `Student` / `Combined`) | תרחיש הבדיקה (`Arrange -> Act -> Assert`) | מה להסביר לבוחן בבחינה בעל-פה? |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| 1 | `ArchitectureSmokeTests` | `TestHarness_WhenInitialized_PassesSanityCheck` | Harness | `Agent` | אימות שתשתית `xUnit` פעילה. | כיצד בדיקות יחידה רצות ללא צורך באמולטור. |
| 2 | `DatabaseServiceTests` | `GetTeachersAsync_ReturnsSeededTeachers` | `DatabaseService` (CRUD) | `Agent` | שליפת מורים ממסד הנתונים ובדיקה שהמידע נשמר ונשלף במלואו. | כיצד פקודת `Table<Teacher>().ToListAsync()` מבצעת `SELECT *` אסינכרוני. |
| 3 | `DatabaseServiceTests` | `GetCoursesAsync_WithRelations_LoadsTeacherDetails` | `DatabaseService` (1:N) | `Agent` | אימות טעינת קשר 1:N בעזרת `GetWithChildrenAsync`. | כיצד שימוש בספריית `SQLiteNetExtensions.Async` מונע כתיבת שאילתות ידניות לטעינת אובייקט מקושר. |
| 4 | `DatabaseServiceTests` | `Pagination_SkipAndTake_ReturnsCorrectSlices` | `DatabaseService` (SKIP/TAKE) | `Agent` | חלוקת תלמידים לעמודים ובדיקה שאין חפיפה בין עמוד ראשון לעמוד שני. | כיצד שיטות `Skip` ו-`Take` מתורגמות ל-`LIMIT` ו-`OFFSET` ב-SQL לצורך דפדוף מהיר. |
| 5 | `DatabaseServiceTests` | `GetCourseGradeSummariesAsync_PerformsJoinAndGroupBy` | `DatabaseService` (JOIN, GROUP BY) | `Agent` | שאילתת קיבוץ המחשבת ממוצע ציונים וכמות תלמידים לפי קורס ומורה. | כיצד שימוש ב-`LEFT JOIN` ו-`GROUP BY` מאפשר להפיק דוח סטטיסטי מסכם בפנייה בודדת למסד. |
| 6 | `DatabaseServiceTests` | `GetStudentTranscriptAsync_ReturnsMultiTableJoinedItems` | `DatabaseService` (JOIN 3 טבלאות) | `Agent` | אימות שליפת גיליון ציונים המשלב `Students`, `Courses`, `Teachers` ו-`Enrollments`. | כיצד `INNER JOIN` מחבר טבלאות לפי מפתחות זרים (`StudentId`, `CourseId`). |
| 7 | `DatabaseServiceTests` | `GetTotalCreditsForStudentAsync_CalculatesSumOfCredits` | `DatabaseService` (SUM) | `Agent` | חישוב סך נקודות הזכות של תלמיד בעזרת `SUM(c.Credits)`. | כיצד `ExecuteScalarAsync` מחזיר ערך סקלרי מספרי בודד משאילתת אגרגציה. |
| 8 | `DatabaseServiceTests` | `GetStudentDistributionByGradeLevelAsync_GroupsAndCountsStudents` | `DatabaseService` (GROUP BY, COUNT) | `Agent` | קיבוץ וספירת תלמידים לפי שכבת לימוד (`GradeLevel`). | כיצד `COUNT(Id)` יחד עם `GROUP BY` מייצרים התפלגות קטגוריאלית. |
| 9 | `DatabaseServiceTests` | `EnrollStudentAsync_CreatesNewEnrollment` | `DatabaseService` (M:N) | `Agent` | רישום תלמיד לקורס דרך טבלת הקשר `Enrollment` ובדיקת עדכון ציון. | כיצד מודל M:N ממומש הלכה למעשה ב-SQLite באמצעות טבלת ביניים ומשקף מציאות של בית ספר. |
| 10 | `ViewModelsTests` | `TeachersViewModel_LoadTeachersAsync_PopulatesTeachersWithCourses` | `TeachersViewModel` (`ICommand` + `1:N`) | `Agent` | בדיקת `CanExecute` של `AddTeacherCommand` וטעינת מורים כולל רשימת הקורסים שלהם. | כיצד `ChangeCanExecute()` מעדכן כפתורים ב-XAML וכיצד ViewModel צורך `IDatabaseService`. |
| 11 | `ViewModelsTests` | `CoursesViewModel_LoadCoursesAsync_PopulatesTeachersAndCoursesWithRelations` | `CoursesViewModel` (`ICommand` + `N:1`) | `Agent` | טעינת מורים וקורסים מקושרים ואימות תנאי `CanAddCourse` מול `SelectedTeacher`. | כיצד קשר `ManyToOne` מוצג במסך הקורסים ואיך `SelectedTeacher` מספק את המפתח הזר `TeacherId`. |
| 12 | `ViewModelsTests` | `StudentsViewModel_LoadStudentsAsync_HandlesPaginationAndCanExecute` | `StudentsViewModel` (Pagination + `ICommand`) | `Agent` | טעינת עמוד ראשון של תלמידים ובדיקת `PrevPageCommand` (`false`) מול `NextPageCommand` (`true`). | כיצד ה-ViewModel מנהל עמודים מול `Skip`/`Take` ומונע ניווט מחוץ לטווח דרך `CanExecute`. |
| 13 | `ViewModelsTests` | `ReportsViewModel_LoadReportsAsync_PopulatesSummariesAndDistributions` | `ReportsViewModel` (JOIN/GROUP BY) | `Agent` | טעינת דוחות ציונים והתפלגות שכבות לתוך `ObservableCollection` ועדכון `StatusMessage`. | כיצד תוצאות שאילתות DTO זורמות משכבת השירות אל `CollectionView` במסך הדוחות. |
