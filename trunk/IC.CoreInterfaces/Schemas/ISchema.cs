using System.Collections.Generic;

namespace IC.CoreInterfaces.Schemas
{
	/// <summary>
	/// Схема, являющаяся совокупностью связанных между собой блоков.
	/// </summary>
	public interface ISchema
	{
		/// <summary>
		/// Блоки, входящие в схему.
		/// </summary>
		IBlocks Blocks { get; }
	}
}
