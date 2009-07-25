using System.Collections.Generic;

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
		/// Закрытый конструктор, выполняющий начальную инициализацию класса.
		/// </summary>
		private Schema()
		{
			Blocks = new Blocks();
		}
	}
}
