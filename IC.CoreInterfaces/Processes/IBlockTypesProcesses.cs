using System.Collections.Generic;
using IC.CoreInterfaces.Objects;

namespace IC.CoreInterfaces.Processes
{
	/// <summary>
	/// Процессы для работы с блоками.
	/// </summary>
	public interface IBlockTypesProcesses
	{
		/// <summary>
		/// Загружает типы блоков из xml файла.
		/// </summary>
		/// <returns>Типы блоков.</returns>
		IList<IBlockType> LoadBlockTypesFromFile();
	}
}
