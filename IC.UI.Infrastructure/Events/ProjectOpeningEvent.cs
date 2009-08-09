using IC.CoreInterfaces.Objects;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее перед открытием проекта.
	/// </summary>
	public sealed class ProjectOpeningEvent : CompositePresentationEvent<IProject>
	{
	}
}
