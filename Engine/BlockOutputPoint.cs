using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class BlockOutputPoint : BlockConnectionPoint
	{
		public BlockOutputPoint(Block block, string name, Point pos, Orientation orientation, int dataType)
			: base(block, name, pos, orientation, dataType)
		{}

		public BlockOutputPoint(BlockOutputPoint blockInputPoint)
		{
			throw new System.NotImplementedException();
		}

		public ObjectType Type
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public void Draw()
		{
			throw new System.NotImplementedException();
		}

	}
}
