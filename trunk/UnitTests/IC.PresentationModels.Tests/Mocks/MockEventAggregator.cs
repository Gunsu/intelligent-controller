using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Events;

namespace IC.PresentationModels.Tests.Mocks
{
	public sealed class MockEventAggregator : IEventAggregator
	{
		private readonly Dictionary<Type, object> _events = new Dictionary<Type, object>();

		#region IEventAggregator Members

		public TEventType GetEvent<TEventType>() where TEventType : EventBase
		{
			return (TEventType) _events[typeof (TEventType)];
		}

		#endregion

		public void AddMapping<TEventType>(TEventType mockEvent)
		{
			_events.Add(typeof (TEventType), mockEvent);
		}
	}
}
