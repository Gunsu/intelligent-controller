using System;
using System.Collections.Generic;

using Microsoft.Practices.Composite.Regions;

namespace IC.UI.WixProject.Mock
{
	public class MockRegionManager : IRegionManager
	{
		private readonly Dictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

		public IDictionary<string, IRegion> Regions
		{
			get { return _regions; }
		}

		public void AttachNewRegion(object regionTarget, string regionName)
		{
			throw new NotImplementedException();
		}

		public IRegionManager CreateRegionManager()
		{
			throw new NotImplementedException();
		}
	}
}
