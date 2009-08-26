using System;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее перед созданием проекта.
	/// В качестве параметра передаётся текущий проект.
	/// </summary>
	public class ProjectCreatingEvent : CompositePresentationEvent<EventArgs>
	{
	}
}
