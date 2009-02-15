using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class InputCommandBlock : CommandBlock, ICloneable
	{
		protected InputCommandBlock() {}
		public InputCommandBlock(InputCommandBlock inputCommandBlock)
		{
			throw new System.NotImplementedException();
		}

		public InputCommandBlock(Point size, string name, List<BlockInputPoint> inputPoints, List<BlockOutputPoint> outputPoints)
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
