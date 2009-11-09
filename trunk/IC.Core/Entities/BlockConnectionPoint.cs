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
	public class BlockConnectionPoint : ConnectionPoint
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
		/// Блок, к которому подсоединена данная связь.
		/// </summary>
		public Block Block { get; set; }

		public BlockConnectionPoint()
		{
			this.ObjectType = IC.Core.Enums.ObjectType.ConnectionPoint;
		}

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
