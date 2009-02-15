using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public enum ObjectType
	{
		Block,
		CommandBlock,
		InputCommandBlock,
		OutputCommandBlock,
		OutputCommandBufBlock,
		OutputCommandConstBlock,
		ConnectionPoint,
		BlockInputPoint,
		BlockOutputPoint,
	}
}
