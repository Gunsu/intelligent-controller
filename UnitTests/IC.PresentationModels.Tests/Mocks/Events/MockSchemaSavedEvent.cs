using System;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;

using IC.UI.Infrastructure.Events;

namespace IC.PresentationModels.Tests.Mocks.Events
{
	public sealed class MockSchemaSavedEvent : SchemaSavedEvent
	{
		public bool IsPublished { get; private set; }

		public bool IsSubscribed { get; private set; }

		public override void Publish(EventArgs args)
		{
			IsPublished = true;
			base.Publish(args);
		}

		public override SubscriptionToken Subscribe(Action<EventArgs> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<EventArgs> filter)
		{
			IsSubscribed = true;
			return base.Subscribe(action, threadOption, keepSubscriberReferenceAlive, filter);
		}
	}
}
