using System.Windows;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using IC.UI.Windows;
using IC.UI.Infrastructure.Interfaces.Menu;
using IC.UI.Views;
using IC.UI.Infrastructure.Interfaces.ProjectExplorer;
using IC.UI.Infrastructure.Interfaces.Toolbox;
using IC.PresentationModels;
using IC.CoreInterfaces.Processes;
using IC.Core.Processes;

namespace IC.UI
{
	public sealed class Bootstrapper
	{
		private readonly IUnityContainer _container;


		public IUnityContainer Container
		{
			get { return _container; }
		}


		private void RegisterWindows()
		{
			_container.RegisterType<IMainWindow, MainWindow>();
		}

		private void RegisterViews()
		{
			_container.RegisterType<IMenuView, MenuView>();
			_container.RegisterType<IProjectExplorerView, ProjectExplorerView>();
			_container.RegisterType<IToolboxView, ToolboxView>();
		}

		private void RegisterPresentationModels()
		{
			_container.RegisterType<IMenuPresentationModel, MenuPresentationModel>();
			_container.RegisterType<IProjectExplorerPresentationModel, ProjectExplorerPresentationModel>();
			_container.RegisterType<IToolboxPresentationModel, ToolboxPresentationModel>();
		}

		private void RegisterCoreObjectsAndProcesses()
		{
			var blockTypesProcessesParams = new InjectionMember[] {new InjectionConstructor("BlockTypes.xml")};
			_container.RegisterType<IBlockTypesProcesses, BlockTypesProcesses>(blockTypesProcessesParams);
		}


		public void Run()
		{
			var mainWindow = _container.Resolve<IMainWindow>();
			mainWindow.Show();
		}


		public Bootstrapper()
		{
			_container = new UnityContainer();
            _container.RegisterType<IEventAggregator, EventAggregator>();
			RegisterCoreObjectsAndProcesses();
			RegisterViews();
			RegisterPresentationModels();
			RegisterWindows();
		}
	}
}
