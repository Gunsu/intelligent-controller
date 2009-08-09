using System;
using IC.Modules.Menu.Interfaces.PresentationModels;
using IC.Modules.Menu.Interfaces.Views;

namespace IC.Modules.Menu.Tests.Mocks
{
	public sealed class MockMenuView : IMenuView
	{
		#region IMenuView members

		public IMenuPresentationModel Model { get; set; }

		#endregion
	}
}
