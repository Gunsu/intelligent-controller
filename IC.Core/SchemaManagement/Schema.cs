using System.Collections.Generic;
using IC.CoreInterfaces.SchemaManagement;

namespace IC.Core.SchemaManagement
{
	/// <summary>
	/// Схема, являющаяся совокупностью связанных между собой блоков.
	/// </summary>
	public class Schema : ISchema
	{
		/// <summary>
		/// Блоки, входящие в схему.
		/// </summary>
		public IList<IBlock> Blocks { get; private set; }

		/// <summary>
		/// Закрытый конструктор, выполняющий начальную инициализацию класса.
		/// </summary>
		private Schema()
		{
			Blocks = new List<IBlock>();
		}
	}
}
