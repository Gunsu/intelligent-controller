using System;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;

namespace IC.PresentationModels.Tests.Mocks.Events
{
	public sealed class MockCurrentSchemaChangedEvent : CurrentSchemaChangedEvent
	{
		public bool IsPublished { get; private set; }
		public ISchema PayloadPublished { get; private set; }

		public override void Publish(ISchema payload)
		{
			IsPublished = true;
			PayloadPublished = payload;
			base.Publish(payload);
		}
	}
}
