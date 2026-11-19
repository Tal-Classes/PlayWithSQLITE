# ✅ שלב 6: אימות מלא של המערכת, בדיקות `xUnit v3` ובדיקה חיה באמולטור

בשלב הסופי נאמת שכל שכבות המערכת — `Constants`, 4 המודלים המקושרים, `DatabaseService`, שאילתות ה-SQL המתקדמות, רישום ה-DI ופקודות ה-`ICommand` ב-4 ה-ViewModels — עובדות יחד ללא דופי!

> [!NOTE]
> זקוקים לרענון על איך קוראים קובץ בדיקות, מסירים `Skip`, מריצים ב-Visual Studio / Rider ומנתחים שגיאות?  
> חזרו אל המדריך הייעודי: **[00b-How-To-Run-Tests.pdf](./00b-How-To-Run-Tests.pdf)**.

---

## 1. מה נבדק בחבילת הבדיקות המלאה (`step-06-full-solution`)?

פרויקט הבדיקות `PlayWithSQLITE.Tests` פועל על **xUnit v3** (`xunit.v3` 4.0.3 + `Microsoft.Testing.Platform` ב-`global.json`) וכולל בפתרון המלא **13 בדיקות אוטומטיות**:
1. **`ArchitectureSmokeTests`** (בדיקה 1): אימות סביבת xUnit v3.
2. **`DatabaseServiceTests`** (8 בדיקות):
   - CRUD וזריעת נתונים (שלב 2)
   - קשר `1:N` (`GetCoursesAsync` עם `includeRelations: true`) וקשר `M:N` (`EnrollStudentAsync`) (שלב 3)
   - דפדוף `Skip`/`Take`, `LEFT JOIN + GROUP BY + AVG/COUNT`, גיליון ציונים מרובה-טבלאות, `SUM` עם `ExecuteScalarAsync`, והתפלגות שכבות (שלב 4)
3. **`ViewModelsTests`** (4 בדיקות בפתרון המלא `step-06-full-solution`):
   - `TeachersViewModel`: אימות `AddTeacherCommand.CanExecute` וטעינת מורים כולל קורסים (`1:N`).
   - `CoursesViewModel`: טעינת מורים וקורסים מקושרים (`N:1`) ואימות `CanAddCourse`.
   - `StudentsViewModel`: דפדוף עמוד 1 מול 2, ואימות `PrevPageCommand.CanExecute == false` מול `NextPageCommand.CanExecute == true`.
   - `ReportsViewModel`: טעינת `CourseSummaries` ו-`GradeDistributions` לתוך `ObservableCollection`.

---

## 2. רשימת תיוג (Checklist) לאימות סופי לפני הגשה

ודאו שכל 3 הבדיקות הבאות עוברות בצורה מלאה:

1. **בניית הפתרון המלא ללא שגיאות (`Build Solution`):**
   - ב-Visual Studio או ב-Rider בצעו **Rebuild Solution** (או במסוף: `dotnet build`) וודאו `0 Error(s)`.
2. **הרצת כל בדיקות היחידה (`xUnit v3`):**
   - ב-Visual Studio: פתחו `Test ➡️ Test Explorer` ולחצו **Run All Tests**.
   - ב-Rider: פתחו `View ➡️ Tool Windows ➡️ Unit Tests` ולחצו **Run All Tests**.
   - או במסוף:
     ```powershell
     dotnet test PlayWithSQLITE.Tests\PlayWithSQLITE.Tests.csproj
     ```
   - ודאו ש**כל הבדיקות ירוקות (`failed: 0`)**!
3. **אימות חי בהרצת האפליקציה (אמולטור Android / Windows):**
   - לשונית **דוחות** ➡️ לחיצה על **צור נתוני הדגמה (Seed)** ➡️ וידוא ש-4 כרטיסי הקורסים (ממוצע, רשומים, ציון שיא) ו-3 שכבות הגיל מוצגים מיד.
   - לשונית **תלמידים** ➡️ מעבר בין עמוד 1 ל-2 (`הבא` / `קודם`), בחירת תלמיד לצפייה בגיליון הציונים ובסך נ"ז (`SUM`), והוספת תלמיד חדש.
   - לשונית **קורסים** ➡️ בחירת מורה מה-`Picker`, הוספת קורס חדש ווידוא ששם המורה מוצג בכרטיס.
   - לשונית **מורים** ➡️ צפייה בקורסים המשוייכים לכל מורה ובדיקת הוספה ומחיקה.

🏆 **כל הכבוד!** השלמתם פרויקט מלא ומקצועי של SQLite מרובה-טבלאות ב-.NET MAUI.  
להשוואה מול הפתרון השלם והמאומת ביותר: `git checkout step-06-full-solution`.
