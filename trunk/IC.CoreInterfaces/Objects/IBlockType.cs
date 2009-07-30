using System.Collections.Generic;

namespace IC.CoreInterfaces.Objects
{
	/// <summary>
	/// Тип блока.
	/// </summary>
	public interface IBlockType
	{
		/// <summary>
		/// Название.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Идентификатор.
		/// </summary>
		int ID { get; }

		/// <summary>
		/// Описание о предназначении данного типа блока.
		/// </summary>
		string Description { get; }

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
