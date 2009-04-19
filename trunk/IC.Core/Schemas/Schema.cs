using System.Collections.Generic;
using IC.CoreInterfaces.Schemas;

namespace IC.Core.Schemas
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
		/// Закрытый конструктор, выполняющий начальную инициализацию класса.
		/// </summary>
		private Schema()
		{
			Blocks = new Blocks();
		}
	}
}
