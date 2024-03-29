﻿using IC.Core.Entities.UI;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее сразу после закрытия проекта.
	/// </summary>
	public sealed class ProjectClosedEvent : CompositePresentationEvent<Project>
	{
	}
}
