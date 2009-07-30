namespace IC.CoreInterfaces.Objects
{
	/// <summary>
	/// Входная или выходная точка блока.
	/// </summary>
	public interface IBlockConnectionPoint
	{
		/// <summary>
		/// Название.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Описание о предназначении данной точки.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Размер в байтах.
		/// </summary>
		int Size { get; }
	}
}
