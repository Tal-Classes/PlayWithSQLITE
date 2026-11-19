using PlayWithSQLITE.ViewModels;

namespace PlayWithSQLITE.Views;

public partial class StudentsPage : ContentPage
{
    private readonly StudentsViewModel _viewModel;

    public StudentsPage(StudentsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadStudentsAsync();
    }
}
