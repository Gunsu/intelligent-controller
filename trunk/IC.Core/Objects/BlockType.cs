using IC.CoreInterfaces.Objects;
using System.Collections.Generic;
using ValidationAspects;
using System.Drawing;

namespace IC.Core.Objects
{
	/// <summary>
	/// Тип блока.
	/// </summary>
	public class BlockType : IBlockType
	{
		private BlockType()
		{
			InputPoints = new List<IBlockConnectionPoint>();
			OutputPoints = new List<IBlockConnectionPoint>();
		}

		/// <remarks>Минимум 2 установлен, потому что 0 и 1 зарезервированы для входного и выходного блоков.</remarks>
		public BlockType ([Minimum(2)] int id, [NotNullOrEmpty] string name)
			: this()
		{
			ID = id;
			Name = name;
		}

		public BlockType(int id, string name, [NotNullOrEmpty] string description)
			: this(id, name)
		{
			Description = description;
		}

		#region IBlock Members

		/// <summary>
		/// Название.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Идентификатор.
		/// </summary>
		public int ID { get; private set; }

		/// <summary>
		/// Описание о предназначении данного типа блока.
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// Входные точки.
		/// </summary>
		public IList<IBlockConnectionPoint> InputPoints { get; private set; }

		/// <summary>
		/// Выходные точки.
		/// </summary>
		public IList<IBlockConnectionPoint> OutputPoints { get; private set; }

		public Image Image
		{
			get
			{
				return new Bitmap(@"c:\1.bmp");
			}
		}

		#endregion
	}
}
