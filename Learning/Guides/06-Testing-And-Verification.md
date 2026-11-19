# ✅ שלב 6: בדיקות יחידה ב-xUnit v3 ואימות מלא של המערכת

בשלב הסופי נבדוק שהמערכת כולה — החל מהמודלים ו-`Constants`, דרך `DatabaseService` ושאילתות ה-SQL, ועד ל-DI ולפקודות ה-`ICommand` ב-ViewModels — עובדת בצורה אמינה, אוטומטית ומאומתת.

---

## 1. מה מיוחד בבדיקות יחידה עם **xUnit v3** ב-.NET 10?

פרויקט הבדיקות [PlayWithSQLITE.Tests.csproj](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/PlayWithSQLITE.Tests/PlayWithSQLITE.Tests.csproj) מוגדר עם הדור החדש של בדיקות היחידה ב-.NET:
1. **חבילת `xunit.v3`** (גרסה `4.0.3`) + מנוע ההרצה המודרני **`Microsoft.Testing.Platform`** (מוגדר ב-[global.json](file:///c:/Users/Owner/source/repos/PlayWithSQLITE/global.json)).
2. **פרויקט הפעלה עצמאי (`<OutputType>Exe</OutputType>`):** פרויקט הבדיקות מתקמפל לקובץ הרצה מהיר ללא תלות ב-VSTest הישן, ורץ ישירות דרך `dotnet test` או Test Explorer ב-Visual Studio / Rider.
3. **מחזור חיים אסינכרוני עם `ValueTask` (`IAsyncLifetime`):**
   - `public async ValueTask InitializeAsync()` — רצה לפני **כל בדיקה**, מאתחלת קובץ SQLite זמני מבודד בתיקיית `Path.GetTempPath()` וזורעת נתוני בדיקה.
   - `public ValueTask DisposeAsync()` — רצה בסיום **כל בדיקה** ומוחקת את הקובץ הזמני כדי שכל בדיקה תהיה עצמאית לחלוטין (Isolated).

---

## 2. בדיקת שכבת מסד הנתונים ובדיקת ה-ViewModels

בפרויקט הבדיקות קיימות הבדיקות הבאות:
1. **`ArchitectureSmokeTests`**: בדיקת תקינות בסיסית של סביבת xUnit v3.
2. **`DatabaseServiceTests`** (8 בדיקות מלאות):
   - CRUD וזריעת מורים (`GetTeachersAsync_ReturnsSeededTeachers`)
   - קשר `1:N` בין קורס למורה (`GetCoursesAsync_WithRelations_LoadsTeacherDetails`)
   - דפדוף `Skip` / `Take` (`Pagination_SkipAndTake_ReturnsCorrectSlices`)
   - `LEFT JOIN` + `GROUP BY` + `COUNT`/`AVG`/`MAX`/`MIN` (`GetCourseGradeSummariesAsync_PerformsJoinAndGroupBy`)
   - `INNER JOIN` + `LEFT JOIN` של 4 טבלאות (`GetStudentTranscriptAsync_ReturnsMultiTableJoinedItems`)
   - `INNER JOIN` + `SUM` + `ExecuteScalarAsync` (`GetTotalCreditsForStudentAsync_CalculatesSumOfCredits`)
   - `GROUP BY` + `COUNT` (`GetStudentDistributionByGradeLevelAsync_GroupsAndCountsStudents`)
   - קשר `M:N` דרך `Enrollment` (`EnrollStudentAsync_CreatesNewEnrollment`)

---

## 3. רשימת תיוג (Checklist) לאימות סופי לפני הגשה

הריצו את הפקודות הבאות מתוך שורש הפרויקט וודאו שכולן עוברות ללא שגיאות:

1. **בניית הפתרון המלא:**
   ```powershell
   dotnet build
   ```
2. **בדיקת רישום DI Container (מבטיח ש-`DatabaseService` וכל ה-ViewModels רשומים):**
   ```powershell
   .\Scripts\Audit-DI.ps1
   ```
3. **הרצת כל בדיקות ה-xUnit v3:**
   ```powershell
   dotnet test PlayWithSQLITE.Tests\PlayWithSQLITE.Tests.csproj
   ```
   *(או דרך כלי הבקרה: `.\Scripts\Validate-Tests.ps1`)*
4. **הרצת האפליקציה בפועל:**
   - היכנסו למסך **דוחות** ➡️ לחצו **זריעת נתוני הדגמה** ➡️ בדקו שממוצעי הקורסים והתפלגות השכבות מתמלאים.
   - היכנסו למסך **תלמידים** ➡️ דפדפו בין עמודים (`הבא` / `הקודם`), לחצו על תלמיד וראו את גיליון הציונים וסך נ"ז שלו, והוסיפו תלמיד חדש.
   - היכנסו למסך **קורסים** ➡️ בחרו מורה ב-`Picker`, צרו קורס חדש וודאו ששם המורה מוצג בכרטיס הקורס.
   - היכנסו למסך **מורים** ➡️ צפו בקורסים המשוייכים לכל מורה ובדקו הוספה ומחיקה.

🏆 **כל הכבוד!** השלמתם פרויקט מלא ומקצועי של SQLite מרובה-טבלאות ב-.NET MAUI.  
להשוואה מול הפתרון השלם והמאומת ביותר: `git checkout step-06-full-solution`.
