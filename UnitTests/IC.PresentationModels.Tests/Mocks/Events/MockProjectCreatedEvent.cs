using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;

namespace IC.PresentationModels.Tests.Mocks.Events
{
	public sealed class MockProjectCreatedEvent : ProjectCreatedEvent
	{
		public bool IsPublished { get; private set; }

		public override void Publish(IProject payload)
		{
			IsPublished = true;
			base.Publish(payload);
		}
	}
}
