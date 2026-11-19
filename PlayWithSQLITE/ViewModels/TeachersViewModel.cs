using System.Collections.ObjectModel;
using System.Windows.Input;
using PlayWithSQLITE.Models;
using PlayWithSQLITE.Services;

namespace PlayWithSQLITE.ViewModels;

/// <summary>
/// ViewModel לניהול מסך המורים (TeachersPage).
/// תשתית המאפיינים (Properties) והאוסף (ObservableCollection) מוכנה ומחוברת ל-XAML.
/// משימתכם בשלב 5 (ראו מדריך 05): יצירת הפקודות (ICommand) והפעלת שירות מסד הנתונים!
/// </summary>
public class TeachersViewModel : BaseViewModel
{
    private readonly IDatabaseService _databaseService;
    private string _newFullName = string.Empty;
    private string _newEmail = string.Empty;
    private string _newDepartment = "מדעי המחשב";
    private Teacher? _selectedTeacher;

    public ObservableCollection<Teacher> Teachers { get; } = new();

    public string NewFullName
    {
        get => _newFullName;
        set
        {
            if (_newFullName == value) return;
            _newFullName = value;
            OnPropertyChanged();
            // 💡 שלב 5: לאחר יצירת AddTeacherCommand, כיצד נעדכן את הכפתור במסך שהתנאי CanExecute השתנה?
            (AddTeacherCommand as Command)?.ChangeCanExecute();
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
            (AddTeacherCommand as Command)?.ChangeCanExecute();
        }
    }

    public string NewDepartment
    {
        get => _newDepartment;
        set
        {
            if (_newDepartment == value) return;
            _newDepartment = value;
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
        }
    }

    // 💡 שלב 5: מאפייני הפקודות שכבר מקושרים (Binding) לכפתורים ב-TeachersPage.xaml
    public ICommand? LoadTeachersCommand { get; }
    public ICommand? AddTeacherCommand { get; }
    public ICommand? DeleteTeacherCommand { get; }

    public TeachersViewModel(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
        Title = "ניהול סגל מורים";

        // 💡 TODO (Student - שלב 5, ראו מדריך 05):
        // אתחלו כאן את שלוש הפקודות באמצעות new Command(...) ו-new Command<Teacher>(...):
        // 1. LoadTeachersCommand — תפעיל אסינכרונית את LoadTeachersAsync().
        // 2. AddTeacherCommand — תפעיל אסינכרונית את AddTeacherAsync(), ותקבל תנאי CanExecute (CanAddTeacher) כדי שהכפתור יהיה פעיל רק כשהשדות מלאים!
        // 3. DeleteTeacherCommand — פקודה גנרית המקבלת פרמטר מסוג Teacher ותפעיל את DeleteTeacherAsync(teacher).
    }

    private bool CanAddTeacher()
    {
        return !string.IsNullOrWhiteSpace(NewFullName) && !string.IsNullOrWhiteSpace(NewEmail);
    }

    public async Task LoadTeachersAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // 1. מנעו ריצה כפולה בעזרת בדיקת IsLoading והדלקתו בתוך בלוק try / finally.
        // 2. ודאו אתחול של המסד (InitializeAsync), שלפו את כל המורים דרך _databaseService.
        // 3. נקו את אוסף Teachers והוסיפו כל מורה (טיפ: שלפו כל מורה עם includeCourses: true כדי להציג קשר 1:N!).
        await Task.CompletedTask;
    }

    private async Task AddTeacherAsync()
    {
        // 💡 TODO (Student - שלב 5):
        // 1. בדקו את CanAddTeacher().
        // 2. צרו אובייקט Teacher חדש מתוך שדות הטופס (NewFullName, NewEmail, NewDepartment).
        // 3. שמרו אותו במסד באמצעות _databaseService, נקו את שדות הקלט ורעננו את הרשימה (LoadTeachersAsync).
        await Task.CompletedTask;
    }

    private async Task DeleteTeacherAsync(Teacher? teacher)
    {
        // 💡 TODO (Student - שלב 5):
        // 1. ודאו ש-teacher אינו null.
        // 2. מחקו אותו דרך _databaseService ורעננו את הרשימה במסך.
        await Task.CompletedTask;
    }
}
