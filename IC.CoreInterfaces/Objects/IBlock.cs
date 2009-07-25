using Project.Utils.Common;
using System.Collections.Generic;

namespace IC.CoreInterfaces.Objects
{
	/// <summary>
	/// Блок команды, выполняющий определённое действие, например, проверку на ноль, и, в общем случае, имеющий точки входа и выхода.
	/// </summary>
	public interface IBlock
	{
		/// <summary>
		/// Идентификатор блока.
		/// </summary>
		int ID { get; }

		/// <summary>
		/// Входные точки.
		/// </summary>
		IList<IBlockConnectionPoint> InputPoints { get; }

		/// <summary>
		/// Выходные точки.
		/// </summary>
		IList<IBlockConnectionPoint> OutputPoints { get; }
	}
}
