﻿using IC.CoreInterfaces.Objects;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.UI.Infrastructure.Events
{
	public sealed class CurrentBlockTypeChangedEvent : CompositePresentationEvent<IBlockType>
	{
	}
}
