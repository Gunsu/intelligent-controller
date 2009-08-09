using IC.Modules.Menu.PresentationModels;
using IC.Modules.Menu.Tests.Mocks;
using NUnit.Framework;

namespace IC.Modules.Menu.Tests.PresentationModels
{
	[TestFixture]
	public sealed class MenuPresentationModelFixture
	{
		private MockEventAggregator _eventAggregator;
		private MockMenuView _view;

		[SetUp]
		public void SetUp()
		{
			_eventAggregator = new MockEventAggregator();
			_view = new MockMenuView();
		}

		[Test]
		public void CanInitPresenter()
		{
			MenuPresentationModel presentationModel = CreatePresenter();
			Assert.AreEqual(_view, presentationModel.View);
		}

		public void ClickingOnCreateProjectMenuItemCallsTheCreateProjectMethod()
		{
			MenuPresentationModel presentationModel = CreatePresenter();
			
		}

		private MenuPresentationModel CreatePresenter()
		{
			return new MenuPresentationModel(_view, _eventAggregator);
		}
	}
}
