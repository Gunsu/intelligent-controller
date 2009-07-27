using System.Collections.Generic;

namespace IC.CoreInterfaces.Objects
{
	/// <summary>
	/// Тип блока.
	/// </summary>
	public interface IBlockType
	{
		string Name { get; }

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
