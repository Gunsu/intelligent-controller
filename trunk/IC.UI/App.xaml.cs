using System.Windows;

namespace IC.UI
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			var bootstrapper = new Bootstrapper();
			bootstrapper.Run();
		}
	}
}
