using System.Collections.Generic;

using IC.CoreInterfaces.Enums;
using Project.Utils.Common;

namespace IC.CoreInterfaces.Objects
{
	/// <summary>
	/// Блок команды, выполняющий определённое действие, например, проверку на ноль, и, в общем случае, имеющий точки входа и выхода.
	/// </summary>
	public interface IBlock
	{
		Coordinates Coordinates { get; set; }
		IBlockType BlockType { get; }
		Orientation Orientation { get; set; }
		IList<IBlockConnectionPoint> InputPoints { get; }
		IList<IBlockConnectionPoint> OutputPoints { get; }
		int X { get; set; }
		int Y { get; set; }
	}
}
