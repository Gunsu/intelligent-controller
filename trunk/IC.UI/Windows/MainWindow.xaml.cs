using System;
using System.Windows;

using IC.UI.Infrastructure.Interfaces.Menu;
using IC.UI.Infrastructure.Interfaces.ProjectExplorer;
using IC.UI.Infrastructure.Interfaces.Manager;
using IC.UI.Infrastructure.Interfaces.Toolbox;
using IC.UI.Infrastructure.Interfaces.Schema;

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
		private readonly ISchemaPresentationModel _schemaPresentationModel;
		private readonly IManagerPresentationModel _managerPresentationModel;

		public MainWindow(IMenuPresentationModel menuPresentationModel,
			              IProjectExplorerPresentationModel projectExplorerPresentationModel,
			              IToolboxPresentationModel toolboxPresentationModel,
						  ISchemaPresentationModel schemaPresentationModel,
						  IManagerPresentationModel managerPresentationModel)
		{
			_menuPresentationModel = menuPresentationModel;
			_projectExplorerPresentationModel = projectExplorerPresentationModel;
			_toolboxPresentationModel = toolboxPresentationModel;
			_schemaPresentationModel = schemaPresentationModel;
			_managerPresentationModel = managerPresentationModel;

			InitializeComponent();
		}

		private void AttachModels()
		{
			menuView.Model = _menuPresentationModel;
			projectExplorerView.Model = _projectExplorerPresentationModel;
			toolboxView.Model = _toolboxPresentationModel;
			schemaView.Model = _schemaPresentationModel;
			schemaView.MyDesigner.EventAggregator = _schemaPresentationModel.EventAggregator;
			schemaView.MyDesigner.SubscribeToEvents();
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
