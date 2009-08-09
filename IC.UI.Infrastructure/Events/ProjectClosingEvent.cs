using IC.CoreInterfaces.Objects;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее перед закрытием проекта.
	/// </summary>
	public sealed class ProjectClosingEvent : CompositePresentationEvent<IProject>
	{
	}
}
