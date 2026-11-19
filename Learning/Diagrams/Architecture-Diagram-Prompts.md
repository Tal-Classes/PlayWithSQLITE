# תרשימי ארכיטקטורה ופרומפטים לתיק הפרויקט (883589)

קובץ זה נוצר אוטומטית מתוך קוד הפרויקט על ידי `Generate-Architecture-Diagrams.py`.
הוא מכיל את **תרשים זרימת המסכים (Screen Flow Diagram)** ואת **תרשים המחלקות (UML Class Diagram)** — כולל **תמונות PNG ויזואליות מוכנות** (`Learning/Images/Screen-Flow.png` ו-`Learning/Images/Architecture-UML.png`) — לשילוב בפרק **"מבנה / ארכיטקטורה של הפרויקט"** בתיק הפרויקט ובספר הלמידה (`MAUI-Learning-Book.pdf`).

---

## 1. תרשים זרימת מסכים (Screen Flow Diagram)

![תרשים זרימת מסכים (Screen Flow)](../Images/Screen-Flow.png)

```mermaid
flowchart TD
  AppShell(["AppShell (נקודת פתיחה וניווט)"])
  AppShell -->|"Shell Tab / Flyout"| StudentsPage
  AppShell -->|"Shell Tab / Flyout"| CoursesPage
  AppShell -->|"Shell Tab / Flyout"| TeachersPage
  AppShell -->|"Shell Tab / Flyout"| ReportsPage
```

### פרומפט שמור ליצירת תרשים זרימת המסכים בעיצוב גרפי:
> צור תרשים זרימת מסכים (Screen Flow Diagram) נקי וברור עבור אפליקציית .NET MAUI. מסך הפתיחה וההיררכיה הראשיים מנוהלים ב-`AppShell`, והמסכים בפרויקט הם: `CoursesPage, MainPage, ReportsPage, StudentsPage, TeachersPage`. הצג חיצים ברורים למעברי הניווט (`Shell.Current.GoToAsync`) בין המסכים לפי מבנה ה-Mermaid שלעיל.

---

## 2. תרשים מחלקות (UML Class Diagram)

![תרשים מחלקות (UML Class Diagram)](../Images/Architecture-UML.png)

```mermaid
classDiagram
  direction TB
  class AppShell {
  }
  class MainPage {
  }
  class Course {
    +int Id
    +string Title
    +int Credits
    +string Description
    +int TeacherId
    +Teacher? Teacher
    +List~Student~ Students
  }
  class CourseGradeSummary {
    +int CourseId
    +string CourseTitle
    +string TeacherName
    +int StudentCount
    +double AverageGrade
    +double MaxGrade
    +double MinGrade
  }
  class GradeLevelCount {
    +int GradeLevel
    +int StudentCount
  }
  class StudentTranscriptItem {
    +int StudentId
    +string StudentName
    +string CourseTitle
    +int Credits
    +string TeacherName
    +double? Grade
    +bool IsPaid
  }
  class Enrollment {
    +int Id
    +int StudentId
    +int CourseId
    +DateTime EnrollmentDate
    +double? Grade
    +bool IsPaid
    +Student? Student
    +Course? Course
  }
  class Student {
    +int Id
    +string FirstName
    +string LastName
    +string Email
    +int GradeLevel
    +DateTime BirthDate
    +List~Course~ Courses
  }
  class Teacher {
    +int Id
    +string FullName
    +string Email
    +string Department
    +List~Course~ Courses
  }
  class DatabaseService {
    +InitializeAsync() Task
    +ClearAllDataAsync() Task
    +SeedSampleDataAsync() Task
    +GetTeachersAsync() Task~List~Teacher~~
    +GetTeacherByIdAsync() Task~Teacher?~
    +SaveTeacherAsync() Task~int~
    +DeleteTeacherAsync() Task~int~
    +GetCoursesAsync() Task~List~Course~~
  }
  class IDatabaseService {
    <<interface>>
  }
  class BaseViewModel {
  }
  class CoursesViewModel {
    +ObservableCollection~Course~ Courses
    +ObservableCollection~Teacher~ Teachers
    +ICommand LoadCoursesCommand
    +ICommand AddCourseCommand
    +ICommand DeleteCourseCommand
    +LoadCoursesAsync() Task
  }
  class ReportsViewModel {
    +ObservableCollection~CourseGradeSummary~ CourseSummaries
    +ObservableCollection~GradeLevelCount~ GradeDistributions
    +ICommand LoadReportsCommand
    +ICommand SeedDataCommand
    +ICommand ClearDataCommand
    +LoadReportsAsync() Task
  }
  class StudentsViewModel {
    +ObservableCollection~Student~ Students
    +ObservableCollection~StudentTranscriptItem~ SelectedStudentTranscript
    +ICommand LoadStudentsCommand
    +ICommand NextPageCommand
    +ICommand PrevPageCommand
    +ICommand AddStudentCommand
    +ICommand DeleteStudentCommand
    +LoadStudentsAsync() Task
  }
  class TeachersViewModel {
    +ObservableCollection~Teacher~ Teachers
    +ICommand LoadTeachersCommand
    +ICommand AddTeacherCommand
    +ICommand DeleteTeacherCommand
    +LoadTeachersAsync() Task
  }
  class CoursesPage {
  }
  class ReportsPage {
  }
  class StudentsPage {
  }
  class TeachersPage {
  }
  class ArchitectureSmokeTests {
    +TestHarness_WhenInitialized_PassesSanityCheck() void
  }
  class DatabaseServiceTests {
    +InitializeAsync() Task
    +DisposeAsync() Task
    +GetTeachersAsync_ReturnsSeededTeachers() Task
    +GetCoursesAsync_WithRelations_LoadsTeacherDetails() Task
    +Pagination_SkipAndTake_ReturnsCorrectSlices() Task
    +GetCourseGradeSummariesAsync_PerformsJoinAndGroupBy() Task
    +GetStudentTranscriptAsync_ReturnsMultiTableJoinedItems() Task
    +GetTotalCreditsForStudentAsync_CalculatesSumOfCredits() Task
  }
  ContentPage <|-- MainPage
  IDatabaseService <|-- DatabaseService
  INotifyPropertyChanged <|-- BaseViewModel
  BaseViewModel <|-- CoursesViewModel
  CoursesViewModel ..> IDatabaseService : DI
  BaseViewModel <|-- ReportsViewModel
  ReportsViewModel ..> IDatabaseService : DI
  BaseViewModel <|-- StudentsViewModel
  StudentsViewModel ..> IDatabaseService : DI
  BaseViewModel <|-- TeachersViewModel
  TeachersViewModel ..> IDatabaseService : DI
  ContentPage <|-- CoursesPage
  CoursesPage ..> CoursesViewModel : DI
  ContentPage <|-- ReportsPage
  ReportsPage ..> ReportsViewModel : DI
  ContentPage <|-- StudentsPage
  StudentsPage ..> StudentsViewModel : DI
  ContentPage <|-- TeachersPage
  TeachersPage ..> TeachersViewModel : DI
```

### פרומפט שמור ליצירת תרשים ה-UML בעיצוב גרפי:
> צור תרשים מחלקות UML תקני (UML Class Diagram) המחולק לשכבות MVVM (`Models`, `Services`, `ViewModels`, `Views`) עבור פרויקט ה-.NET MAUI, כולל קשרי ירושה (`<|--`), הכלה, והזרקת תלויות בבנאי (`..> DI`), בהתבסס על הגדרת ה-Mermaid שלעיל.
