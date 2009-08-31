using System.Collections.Generic;

using IC.CoreInterfaces.Enums;
using IC.CoreInterfaces.Objects;
using Project.Utils.Common;

namespace IC.Core.Objects
{
	public sealed class Block : IBlock
	{
		public Coordinates Coordinates { get; set; }
		public IBlockType BlockType { get; private set; }
		public IList<IBlockConnectionPoint> InputPoints { get; private set; }
		public IList<IBlockConnectionPoint> OutputPoints { get; private set; }
		public Orientation Orientation { get; set; }
		
		public Block(IBlockType blockType, Coordinates coordinates, Orientation orientation)
		{
			BlockType = blockType;
			Coordinates = coordinates;
			Orientation = orientation;
			InputPoints = new List<IBlockConnectionPoint>();
			OutputPoints = new List<IBlockConnectionPoint>();
		}
	}
}
