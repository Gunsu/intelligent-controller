using System;
using System.Collections.Specialized;
using System.Xml.Linq;
using ValidationAspects;

namespace IC.Core.Entities
{
	/// <summary>
	/// Схема, являющаяся совокупностью связанных между собой блоков.
	/// </summary>
	[Serializable]
	public class Schema
	{
		/// <summary>
		/// Блоки, входящие в схему.
		/// </summary>
		public Blocks Blocks { get; set; }

        /// <summary>
        /// Имя схемы.
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Определяет, сохранена ли схема.
		/// </summary>
		public bool IsSaved { get; set; }

		[NonSerialized]
		private XElement _uiSchema;

		/// <summary>
		/// Определяет структуру UI.
		/// </summary>
		public XElement UISchema
		{
			get { return _uiSchema; }
			set { _uiSchema = value; }
		}

		/// <summary>
		/// Закрытый конструктор, выполняющий начальную инициализацию класса.
		/// </summary>
		public Schema()
		{
			Blocks = new Blocks();
			IsSaved = false;
		}

        private readonly Project _project;

		/// <summary>
		/// Компилирует схему.
		/// </summary>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		public void Compile()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Проверяет схему.
		/// </summary>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		public void Validate()
		{
			bool noErrors = true;
			StringCollection errors = new StringCollection();

			if (Blocks.GetCommandInputBlocks().Count == 0)
			{
				noErrors = false;
				errors.Add("Необходим хотя бы один блок входной команды.");
			}

			if (Blocks.GetCommandOutputBlocks().Count == 0)
			{
				noErrors = false;
				errors.Add("Необходим хотя бы один блок выходной команды.");
			}

			if (Blocks.CommandInputBlocksAreConnectedInChain() == false)
			{
				noErrors = false;
				errors.Add("Не все блоки входной команды соединены в цепочку.");
			}

			if (Blocks.CommandOutputBlocksAreConnectedInChain() == false)
			{
				noErrors = false;
				errors.Add("Не все блоки выходной команды соединены в цепочку.");
			}

#warning и что делать с координатами?
			//foreach (var block in schema.Blocks)
			//{
			//    if (bp.BlockHasHangingInput(block).Result == true)
			//    {
			//        noErrors = false;
			//        errors.Add(string.Format("Блок по координатам {0} имеет висячие входы.", block.Coordinates));
			//    }
			//}
			if (!noErrors)
				throw new NotImplementedException();
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
	}
}
