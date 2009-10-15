using System;
using IC.Core.Abstract;
using ValidationAspects.PostSharp;
using ValidationAspects;

namespace IC.Core.Entities
{
	/// <summary>
	/// Входная или выходная точка блока.
	/// </summary>
	[Validate]
	public sealed class BlockConnectionPoint : ConnectionPoint
	{
		/// <summary>
		/// Название.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание о предназначении данной точки.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Размер в байтах.
		/// </summary>
		public int Size { get; set; }

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
