using System.Collections.Generic;

namespace IC.Core.Entities
{
	public class ConnectionPoint
	{
		public List<ConnectionPoint> Outputs { get; set; }

		internal int VariableIndex { get; set; }
		internal bool Processed { get; set; }

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
	}
}
