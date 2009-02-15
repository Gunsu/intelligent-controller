using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class CommandBlock : Block, ICloneable
	{
		protected CommandBlock(){}
		private string _mask;

		public CommandBlock(CommandBlock commandBlock)
		{
			throw new System.NotImplementedException();
		}

		public CommandBlock(Point size, string name, List<BlockInputPoint> inputPoints, List<BlockOutputPoint> outputPoints)
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
