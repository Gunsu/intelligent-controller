using IC.Modules.Menu.Interfaces.PresentationModels;
using IC.Modules.Menu.Interfaces.Views;
using IC.Modules.Menu.PresentationModels;
using IC.Modules.Menu.Views;
using IC.UI.Infrastructure;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.Modules.Menu
{
	[Validate]
	public class MenuModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _regionManager;

		#region IModule members

		public void Initialize()
		{
			RegisterViewsAndServices();

			_regionManager.RegisterViewWithRegion(RegionNames.MenuRegion,
			                                      () => _container.Resolve<IMenuPresentationModel>().View);
		}

		#endregion

		protected void RegisterViewsAndServices()
		{
			_container.RegisterType<IMenuPresentationModel, MenuPresentationModel>();
			_container.RegisterType<IMenuView, MenuView>();	
		}

		public MenuModule([NotNull] IUnityContainer container, IRegionManager regionManager)
		{
			_container = container;
			_regionManager = regionManager;
		}
	}
}
