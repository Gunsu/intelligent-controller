using System.Collections.Generic;

namespace IC.CoreInterfaces.Objects
{
	/// <summary>
	/// Список блоков с возможностью получить список только нужных блоков, например, только входящих.
	/// </summary>
	public interface IBlocks : IList<IBlock>
	{
		/// <summary>
		/// Получает список всех входных блоков из данного списка.
		/// </summary>
		/// <returns>Список всех входных блоков из данного списка.</returns>
		IList<ICommandInputBlock> GetCommandInputBlocks();

		/// <summary>
		/// Получает список всех выходных блоков из данного списка.
		/// </summary>
		/// <returns>Список всех выходных блоков из данного списка.</returns>
		IList<ICommandOutputBlock> GetCommandOutputBlocks();
	}
}
