using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;

using IC.CoreInterfaces.Objects;
using System;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее перед созданием схемы.
	/// Нужно для того, чтобы ProjectExplorer сумел поймать данное событие и послать своё
	/// с текущей схемой в качестве параметра.
	/// </summary>
	public sealed class SchemaPreCreatingEvent : CompositePresentationEvent<EventArgs>
	{
	}
}
