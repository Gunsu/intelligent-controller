using System;
using Microsoft.Practices.Composite.Presentation.Events;

using IC.CoreInterfaces.Objects;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее перед созданием проекта.
	/// В качестве параметра передаётся текущий проект.
	/// </summary>
	public sealed class ProjectCreatingEvent : CompositePresentationEvent<EventArgs>
	{
	}
}
