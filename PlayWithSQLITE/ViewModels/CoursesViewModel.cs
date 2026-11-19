using System.Collections.ObjectModel;
using System.Windows.Input;
using PlayWithSQLITE.Models;
using PlayWithSQLITE.Services;

namespace PlayWithSQLITE.ViewModels;

/// <summary>
/// ViewModel לניהול מסך הקורסים (CoursesPage).
/// תשתית המאפיינים והאוספים מוכנה ומקושרת ל-XAML.
/// משימתכם בשלב 5 (ראו מדריך 05): יצירת הפקודות (ICommand) וחיבורן ל-IDatabaseService!
/// </summary>
public class CoursesViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;
    private string _newCourseTitle = string.Empty;
    private int _newCourseCredits = 3;
    private string _newCourseDescription = string.Empty;
    private Teacher? _selectedTeacher;
    private Course? _selectedCourse;

    public ObservableCollection<Course> Courses { get; } = new();
    public ObservableCollection<Teacher> Teachers { get; } = new();

    public string NewCourseTitle
    {
        get => _newCourseTitle;
        set
        {
            if (_newCourseTitle == value) return;
            _newCourseTitle = value;
            OnPropertyChanged();
            (AddCourseCommand as Command)?.ChangeCanExecute();
        }
    }

    public int NewCourseCredits
    {
        get => _newCourseCredits;
        set
        {
            if (_newCourseCredits == value) return;
            _newCourseCredits = value;
            OnPropertyChanged();
        }
    }

    public string NewCourseDescription
    {
        get => _newCourseDescription;
        set
        {
            if (_newCourseDescription == value) return;
            _newCourseDescription = value;
            OnPropertyChanged();
        }
    }

    public Teacher? SelectedTeacher
    {
        get => _selectedTeacher;
        set
        {
            if (_selectedTeacher == value) return;
            _selectedTeacher = value;
            OnPropertyChanged();
            (AddCourseCommand as Command)?.ChangeCanExecute();
        }
    }

    public Course? SelectedCourse
    {
        get => _selectedCourse;
        set
        {
            if (_selectedCourse == value) return;
            _selectedCourse = value;
            OnPropertyChanged();
        }
    }

    // 💡 שלב 5: מאפייני הפקודות המקושרים לכפתורים ב-CoursesPage.xaml
    public ICommand? LoadCoursesCommand { get; }
    public ICommand? AddCourseCommand { get; }
    public ICommand? DeleteCourseCommand { get; }

    public CoursesViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        Title = "ניהול קורסים ומורים מקושרים";

        // 💡 TODO (Student - שלב 5, ראו מדריך 05):
        // אתחלו כאן את שלוש הפקודות:
        // 1. LoadCoursesCommand — תפעיל אסינכרונית את LoadCoursesAsync().
        // 2. AddCourseCommand — תפעיל אסינכרונית את AddCourseAsync() עם תנאי CanExecute (CanAddCourse).
        // 3. DeleteCourseCommand — פקודה המקבלת Course ותפעיל את DeleteCourseAsync(course).
    }

    private bool CanAddCourse()
    {
        return !string.IsNullOrWhiteSpace(NewCourseTitle) && SelectedTeacher != null;
    }

    public async Task LoadCoursesAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // 1. מנעו ריצה כפולה (IsLoading בתוך try / finally) וודאו אתחול המסד.
        // 2. טענו את רשימת המורים לתוך אוסף Teachers (כדי שה-Picker במסך יציג את המורים לבחירה).
        // 3. טענו את רשימת הקורסים עם includeRelations: true לתוך אוסף Courses (כדי להציג את שם המורה לכל קורס!).
        await Task.CompletedTask;
    }

    private async Task AddCourseAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // 1. בדקו את CanAddCourse().
        // 2. צרו אובייקט Course חדש וקשרו אליו את המפתח הזר TeacherId מתוך SelectedTeacher!
        // 3. שמרו במסד דרך _databaseService, נקו את שדות הקלט ורעננו את הרשימה.
        await Task.CompletedTask;
    }

    private async Task DeleteCourseAsync(Course? course)
    {
        // 💡 TODO (Student - שלב 5):
        // מחקו את הקורס דרך _databaseService ורעננו את רשימת הקורסים.
        await Task.CompletedTask;
    }
}
