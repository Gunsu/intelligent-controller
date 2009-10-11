using IC.Core.Entities;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее сразу после смены текущей схемы в ProjectExplorer.
	/// </summary>
	public class CurrentSchemaChangedEvent : CompositePresentationEvent<Schema>
	{
	}
}
