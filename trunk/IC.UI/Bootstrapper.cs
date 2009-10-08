using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using IC.UI.Windows;
using IC.UI.Infrastructure.Interfaces.Menu;
using IC.UI.Infrastructure.Interfaces.ProjectExplorer;
using IC.UI.Infrastructure.Interfaces.Toolbox;
using IC.PresentationModels;
using IC.CoreInterfaces.Processes;
using IC.Core.Processes;
using IC.UI.Infrastructure.Interfaces.Manager;
using IC.UI.Infrastructure.Interfaces.Windows;
using IC.UI.Infrastructure.Interfaces.Schema;

namespace IC.UI
{
	public sealed class Bootstrapper
	{
		private readonly IUnityContainer _container;

		private static ContainerControlledLifetimeManager IsSingleton
		{
			get { return new ContainerControlledLifetimeManager(); }
		}

		public IUnityContainer Container
		{
			get { return _container; }
		}


		private void RegisterWindows()
		{
			_container.RegisterType<IMainWindow, MainWindow>(IsSingleton);
			_container.RegisterType<ICreateProjectWindow, CreateProjectWindow>();
			_container.RegisterType<ICreateSchemaWindow, CreateSchemaWindow>();
		}

		private void RegisterPresentationModels()
		{
			_container.RegisterType<IMenuPresentationModel, MenuPresentationModel>();
			_container.RegisterType<IProjectExplorerPresentationModel, ProjectExplorerPresentationModel>();
			_container.RegisterType<IToolboxPresentationModel, ToolboxPresentationModel>();
			_container.RegisterType<ISchemaPresentationModel, SchemaPresentationModel>();
			_container.RegisterType<IManagerPresentationModel, ManagerPresentationModel>();
		}

		private void RegisterCoreObjectsAndProcesses()
		{
			var blockTypesProcessesParams = new InjectionMember[] {new InjectionConstructor("BlockTypes.xml")};
			_container.RegisterType<IBlockTypesProcesses, BlockTypesProcesses>(IsSingleton,
				                                                               blockTypesProcessesParams);
			_container.RegisterType<IProjectProcesses, ProjectProcesses>(IsSingleton);
			_container.RegisterType<ISchemaProcesses, SchemaProcesses>(IsSingleton);
		}


		public void Run()
		{
			var mainWindow = _container.Resolve<IMainWindow>();
			mainWindow.Show();
		}


		public Bootstrapper()
		{
			_container = new UnityContainer();
			_container.RegisterType<IEventAggregator, EventAggregator>(IsSingleton);
			RegisterCoreObjectsAndProcesses();
			RegisterPresentationModels();
			RegisterWindows();
		}
	}
}
