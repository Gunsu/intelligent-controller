using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class OutputCommandConstBlock : OutputCommandBlock, ICloneable
	{
		protected OutputCommandConstBlock() {}
		public OutputCommandConstBlock(OutputCommandConstBlock outputCommandConstBlock)
		{
			throw new System.NotImplementedException();
		}

		public OutputCommandConstBlock(Point size, string name, List<BlockInputPoint> inputPoints, List<BlockOutputPoint> outputPoints)
		{
			throw new System.NotImplementedException();
		}
	
		public string Mask
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
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
		#region ICloneable Members

		public object Clone()
		{
			throw new NotImplementedException();
		}

		#endregion

		public void Draw()
		{
			throw new System.NotImplementedException();
		}
	}
}
