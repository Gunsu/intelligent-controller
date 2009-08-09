using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

namespace IC.Modules.Menu.Tests.Mocks
{
	internal class TestableMenuModule : MenuModule
	{
		public TestableMenuModule(IUnityContainer container, IRegionManager regionManager)
			: base(container, regionManager)
		{
		}

		public void InvokeRegisterView()
		{
			base.RegisterViewsAndServices();
		}
	}
}
