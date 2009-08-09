using IC.Modules.Menu.Interfaces.PresentationModels;
using IC.Modules.Menu.Interfaces.Views;
using IC.Modules.Menu.PresentationModels;
using IC.Modules.Menu.Tests.Mocks;
using IC.Modules.Menu.Views;
using NUnit.Framework;

namespace IC.Modules.Menu.Tests
{
	[TestFixture]
	public class MenuModuleFixture
	{
		[Test]
		public void Register()
		{
			var container = new MockUnityContainer();
			var module = new TestableMenuModule(container, new MockRegionManager());

			module.InvokeRegisterView();

			Assert.AreEqual(typeof(MenuView), container.Types[typeof(IMenuView)]);
			Assert.AreEqual(typeof(MenuPresentationModel), container.Types[typeof(IMenuPresentationModel)]);
		}
	}
}
