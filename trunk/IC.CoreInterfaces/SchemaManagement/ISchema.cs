using System.Collections.Generic;

namespace IC.CoreInterfaces.SchemaManagement
{
	/// <summary>
	/// Схема, являющаяся совокупностью связанных между собой блоков.
	/// </summary>
	public interface ISchema
	{
		/// <summary>
		/// Блоки, входящие в схему.
		/// </summary>
		IList<IBlock> Blocks { get; }
	}
}
