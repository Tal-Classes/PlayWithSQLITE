# 🧪 מדריך למתחילים: איך קוראים, מפעילים ומריצים בדיקות יחידה (`xUnit v3`)?

אם זו הפעם הראשונה שאתם פוגשים **בדיקות יחידה (Unit Tests)** — המדריך הזה נכתב במיוחד עבורכם!  
במקום להריץ את כל האפליקציה באמולטור וללחוץ ידנית על כפתורים אחרי כל שינוי קטן בקוד, בדיקות יחידה הן **תוכנית C# קטנה ומהירה** שבודקת אוטומטית תוך 2 שניות האם `DatabaseService` שלכם שומר, שולף, מחבר טבלאות ומחשב ציונים בצורה מדויקת!

---

## 1. היכן נמצאות הבדיקות בפתרון (Solution)?

בחלון **Solution Explorer** תראו שני פרויקטים:
1. **`PlayWithSQLITE`** — האפליקציה עצמה (מסכים, ViewModels, Models, `DatabaseService`).
2. **`PlayWithSQLITE.Tests`** — פרויקט בדיקות יחידה המבוסס על **xUnit v3** (עם מנוע ההרצה `Microsoft.Testing.Platform` שמוגדר ב-`global.json`).

בתוך `PlayWithSQLITE.Tests` תמצאו שני קבצים:
- **`ArchitectureSmokeTests.cs`** — בדיקת תקינות בסיסית שמוודאת שהסביבה עובדת (עוברת כבר מהרגע הראשון!).
- **`DatabaseServiceTests.cs`** — 8 בדיקות שמאמתות את כל הפעולות שתבנו בשלבים 2, 3 ו-4!

---

## 2. איך קוראים קובץ בדיקות? (אנטומיה של `DatabaseServiceTests.cs`)

פתחו את `PlayWithSQLITE.Tests/DatabaseServiceTests.cs`. כל מחלקת בדיקה בנויה מ-3 חלקים פשוטים:

### חלק א' — הכנה וניקוי אוטומטי לכל בדיקה (`IAsyncLifetime`)
```csharp
public class DatabaseServiceTests : IAsyncLifetime
{
    private readonly string _testDbPath;
    private readonly DatabaseService _databaseService;

    public DatabaseServiceTests()
    {
        // יוצר נתיב זמני ייחודי בתיקיית Temp — לא נוגע במסד האמיתי של האפליקציה!
        _testDbPath = Path.Combine(Path.GetTempPath(), $"SchoolLms_Test_{Guid.NewGuid():N}.db3");
        _databaseService = new DatabaseService(_testDbPath);
    }

    // רצה אוטומטית לפני כל בדיקה (ב-xUnit v3 טיפוס ההחזרה הוא ValueTask!)
    public async ValueTask InitializeAsync()
    {
        await _databaseService.SeedSampleDataAsync();
    }

    // רצה אוטומטית בסיום כל בדיקה — מוחקת את הקובץ הזמני כדי שהמחשב יישאר נקי!
    public ValueTask DisposeAsync()
    {
        if (File.Exists(_testDbPath)) File.Delete(_testDbPath);
        return ValueTask.CompletedTask;
    }
}
```
> [!TIP]
> **למה כל בדיקה מקבלת קובץ `.db3` זמני חדש עם `Guid.NewGuid()`?**  
> כדי שהבדיקות לא "ילכלכו" אחת את השנייה! גם אם בדיקה אחת מוחקת תלמידים, הבדיקה הבאה מקבלת מסד חדש, נקי ומסודר.

### חלק ב' — מבנה בדיקה בודדת לפי תבנית **AAA (Arrange – Act – Assert)**
כל מתודה שמסומנת בדקורציה **`[Fact]`** היא בדיקה אחת עצמאית:
```csharp
[Fact]
public async Task GetTeachersAsync_ReturnsSeededTeachers()
{
    // 1. Arrange (הכנה): הנתונים כבר נזרעו ב-InitializeAsync()

    // 2. Act (הפעלה): קוראים לפעולה שאנחנו רוצים לבדוק
    var teachers = await _databaseService.GetTeachersAsync();

    // 3. Assert (אימות): בודקים שהתוצאה תואמת לציפיות!
    Assert.NotNull(teachers);                                          // הרשימה אינה null
    Assert.True(teachers.Count >= 3);                                  // יש לפחות 3 מורים
    Assert.Contains(teachers, t => t.FullName == "ישראל ישראלי");      // המורה קיים ברשימה
}
```

### פקודות ה-`Assert` הנפוצות שתראו בקובץ:
| פקודה | מה היא בודקת? | מתי הבדיקה תיכשל? |
| :--- | :--- | :--- |
| `Assert.NotNull(x)` | ש-`x` אינו `null` | אם המתודה החזירה `null` |
| `Assert.NotEmpty(list)` | שהרשימה מכילה לפחות פריט אחד | אם הרשימה ריקה (`Count == 0`) |
| `Assert.Equal(expected, actual)` | שהערך שקיבלנו שווה בדיוק לערך הצפוי | אם המספרים/מחרוזות שונים |
| `Assert.NotEqual(a, b)` | ששני ערכים שונים זה מזה (למשל עמוד 1 מול עמוד 2) | אם חזרו אותם פריטים |
| `Assert.True(condition)` | שתנאי בוליאני מתקיים (`totalCredits > 0`) | אם התנאי הוא `false` |
| `Assert.Contains(list, x => ...)` | שקיים ברשימה לפחות פריט המקיים תנאי | אם אף פריט לא עמד בתנאי |

---

## 3. איך מפעילים בדיקה? (הסרת `Skip` בהדרגה!)

בענף `main`, רוב הבדיקות ב-`DatabaseServiceTests.cs` מסומנות כך:
```csharp
[Fact(Skip = "שלב 2: יופעל לאחר מימוש GetTeachersAsync ו-SeedSampleDataAsync ב-DatabaseService")]
public async Task GetTeachersAsync_ReturnsSeededTeachers()
```
הפרמטר **`Skip = "..."`** אומר ל-xUnit: *"דלג כרגע על הבדיקה הזו, כי התלמיד עדיין לא הגיע לשלב הזה!"*

**כשאתם מסיימים לממש שלב ורוצים לבדוק את הקוד שלכם:**
1. מצאו את הבדיקה של השלב שסיימתם.
2. מחקו את החלק `(Skip = "...")` כך שיישאר רק **`[Fact]`**:
   ```csharp
   [Fact]
   public async Task GetTeachersAsync_ReturnsSeededTeachers()
   ```
3. שמרו את הקובץ והריצו את הבדיקות!

### מפת הבדיקות לפי שלבים:
| שלב שסיימתם | אילו בדיקות להפעיל (להסיר `Skip`)? |
| :---: | :--- |
| **שלב 2** | `GetTeachersAsync_ReturnsSeededTeachers` |
| **שלב 3** | `GetCoursesAsync_WithRelations_LoadsTeacherDetails`, `EnrollStudentAsync_CreatesNewEnrollment` |
| **שלב 4** | `Pagination_SkipAndTake_ReturnsCorrectSlices`, `GetCourseGradeSummariesAsync_PerformsJoinAndGroupBy`, `GetStudentTranscriptAsync_ReturnsMultiTableJoinedItems`, `GetTotalCreditsForStudentAsync_CalculatesSumOfCredits`, `GetStudentDistributionByGradeLevelAsync_GroupsAndCountsStudents` |

---

## 4. איך מריצים את הבדיקות? (3 דרכים לבחירתכם)

### 🖥️ דרך א': Visual Studio (Windows — VS 2026 / VS 2022)
1. פתחו את חלון **Test Explorer** דרך התפריט העליון:  
   **`Test`** ➡️ **`Test Explorer`** *(או קיצור מקלדת `Ctrl + E, T`)*.
2. בחלק העליון של חלון Test Explorer לחצו על כפתור **`Run All Tests`** (המשולש הירוק הכפול `▶▶`), או קליק ימני על בדיקה אחת ➡️ **`Run`**.
3. רוצים לעצור בנקודת עצירה (Breakpoint) בתוך `DatabaseService`? בצעו קליק ימני על הבדיקה ➡️ **`Debug`**!

### 🍎 דרך ב': JetBrains Rider (macOS / Mac)
1. פתחו את חלון **Unit Tests** דרך התפריט העליון:  
   **`View`** ➡️ **`Tool Windows`** ➡️ **`Unit Tests`** *(או `Cmd + 8`)*.
2. לחצו על אייקון ההרצה הירוק (**Run All Tests**) בראש החלונית — או לחצו ישירות על האייקון הירוק שמופיע בצד שמאל של כל `[Fact]` בתוך עורך הקוד!
3. לבדיקה עם Debugger: לחצו על אייקון החיפושית (**Debug**) ליד שם הבדיקה.

### 💻 דרך ג': שורת הפקודה (Terminal / PowerShell)
פתחו מסוף בתיקיית השורש של הפרויקט והריצו:
```powershell
dotnet test PlayWithSQLITE.Tests\PlayWithSQLITE.Tests.csproj
```

---

## 5. איך קוראים ומנתחים את התוצאות?

לאחר ההרצה, כל בדיקה תקבל אחד מ-3 סטטוסים:

| סמל / צבע | משמעות | מה עושים? |
| :---: | :--- | :--- |
| ✅ **ירוק (`Passed` / `succeeded`)** | הבדיקה עברה בהצלחה! הקוד שלכם החזיר בדיוק את מה שנדרש. | כל הכבוד — אפשר להמשיך למתודה או לשלב הבא! |
| ⏭️ **צהוב / כחול (`Skipped`)** | הבדיקה עדיין כוללת `Skip = "..."` ולא רצה. | תקין עבור שלבים שטרם הגעתם אליהם. |
| ❌ **אדום (`Failed`)** | נמצאה תקלה בקוד או שהתוצאה לא תואמת לציפייה. | **אל תיבהלו!** לחצו על הבדיקה האדומה וקראו שני שדות חשובים: |

### איך מוצאים את הבאג כשבדיקה נכשלת (אדום)?
1. **`Error Message` (הודעת השגיאה):** אומרת *מה* לא היה תקין. לדוגמה:
   - `Assert.NotEmpty() Failure: Collection was empty` ➡️ המתודה החזירה רשימה ריקה (בדקו האם שכחתם לממש אותה או ש-`SeedSampleDataAsync` לא שמר נתונים).
   - `Assert.Equal() Failure: Expected: 3, Actual: 6` ➡️ למשל ב-`Pagination`, חזרו כל 6 התלמידים במקום 3 (שכחתם `.Take(take)`!).
   - `SQLiteException: no such table: Courses` ➡️ שכחתם לקרוא ל-`CreateTableAsync<Course>()` בתוך `InitDBAsync()`.
2. **`Stack Trace` (עקבות הקריאה):** מציג את **שם הקובץ ומספר השורה המדויק** שבה הבדיקה נכשלה (למשל `DatabaseService.cs:line 84`). לחיצה על השורה ב-Visual Studio / Rider תקפיץ אתכם ישירות אל השורה הבעייתית!
