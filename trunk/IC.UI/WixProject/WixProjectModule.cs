using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Composite.Modularity;

namespace IC.UI.WixProject
{
	public class WixProjectModule : IModule
	{
		private readonly IRegionManager _regionManager;

		#region IModule Members

		public void Initialize()
		{
			throw new System.NotImplementedException();
		}

		#endregion

		public WixProjectModule(IRegionManager regionManager)
		{
			_regionManager = regionManager;
		}
	}
}
