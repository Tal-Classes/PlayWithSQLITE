using System.Collections.ObjectModel;
using System.Windows.Input;
using PlayWithSQLITE.Models.DTOs;
using PlayWithSQLITE.Services;

namespace PlayWithSQLITE.ViewModels;

/// <summary>
/// ViewModel למסך הדוחות והשאילתות המתקדמות (ReportsPage).
/// האוספים ו-StatusMessage מוכנים ומחוברים ל-XAML.
/// משימתכם בשלב 5 (ראו מדריך 05): יצירת הפקודות (ICommand) והפעלת שאילתות הדוחות מ-IDatabaseService!
/// </summary>
public class ReportsViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;
    private string _statusMessage = string.Empty;

    public ObservableCollection<CourseGradeSummary> CourseSummaries { get; } = new();
    public ObservableCollection<GradeLevelCount> GradeDistributions { get; } = new();

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            if (_statusMessage == value) return;
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    // 💡 שלב 5: שלוש הפקודות המקושרות לכפתורים ב-ReportsPage.xaml
    public ICommand? LoadReportsCommand { get; }
    public ICommand? SeedDataCommand { get; }
    public ICommand? ClearDataCommand { get; }

    public ReportsViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        Title = "דוחות ושאילתות מתקדמות (JOIN & GROUP BY)";

        // 💡 TODO (Student - שלב 5, ראו מדריך 05):
        // אתחלו כאן את שלוש הפקודות:
        // 1. LoadReportsCommand — טוענת את דוחות ה-JOIN וה-GROUP BY (LoadReportsAsync).
        // 2. SeedDataCommand — זורעת נתוני הדגמה למסד ומרעננת את הדוחות (SeedDataAsync).
        // 3. ClearDataCommand — מרוקנת את המסד ואת האוספים (ClearDataAsync).
    }

    public async Task LoadReportsAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // 1. מנעו ריצה כפולה בעזרת IsLoading בתוך try / catch / finally ועדכנו את StatusMessage.
        // 2. אתחלו את המסד ושלפו את סיכומי הקורסים (GetCourseGradeSummariesAsync) לתוך CourseSummaries.
        // 3. שלפו את התפלגות השכבות (GetStudentDistributionByGradeLevelAsync) לתוך GradeDistributions.
        // 4. הציגו ב-StatusMessage כמה דוחות ושכבות נטענו.
        await Task.CompletedTask;
    }

    private async Task SeedDataAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // הפעילו את SeedSampleDataAsync() ב-_databaseService ולאחר מכן טענו מחדש את הדוחות (LoadReportsAsync).
        await Task.CompletedTask;
    }

    private async Task ClearDataAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // הפעילו את ClearAllDataAsync() ב-_databaseService, נקו את האוספים ועדכנו את StatusMessage.
        await Task.CompletedTask;
    }
}
