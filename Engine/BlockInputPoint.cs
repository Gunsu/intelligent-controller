using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SSAU.BlocksConstruct.Engine
{
	public class BlockInputPoint : BlockConnectionPoint
	{
		public BlockInputPoint(Block block, string name, Point pos, Orientation orientation, int dataType)
			: base(block, name, pos, orientation, dataType)
		{}

		public BlockInputPoint(BlockInputPoint blockInputPoint)
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
