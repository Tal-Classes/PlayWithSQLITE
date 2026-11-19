using PlayWithSQLITE.ViewModels;

namespace PlayWithSQLITE.Views;

public partial class TeachersPage : ContentPage
{
    private readonly TeachersViewModel _viewModel;

    public TeachersPage(TeachersViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadTeachersAsync();
    }
}
