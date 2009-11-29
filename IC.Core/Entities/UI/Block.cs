using System;
using System.Collections.Generic;
using ValidationAspects.PostSharp;

namespace IC.Core.Entities.UI
{
	[Serializable]
	[Validate]
	public class Block
	{
		#region Fields and properties

		public BlockType BlockType { get; set; }
		public List<ConnectionPoint> InputPoints { get; set; }
		public List<ConnectionPoint> OutputPoints { get; set; }
		public Coordinates Coordinates { get; set; }
		
		#endregion


		#region Constructors

		public Block()
		{
			InputPoints = new List<ConnectionPoint>();
			OutputPoints = new List<ConnectionPoint>();
		}

		public Block(BlockType blockType, Coordinates coordinates)
			: this()
		{
			BlockType = blockType;
			Coordinates = coordinates;
		}

		#endregion
	}
}
