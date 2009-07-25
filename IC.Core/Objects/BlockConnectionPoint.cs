using IC.CoreInterfaces.Objects;
using ValidationAspects.PostSharp;
using ValidationAspects;

namespace IC.Core.Objects
{
	/// <summary>
	/// Входная или выходная точка блока.
	/// </summary>
	[Validate]
	public sealed class BlockConnectionPoint : IBlockConnectionPoint
	{
		#region IBlockConnectionPoint Members

		/// <summary>
		/// Имя точки.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Размер выхода в байтах.
		/// </summary>
		public int Size { get; private set; }

		#endregion

		public BlockConnectionPoint([NotNullOrEmpty] string name, [Minimum(1)] int size)
		{
			Name = name;
			Size = size;
		}
	}
}
