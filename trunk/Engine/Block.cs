using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class Block : GraphicalObject, ICloneable
	{
		protected Block(){}
		private List<BlockInputPoint> _inputPoints;
		private List<BlockOutputPoint> _outputPoints;
		private string _name;
		private Point _size;
		private int _id;

		public Block(Point size, string name, List<BlockInputPoint> inputPoints, List<BlockOutputPoint> outputPoints, int id)
		{
			throw new System.NotImplementedException();
		}

		public Block(Block block)
		{
			throw new System.NotImplementedException();
		}
	
		public CompileData CompileData
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public virtual bool ShowNameAllowed
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public string Name
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public int ID
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

		public bool CanMove
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public Point Pos
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

		public void TestIntersect(Point pos)
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
