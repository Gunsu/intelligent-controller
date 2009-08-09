using System;
using System.Collections.Generic;
using Microsoft.Practices.Composite.Events;

namespace IC.Modules.Menu.Tests.Mocks
{
	public class MockEventAggregator : IEventAggregator
	{
		private readonly Dictionary<Type, object> _events = new Dictionary<Type, object>();
		
		public TEventType GetEvent<TEventType>() where TEventType : EventBase
		{
			return (TEventType)_events[typeof(TEventType)];
		}

		public void AddMapping<TEventType>(TEventType mockEvent)
		{
			_events.Add(typeof(TEventType), mockEvent);
		}
	}
}
