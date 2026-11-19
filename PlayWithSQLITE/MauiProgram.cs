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

			// =====================================================================
			// 💡 שלב 5 — רישום שירות מסד הנתונים ב-DI Container (ראו מדריך 05):
			// =====================================================================
			// כדי שה-ViewModels יקבלו אוטומטית את שירות מסד הנתונים בבנאי שלהם,
			// עלינו לרשום את הממשק IDatabaseService מול המימוש הקונקרטי DatabaseService בתוך builder.Services.
			// חשבו: איזה מחזור חיים (Lifetime) מתאים לשירות מסד נתונים — AddSingleton או AddTransient? ומדוע?
			// TODO (Student - שלב 5): הוסיפו כאן את שורת הרישום של IDatabaseService מול DatabaseService.

			// ViewModels (מופע חדש בכל בקשה — Transient)
			builder.Services.AddTransient<StudentsViewModel>();
			builder.Services.AddTransient<CoursesViewModel>();
			builder.Services.AddTransient<TeachersViewModel>();
			builder.Services.AddTransient<ReportsViewModel>();

			// Pages (מופע חדש בכל ניווט — Transient)
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
