using System;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее перед сохранением проекта.
	/// </summary>
	public class ProjectSavingEvent : CompositePresentationEvent<EventArgs>
	{
	}
}
