using System;

using IC.UI.Infrastructure.Events;

namespace IC.PresentationModels.Tests.Mocks.Events
{
	public sealed class MockProjectCreatingEvent : ProjectCreatingEvent
	{
		public bool IsPublished { get; private set; }

		public override void Publish(EventArgs payload)
		{
			IsPublished = true;
			base.Publish(payload);
		}
	}
}
