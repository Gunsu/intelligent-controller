using System;
using IC.UI.Infrastructure.Events;

namespace IC.PresentationModels.Tests.Mocks.Events
{
	public sealed class MockProjectOpeningEvent : ProjectOpeningEvent
	{
		public bool IsPublished { get; private set; }

		public override void Publish(EventArgs payload)
		{
			IsPublished = true;
			base.Publish(payload);
		}
	}
}
