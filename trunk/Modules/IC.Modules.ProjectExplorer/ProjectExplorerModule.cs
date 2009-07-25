using IC.Modules.ProjectExplorer.Interfaces.PresentationModels;
using IC.Modules.ProjectExplorer.Interfaces.Views;
using IC.Modules.ProjectExplorer.PresentationModels;
using IC.Modules.ProjectExplorer.Views;
using IC.UI.Infrastructure;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace IC.Modules.ProjectExplorer
{
	public sealed class ProjectExplorerModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _regionManager;

		public void Initialize()
		{
			_container.RegisterType<IProjectExplorerPresentationModel, ProjectExplorerPresentationModel>();
			_container.RegisterType<IProjectExplorerView, ProjectExplorerView>();

			_regionManager.RegisterViewWithRegion(RegionNames.ProjectExplorerRegion,
			                                      () => _container.Resolve<IProjectExplorerPresentationModel>().View);
		}

		public ProjectExplorerModule(IUnityContainer container, IRegionManager regionManager)
		{
			_container = container;
			_regionManager = regionManager;
		}
	}
}
