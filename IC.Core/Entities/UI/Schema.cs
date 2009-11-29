using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.Serialization;
using ValidationAspects;

namespace IC.Core.Entities.UI
{
	/// <summary>
	/// Схема, являющаяся совокупностью связанных между собой блоков.
	/// </summary>
	[Serializable]
	public class Schema
	{
		private readonly List<Block> _blocks;

		/// <summary>
		/// Блоки, входящие в схему.
		/// </summary>
		public ReadOnlyCollection<Block> Blocks
		{
			get { return _blocks.AsReadOnly(); }
		}

		/// <summary>
		/// Имя схемы.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Определяет, сохранена ли схема.
		/// </summary>
		[XmlIgnore]
		public bool IsSaved { get; set; }

		/// <summary>
		/// Определяет структуру UI.
		/// </summary>
		[NonSerialized] private XElement _uiSchema;
		public XElement UISchema
		{
			get { return _uiSchema; }
			set { _uiSchema = value; }
		}

		public Project Project { get; set; }

		public Schema()
		{
			_blocks = new List<Block>();
			IsSaved = false;
		}

		/// <summary>
		/// Сохраняет схему.
		/// </summary>
		/// <param name="uiSchema">Сериализованный набор компонентов в дизайнере.</param>
		/// <returns>Возвращает true, если схема успешно сохранена.</returns>
		public bool Save([NotNull] XElement uiSchema)
		{
			UISchema = uiSchema;
			IsSaved = true;
			return true;
		}

		public Block AddBlock(BlockType blockType, Coordinates coordinates)
		{
			var block = new Block(blockType, coordinates);
			_blocks.Add(block);
			return block;
		}
	}
}
