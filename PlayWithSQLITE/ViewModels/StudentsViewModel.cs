using System.Collections.ObjectModel;
using System.Windows.Input;
using PlayWithSQLITE.Models;
using PlayWithSQLITE.Models.DTOs;
using PlayWithSQLITE.Services;

namespace PlayWithSQLITE.ViewModels;

/// <summary>
/// ViewModel לניהול מסך התלמידים, דפדוף (Pagination) וגיליון הציונים האישי (StudentsPage).
/// תשתית המאפיינים, חישובי העמודים והאוספים מוכנה ומקושרת ל-XAML.
/// משימתכם בשלב 5 (ראו מדריך 05): יצירת הפקודות (ICommand) וחיבורן ל-IDatabaseService!
/// </summary>
public class StudentsViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;
    private int _currentPage = 1;
    private int _pageSize = 5;
    private int _totalStudents;
    private Student? _selectedStudent;
    private int _studentTotalCredits;
    private string _newFirstName = string.Empty;
    private string _newLastName = string.Empty;
    private string _newEmail = string.Empty;
    private int _newGradeLevel = 10;

    public ObservableCollection<Student> Students { get; } = new();
    public ObservableCollection<StudentTranscriptItem> SelectedStudentTranscript { get; } = new();

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage == value) return;
            _currentPage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PageInfo));
            (NextPageCommand as Command)?.ChangeCanExecute();
            (PrevPageCommand as Command)?.ChangeCanExecute();
        }
    }

    public int TotalStudents
    {
        get => _totalStudents;
        set
        {
            if (_totalStudents == value) return;
            _totalStudents = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PageInfo));
            OnPropertyChanged(nameof(TotalPages));
            (NextPageCommand as Command)?.ChangeCanExecute();
            (PrevPageCommand as Command)?.ChangeCanExecute();
        }
    }

    public int TotalPages => Math.Max(1, (int)Math.Ceiling((double)TotalStudents / _pageSize));

    public string PageInfo => $"עמוד {CurrentPage} מתוך {TotalPages} (סה\"כ {TotalStudents} תלמידים)";

    public Student? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            if (_selectedStudent == value) return;
            _selectedStudent = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasSelectedStudent));
            if (_selectedStudent != null)
            {
                _ = LoadTranscriptForStudentAsync(_selectedStudent.Id);
            }
        }
    }

    public bool HasSelectedStudent => SelectedStudent != null;

    public int StudentTotalCredits
    {
        get => _studentTotalCredits;
        set
        {
            if (_studentTotalCredits == value) return;
            _studentTotalCredits = value;
            OnPropertyChanged();
        }
    }

    public string NewFirstName
    {
        get => _newFirstName;
        set
        {
            if (_newFirstName == value) return;
            _newFirstName = value;
            OnPropertyChanged();
            (AddStudentCommand as Command)?.ChangeCanExecute();
        }
    }

    public string NewLastName
    {
        get => _newLastName;
        set
        {
            if (_newLastName == value) return;
            _newLastName = value;
            OnPropertyChanged();
            (AddStudentCommand as Command)?.ChangeCanExecute();
        }
    }

    public string NewEmail
    {
        get => _newEmail;
        set
        {
            if (_newEmail == value) return;
            _newEmail = value;
            OnPropertyChanged();
            (AddStudentCommand as Command)?.ChangeCanExecute();
        }
    }

    public int NewGradeLevel
    {
        get => _newGradeLevel;
        set
        {
            if (_newGradeLevel == value) return;
            _newGradeLevel = value;
            OnPropertyChanged();
        }
    }

    // 💡 שלב 5: חמש הפקודות המקושרות לכפתורים ב-StudentsPage.xaml
    public ICommand? LoadStudentsCommand { get; }
    public ICommand? NextPageCommand { get; }
    public ICommand? PrevPageCommand { get; }
    public ICommand? AddStudentCommand { get; }
    public ICommand? DeleteStudentCommand { get; }

    public StudentsViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        Title = "ניהול תלמידים ודפדוף";

        // 💡 TODO (Student - שלב 5, ראו מדריך 05):
        // אתחלו כאן את חמש הפקודות:
        // 1. LoadStudentsCommand — טוענת את עמוד התלמידים הנוכחי (LoadStudentsAsync).
        // 2. NextPageCommand — מתקדמת לעמוד הבא (NextPageAsync), פעילה רק כאשר CurrentPage < TotalPages!
        // 3. PrevPageCommand — חוזרת לעמוד הקודם (PrevPageAsync), פעילה רק כאשר CurrentPage > 1!
        // 4. AddStudentCommand — מוסיפה תלמיד (AddStudentAsync), פעילה רק כאשר CanAddStudent() מחזירה true!
        // 5. DeleteStudentCommand — פקודה המקבלת Student ומוחקת אותו (DeleteStudentAsync).
    }

    private bool CanAddStudent()
    {
        return !string.IsNullOrWhiteSpace(NewFirstName) &&
               !string.IsNullOrWhiteSpace(NewLastName) &&
               !string.IsNullOrWhiteSpace(NewEmail);
    }

    public async Task LoadStudentsAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // 1. הגנו בעזרת IsLoading בתוך try / finally (אתחול המסד מתבצע אוטומטית בתוך השירות!).
        // 2. עדכנו את TotalStudents בעזרת שאילתת הספירה (GetStudentsCountAsync).
        // 3. חשבו את skip לפי הנוסחה (CurrentPage - 1) * _pageSize ושלפו רק את העמוד הנוכחי (GetStudentsAsync(skip, _pageSize)).
        // 4. רעננו את אוסף Students (Clear + Add), ואם SelectedStudent != null רעננו גם את גיליון הציונים שלו.
        await Task.CompletedTask;
    }

    private async Task NextPageAsync()
    {
        // 💡 TODO (Student - שלב 5): קדמו את CurrentPage (אם ניתן) וטענו מחדש את העמוד.
        await Task.CompletedTask;
    }

    private async Task PrevPageAsync()
    {
        // 💡 TODO (Student - שלב 5): הפחיתו את CurrentPage (אם ניתן) וטענו מחדש את העמוד.
        await Task.CompletedTask;
    }

    private async Task AddStudentAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // 1. בדקו CanAddStudent(), צרו Student חדש ושמרו אותו במסד דרך _databaseService.
        // 2. אפסו את שדות הטופס ורעננו את הרשימה (LoadStudentsAsync).
        await Task.CompletedTask;
    }

    private async Task DeleteStudentAsync(Student? student)
    {
        // 💡 TODO (Student - שלב 5):
        // מחקו את התלמיד דרך _databaseService; אם הוא היה התלמיד הנבחר — נקו את הבחירה ואת הגיליון, ורעננו את הרשימה.
        await Task.CompletedTask;
    }

    private async Task LoadTranscriptForStudentAsync(int studentId)
    {
        // 💡 TODO (Student - שלב 5):
        // 1. שלפו את גיליון הציונים האישי (GetStudentTranscriptAsync) ומלאו את SelectedStudentTranscript.
        // 2. חשבו את סך נקודות הזכות (GetTotalCreditsForStudentAsync) ועדכנו את StudentTotalCredits.
        await Task.CompletedTask;
    }
}
