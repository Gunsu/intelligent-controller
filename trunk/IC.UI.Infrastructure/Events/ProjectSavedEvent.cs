using System;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходяшее после успешного сохранения проекта.
	/// </summary>
	public class ProjectSavedEvent : CompositePresentationEvent<EventArgs>
	{
	}
}
