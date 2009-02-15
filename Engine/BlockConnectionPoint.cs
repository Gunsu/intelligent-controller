using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class BlockConnectionPoint : ConnectionPoint
	{
		private Block _block;

		protected BlockConnectionPoint(){}
		public BlockConnectionPoint(Block block, string name, Point pos, Orientation orientation, int dataType)
		{
			throw new System.NotImplementedException();
		}

		public BlockConnectionPoint(BlockConnectionPoint blockConnectionPoint)
		{
			throw new System.NotImplementedException();
		}

		public Block Block
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public virtual bool CanMove
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public virtual Point Pos
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public void DisconnectAll()
		{
			throw new System.NotImplementedException();
		}
	}
}
