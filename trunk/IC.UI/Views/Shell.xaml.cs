using System.Windows;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для ShellView.xaml
	/// </summary>
	public partial class Shell : Window, IShellView
	{
		public Shell()
		{
			InitializeComponent();
		}

		public void ShowView()
		{
			Show();
		}
	}

	public sealed class ShellPresenter
	{
		private readonly IShellView _view;

		public IShellView View
		{
			get { return _view; }
		}

		public ShellPresenter(IShellView view)
		{
			_view = view;
		}
	}

	public interface IShellView
	{
		void ShowView();
	}
}
