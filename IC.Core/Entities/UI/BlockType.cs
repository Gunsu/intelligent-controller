using System;
using System.Collections.Generic;
using ValidationAspects;

namespace IC.Core.Entities.UI
{
	/// <summary>
	/// Тип блока.
	/// </summary>
	[Serializable]
	public class BlockType
	{
		public BlockType()
		{
			InputPoints = new List<ConnectionPoint>();
			OutputPoints = new List<ConnectionPoint>();
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

		/// <summary>
		/// Название.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Идентификатор.
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// Описание о предназначении данного типа блока.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Входные точки.
		/// </summary>
		public List<ConnectionPoint> InputPoints { get; set; }

		/// <summary>
		/// Выходные точки.
		/// </summary>
		public List<ConnectionPoint> OutputPoints { get; set; }

		public string ToolTip
		{
			get { return Name; }
		}
	}
}
