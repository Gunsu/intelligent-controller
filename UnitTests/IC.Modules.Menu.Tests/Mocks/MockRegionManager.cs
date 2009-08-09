using System;
using Microsoft.Practices.Composite.Regions;

namespace IC.Modules.Menu.Tests.Mocks
{
	public sealed class MockRegionManager : IRegionManager
	{
		private readonly IRegionCollection _regions = new MockRegionCollection();

		public IRegionCollection Regions
		{
			get { return _regions; }
		}

		#region IRegionManager common members

		public IRegionManager CreateRegionManager()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
