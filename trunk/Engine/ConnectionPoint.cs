using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSAU.BlocksConstruct.Engine
{
	public class ConnectionPoint : GraphicalObject
	{
		public CompileData compileData;
		private List<ConnectionPoint> outputs;
		private int dataType;
		private Orientation orientation;
		private bool signaledState;
		private string name;

		public ConnectionPoint(int dataType, Orientation orientation, string name)
		{
			throw new System.NotImplementedException();
		}

		public ConnectionPoint(ConnectionPoint connectionPoint)
		{
			throw new System.NotImplementedException();
		}

		public virtual bool Signaled
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public static double R
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public int DataType
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
			}
		}

		public virtual string Name
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

		/// <summary>
		/// Каскадно отмечает все дочерние точки как обработанные.
		/// </summary>
		public void SetCompileProcessedRecursive()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Каскадно вешает индекс переменной на все дочерние точки.
		/// </summary>
		public void SetCompileVariableRecursive(int varIndex)
		{
			throw new System.NotImplementedException();
		}

		public void Connect(ConnectionPoint connectionPoint)
		{
			throw new System.NotImplementedException();
		}

		public void Disconnect(ConnectionPoint connectionPoint)
		{
			throw new System.NotImplementedException();
		}

		public void DisconnectAll()
		{
			throw new System.NotImplementedException();
		}

		public ConnectionPoint GetFirstOutputPoint()
		{
			throw new System.NotImplementedException();
		}

		public ConnectionPoint GetNextOutputPoint()
		{
			throw new System.NotImplementedException();
		}

		public void SetCurrentOutputPoint(ConnectionPoint connectionPoint)
		{
			throw new System.NotImplementedException();
		}

		public int GetOutputPointsCount()
		{
			throw new System.NotImplementedException();
		}

		public void OnOutputSetSelected(ConnectionPoint output, bool selected)
		{
			throw new System.NotImplementedException();
		}

		public virtual void TestIntersect(Point pos)
		{
			throw new System.NotImplementedException();
		}

		public virtual void Draw()
		{
			throw new System.NotImplementedException();
		}
	}
}
