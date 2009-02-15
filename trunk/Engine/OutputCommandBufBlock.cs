using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class OutputCommandBufBlock : OutputCommandBlock, ICloneable
	{
		public OutputCommandBufBlock(OutputCommandBufBlock outputCommandBufBlock)
		{
			throw new System.NotImplementedException();
		}

		public OutputCommandBufBlock(Point size, string name, List<BlockInputPoint> inputPoints, List<BlockOutputPoint> outputPoints)
		{
			throw new System.NotImplementedException();
		}
	
		public bool ShowNameAllowed
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
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
