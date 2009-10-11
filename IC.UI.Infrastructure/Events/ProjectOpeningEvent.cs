using System;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее перед открытием проекта.
	/// </summary>
	public class ProjectOpeningEvent : CompositePresentationEvent<EventArgs>
	{
	}
}
