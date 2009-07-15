using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Regions;

using IC.Presenters.ViewInterfaces;

namespace IC.Presenters
{
	public abstract class BasePresenter<ViewInterface>
		where ViewInterface : IBaseView
	{
		protected readonly IUnityContainer _container;
		protected readonly IRegionManager _regionManager;
		protected readonly ViewInterface _view;

		public BasePresenter(IUnityContainer container,
			                 IRegionManager regionManager,
			                 ViewInterface view)
		{
			_container = container;
			_regionManager = regionManager;
			_view = view;
		}

		public abstract void InitView();
	}
}
