using System.Xml.Linq;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее при сохранении схемы.
	/// </summary>
	public class SchemaSavingEvent : CompositePresentationEvent<XElement>
	{
	}
}
