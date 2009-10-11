using System.Collections.Generic;
using ValidationAspects;
using System.Drawing;

namespace IC.Core.Entities
{
	/// <summary>
	/// Тип блока.
	/// </summary>
	public class BlockType
	{
		public BlockType()
		{
			InputPoints = new List<BlockConnectionPoint>();
			OutputPoints = new List<BlockConnectionPoint>();
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
		public IList<BlockConnectionPoint> InputPoints { get; private set; }

		/// <summary>
		/// Выходные точки.
		/// </summary>
		public IList<BlockConnectionPoint> OutputPoints { get; private set; }

		public string ToolTip
		{
			get { return Name; }
		}

		#endregion
	}
}
