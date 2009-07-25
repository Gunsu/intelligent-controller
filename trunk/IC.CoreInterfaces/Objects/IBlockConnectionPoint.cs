namespace IC.CoreInterfaces.Objects
{
	/// <summary>
	/// Входная или выходная точка блока.
	/// </summary>
	public interface IBlockConnectionPoint
	{
		/// <summary>
		/// Имя точки.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Размер выхода в байтах.
		/// </summary>
		int Size { get; }
	}
}
