using System.Collections.Generic;
using System.Xml.Linq;
using IC.CoreInterfaces.Objects;

namespace IC.Core.Objects
{
	/// <summary>
	/// Схема, являющаяся совокупностью связанных между собой блоков.
	/// </summary>
	public class Schema : ISchema
	{
		/// <summary>
		/// Блоки, входящие в схему.
		/// </summary>
		public IBlocks Blocks { get; private set; }

        /// <summary>
        /// Имя схемы.
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Определяет, сохранена ли схема.
		/// </summary>
		public bool IsSaved { get; set; }

		/// <summary>
		/// Определяет структуру UI.
		/// </summary>
		public XElement UISchema { get; set; }


		/// <summary>
		/// Закрытый конструктор, выполняющий начальную инициализацию класса.
		/// </summary>
		private Schema()
		{
			Blocks = new Blocks();
			IsSaved = false;
		}
	}
}
