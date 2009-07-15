using System;

using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

using IC.Core;
using IC.Presenters.ViewInterfaces;

namespace IC.Presenters
{
	public sealed class MenuPresenter : BasePresenter<IMenuView>
	{
		public override void InitView()
		{
			_regionManager.RegisterViewWithRegion(RegionNames.MenuRegion,
									              () => _container.Resolve<IMenuView>());
		}

		private void SaveProject(object sender, EventArgs e)
		{
		}

		public MenuPresenter(IUnityContainer container,
			                 IRegionManager regionManager,
			                 IMenuView view)
			: base(container, regionManager, view)
		{
		}
	}
}
