﻿using System.Xml.Linq;

using IC.UI.Infrastructure.Events;

namespace IC.PresentationModels.Tests.Mocks.Events
{
	public sealed class MockSchemaSavingEvent : SchemaSavingEvent
	{
		public bool IsPublished { get; private set; }
		public XElement PublishedPayload { get; private set; }

		public override void Publish(XElement payload)
		{
			IsPublished = true;
			PublishedPayload = payload;
			base.Publish(payload);
		}
	}
}
