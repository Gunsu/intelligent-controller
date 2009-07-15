using System;

using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

using IC.Core;
using IC.Presenters.ViewInterfaces;

namespace IC.Presenters.ViewInterfaces
{
	public sealed class ToolboxPresenter : BasePresenter<IToolboxView>
	{
		public override void InitView()
		{
			_regionManager.RegisterViewWithRegion(RegionNames.ToolboxRegion,
												  () => _container.Resolve<IToolboxView>());
		}

		public ToolboxPresenter(IUnityContainer container,
								IRegionManager regionManager,
								IToolboxView view)
			: base(container, regionManager, view)
		{
		}
	}
}
