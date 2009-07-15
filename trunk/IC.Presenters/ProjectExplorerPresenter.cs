using System;

using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

using IC.Core;
using IC.Presenters.ViewInterfaces;

namespace IC.Presenters
{
	public sealed class ProjectExplorerPresenter : BasePresenter<IProjectExplorerView>
	{
		public override void InitView()
		{
			_regionManager.AddToRegion(RegionNames.ProjectExplorerRegion,
												        _view);
			_view.InitWith(null);
		}

		public ProjectExplorerPresenter(IUnityContainer container,
										              IRegionManager regionManager,
										              IProjectExplorerView view)
			: base(container, regionManager, view)
		{
		}
	}
}
