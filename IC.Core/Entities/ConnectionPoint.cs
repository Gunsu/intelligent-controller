using System.Collections.Generic;
using IC.Core.Enums;

namespace IC.Core.Entities
{
	public class ConnectionPoint
	{
		public List<ConnectionPoint> Outputs { get; set; }

		internal int VariableIndex { get; set; }
		internal bool Processed { get; set; }
		public int Size { get; set; }
		internal ObjectType ObjectType { get; set; }

		public ConnectionPoint()
		{
			VariableIndex = -1;
			Processed = false;
			Outputs = new List<ConnectionPoint>();
		}

		public void SetProcessedFlagRecursive()
		{
			Processed = true;
			foreach (var output in Outputs)
			{
				output.SetProcessedFlagRecursive();
			}
		}

		/// <summary>
		/// Какскадно вешает индекс переменной на все дочерние точеи
		/// </summary>
		public void SetCompileVariableRecursive(int index)
		{
			VariableIndex = index;
			foreach (var point in Outputs)
				point.SetCompileVariableRecursive(index);
		}
	}
}
