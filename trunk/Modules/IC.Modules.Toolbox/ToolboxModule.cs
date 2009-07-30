using IC.Modules.Toolbox.Interfaces.PresentationModels;
using IC.Modules.Toolbox.Interfaces.Services;
using IC.Modules.Toolbox.Interfaces.Views;
using IC.Modules.Toolbox.PresentationModels;
using IC.Modules.Toolbox.Services;
using IC.Modules.Toolbox.Views;
using IC.UI.Infrastructure;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.Modules.Toolbox
{
	[Validate]
	public sealed class ToolboxModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _regionManager;

		#region IModule members

		public void Initialize()
		{
			_container.RegisterType<IToolboxPresentationModel, ToolboxPresentationModel>();
			_container.RegisterType<IToolboxView, ToolboxView>();
			_container.RegisterType<IBlockTypesService, BlockTypesService>();

			_regionManager.RegisterViewWithRegion(RegionNames.ToolboxRegion,
				                                  () => _container.Resolve<IToolboxPresentationModel>().View);
		}

		#endregion

		public ToolboxModule([NotNull] IUnityContainer container, [NotNull] IRegionManager regionManager)
		{
			_container = container;
			_regionManager = regionManager;
		}
	}
}
