using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public abstract class GraphicalObject
	{
		private Point pos;
		private bool selected;
		private string description;

		public virtual bool Selected
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

		public virtual ObjectType Type
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public virtual bool TestIntersect(Point pos)
		{
			throw new System.NotImplementedException();
		}

		public virtual void Draw()
		{
			throw new System.NotImplementedException();
		}
	}
}
