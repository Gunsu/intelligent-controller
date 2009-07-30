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
		/// Название.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Описание о предназначении данной точки.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// Размер в байтах.
		/// </summary>
		public int Size { get; private set; }

		#endregion

		public BlockConnectionPoint([NotNullOrEmpty] string name, [Minimum(1)] int size)
		{
			Name = name;
			Size = size;
		}

		public BlockConnectionPoint([NotNullOrEmpty] string name, [Minimum(1)] int size, string description)
			: this(name, size)
		{
			Description = description;
		}
	}
}
