using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class OutputCommandBlock : CommandBlock, ICloneable
	{
		private bool _inputConnected;

		public OutputCommandBlock(OutputCommandBlock outputCommandBlock)
		{
			throw new System.NotImplementedException();
		}

		public OutputCommandBlock(Point size, string name, List<BlockInputPoint> inputPoints, List<BlockOutputPoint> outputPoints)
		{
			throw new System.NotImplementedException();
		}

		public bool InputConnected
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

		public void Draw()
		{
			throw new System.NotImplementedException();
		}

		#region ICloneable Members

		public object Clone()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
