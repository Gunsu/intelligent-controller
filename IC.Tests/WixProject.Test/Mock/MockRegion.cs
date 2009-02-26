using System;
using System.Collections.Generic;

using Microsoft.Practices.Composite.Regions;

namespace IC.UI.WixProject.Mock
{
	public class MockRegion : IRegion
	{
		public List<object> AddedViews = new List<object>();

		public IRegionManager Add(object view)
		{
			AddedViews.Add(view);
			return null;
		}

		public void Remove(object view)
		{
			AddedViews.Remove(view);
		}

		public IViewsCollection Views
		{
			get { throw new NotImplementedException(); }
		}

		public void Activate(object view)
		{
			SelectedItem = view;
		}

		public void Deactivate(object view)
		{
			throw new NotImplementedException();
		}

		public IRegionManager Add(object view, string viewName)
		{
			Add(view);
			return null;
		}

		public object GetView(string viewName)
		{
			return null;
		}

		public IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
		{
			throw new NotImplementedException();
		}

		public IRegionManager RegionManager
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public object SelectedItem { get; set; }

		public IViewsCollection ActiveViews
		{
			get { throw new NotImplementedException(); }
		}
	}
}
