using System;
using System.Collections;
using Microsoft.Practices.Composite.Regions;
using System.Collections.Generic;

namespace IC.Modules.Menu.Tests.Mocks
{
	public sealed class MockRegionCollection : IRegionCollection
	{
		private Dictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

		public IRegion this[string regionName]
		{
			get { return _regions[regionName]; }
		}


		public void Add(IRegion region)
		{
			_regions[region.Name] = region;
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region IRegionCollection common members

		public bool ContainsRegionWithName(string regionName)
		{
			throw new NotImplementedException();
		}

		public bool Remove(string regionName)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<IRegion> common members

		public IEnumerator<IRegion> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
