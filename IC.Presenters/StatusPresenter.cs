using System;

using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

using IC.Core;
using IC.Presenters.ViewInterfaces;

namespace IC.Presenters.ViewInterfaces
{
	public sealed class StatusPresenter : BasePresenter<IStatusView>
	{
		public override void InitView()
		{
			_regionManager.RegisterViewWithRegion(RegionNames.StatusRegion,
												  () => _container.Resolve<IStatusView>());
		}

		public StatusPresenter(IUnityContainer container,
							   IRegionManager regionManager,
							   IStatusView view)
			: base(container, regionManager, view)
		{
		}
	}
}
