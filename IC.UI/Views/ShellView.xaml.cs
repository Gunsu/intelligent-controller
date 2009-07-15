using System.Windows;

using IC.Presenters.ViewInterfaces;

namespace IC.UI
{
	/// <summary>
	/// Логика взаимодействия для ShellView.xaml
	/// </summary>
	public partial class ShellView : Window, IShellView
	{
		public ShellView()
		{
			InitializeComponent();
		}

		public void ShowView()
		{
			this.Show();
		}
	}
}
