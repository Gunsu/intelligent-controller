using System;

using IC.UI.Infrastructure.Events;

namespace IC.PresentationModels.Tests.Mocks
{
	public sealed class MockProjectCreatingEvent : ProjectCreatingEvent
	{
		public bool IsPublished { get; private set; }

		public override void Publish(EventArgs payload)
		{
			IsPublished = true;
		}
	}
}
