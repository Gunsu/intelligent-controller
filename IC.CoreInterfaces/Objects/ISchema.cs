using System.Xml.Linq;

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

		/// <summary>
		/// Определяет, сохранена ли схема.
		/// </summary>
		bool IsSaved { get; }

		/// <summary>
		/// Определяет структуру UI.
		/// </summary>
		XElement UISchema { get; set; }
	}
}
