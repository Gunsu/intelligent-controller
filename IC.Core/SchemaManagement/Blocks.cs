using System;
using System.Collections.Generic;
using IC.CoreInterfaces.SchemaManagement;

namespace IC.Core.SchemaManagement
{
	/// <summary>
	/// Список блоков с возможностью получить список только нужных блоков, например, только входящих.
	/// </summary>
	public class Blocks : List<IBlock>, IBlocks
	{
		/// <summary>
		/// Получает список всех входных блоков из данного списка.
		/// </summary>
		/// <returns>Список всех входных блоков из данного списка.</returns>
		public IList<ICommandInputBlock> GetCommandInputBlocks()
		{
			return GetBlocksByType(typeof(ICommandInputBlock)) as IList<ICommandInputBlock>;
		}

		/// <summary>
		/// Получает список всех выходных блоков из данного списка.
		/// </summary>
		/// <returns>Список всех выходных блоков из данного списка.</returns>
		public IList<ICommandOutputBlock> GetCommandOutputBlocks()
		{
			return GetBlocksByType(typeof(ICommandOutputBlock)) as IList<ICommandOutputBlock>;
		}

		/// <summary>
		/// Поулчает список всех блоков определенного типа.
		/// </summary>
		/// <param name="type">Тип блоков, которые необходимо получить.</param>
		/// <returns>Список всех блоков определенного типа.</returns>
		private IList<IBlock> GetBlocksByType(Type type)
		{
			IList<IBlock> result = new List<IBlock>();

			foreach (var block in this)
			{
				if (block.GetType() == type)
				{
					result.Add(block);
				}
			}

			return result;
		}
	}
}
