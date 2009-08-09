using IC.CoreInterfaces.Objects;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	/// <summary>
	/// Событие, происходящее сразу после смены текущего типа блока в Toolbox.
	/// </summary>
	public sealed class CurrentBlockTypeChangedEvent : CompositePresentationEvent<IBlockType>
	{
	}
}
