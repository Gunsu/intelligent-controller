using System;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее перед созданием схемы.
	/// Нужно для того, чтобы ProjectExplorer сумел поймать данное событие и послать своё
	/// с текущей схемой в качестве параметра.
	/// </summary>
	public class SchemaCreatingEvent : CompositePresentationEvent<EventArgs>
	{
	}
}
