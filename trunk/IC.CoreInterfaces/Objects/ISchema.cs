using System.Collections.Generic;

namespace IC.CoreInterfaces.Objects
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

        /// <summary>
        /// Имя схемы.
        /// </summary>
        string Name { get; set; }
	}
}
