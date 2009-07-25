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
			base.OnStartup(e);
			Bootstrapper bootstrapper = new Bootstrapper();
			bootstrapper.Run();
		}
	}
}
