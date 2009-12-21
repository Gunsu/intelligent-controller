using IC.Core.Entities.UI;
using IC.UI.Infrastructure.Events;
using Microsoft.Practices.Composite.Events;

namespace IC.PresentationModels.Tests.Mocks.Events
{
	public sealed class MockCurrentSchemaChangedEvent : CurrentSchemaChangedEvent
	{
		public bool IsPublished { get; private set; }
		public Schema PayloadPublished { get; private set; }

		public bool IsSubscribed { get; private set; }

		public override SubscriptionToken Subscribe(System.Action<Schema> action, Microsoft.Practices.Composite.Presentation.Events.ThreadOption threadOption, bool keepSubscriberReferenceAlive, System.Predicate<Schema> filter)
		{
			IsSubscribed = true;
			return base.Subscribe(action, threadOption, keepSubscriberReferenceAlive, filter);
		}

		public override void Publish(Schema payload)
		{
			IsPublished = true;
			PayloadPublished = payload;
			base.Publish(payload);
		}
	}
}
