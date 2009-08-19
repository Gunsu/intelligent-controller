using System;
using System.Windows;
using Microsoft.Practices.Unity;

using IC.UI.Infrastructure.Interfaces.Menu;
using IC.UI.Infrastructure.Interfaces.ProjectExplorer;
using IC.UI.Infrastructure.Interfaces.Manager;
using IC.UI.Infrastructure.Interfaces.Toolbox;

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
		private readonly IManagerPresentationModel _managerPresentationModel;

		public MainWindow(IMenuPresentationModel menuPresentationModel,
			              IProjectExplorerPresentationModel projectExplorerPresentationModel,
			              IToolboxPresentationModel toolboxPresentationModel,
						  IManagerPresentationModel managerPresentationModel)
		{
			_menuPresentationModel = menuPresentationModel;
			_projectExplorerPresentationModel = projectExplorerPresentationModel;
			_toolboxPresentationModel = toolboxPresentationModel;
			_managerPresentationModel = managerPresentationModel;

			InitializeComponent();
		}

		private void AttachModels()
		{
			menuView.Model = _menuPresentationModel;
			projectExplorerView.Model = _projectExplorerPresentationModel;
			toolboxView.Model = _toolboxPresentationModel;

			managerView.Model = _managerPresentationModel;
		}

		private void MainWindow_Initialized(object sender, EventArgs e)
		{
			AttachModels();
		}
	}

	public interface IMainWindow
	{
		void Show();
	}
}
