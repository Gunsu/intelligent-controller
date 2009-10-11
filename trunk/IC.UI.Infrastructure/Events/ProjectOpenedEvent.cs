using IC.Core.Entities;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее сразу после открытия проекта.
	/// </summary>
	public sealed class ProjectOpenedEvent : CompositePresentationEvent<Project>
	{
	}
}
