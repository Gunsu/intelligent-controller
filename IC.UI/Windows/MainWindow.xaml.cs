using System;
using System.Windows;
using IC.UI.Infrastructure.Interfaces.Menu;
using IC.UI.Infrastructure.Interfaces.ProjectExplorer;
using IC.UI.Infrastructure.Interfaces.Toolbox;
using Microsoft.Practices.Unity;


namespace IC.UI.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IMainWindow
	{
		private readonly IMenuPresentationModel _menuPresentationModel;
		private readonly IProjectExplorerPresentationModel _projectExplorerPresentationModel;
		private readonly IToolboxPresentationModel _toolboxPresentationModel;

		public MainWindow(IMenuPresentationModel menuPresentationModel,
			              IProjectExplorerPresentationModel projectExplorerPresentationModel,
			              IToolboxPresentationModel toolboxPresentationModel)
		{
			_menuPresentationModel = menuPresentationModel;
			_projectExplorerPresentationModel = projectExplorerPresentationModel;
			_toolboxPresentationModel = toolboxPresentationModel;

			InitializeComponent();
		}

		protected override void OnActivated(EventArgs e)
		{
			_menuView.Model = _menuPresentationModel;
			_projectExplorerView.Model = _projectExplorerPresentationModel;
			_toolboxView.Model = _toolboxPresentationModel;
		}
	}

	public interface IMainWindow
	{
		void Show();
	}
}
