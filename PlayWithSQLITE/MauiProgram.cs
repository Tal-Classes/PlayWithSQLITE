#if DEBUG
using Microsoft.Maui.DevFlow.Agent;
#endif
using Microsoft.Extensions.Logging;
using PlayWithSQLITE.Services;
using PlayWithSQLITE.ViewModels;
using PlayWithSQLITE.Views;

namespace PlayWithSQLITE
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			// שירותי מסד נתונים
			builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

			// ViewModels
			builder.Services.AddTransient<StudentsViewModel>();
			builder.Services.AddTransient<CoursesViewModel>();
			builder.Services.AddTransient<TeachersViewModel>();
			builder.Services.AddTransient<ReportsViewModel>();

			// Pages
			builder.Services.AddTransient<StudentsPage>();
			builder.Services.AddTransient<CoursesPage>();
			builder.Services.AddTransient<TeachersPage>();
			builder.Services.AddTransient<ReportsPage>();

#if DEBUG
			builder.Logging.AddDebug();
#endif

#if DEBUG
			builder.AddMauiDevFlowAgent();
#endif
			return builder.Build();
		}
	}
}
