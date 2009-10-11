using System;
using System.Collections.Generic;

namespace IC.Core.Entities
{
	/// <summary>
	/// Список блоков с возможностью получить список только нужных блоков, например, только входящих.
	/// </summary>
	[Serializable]
	public class Blocks : List<Block>
	{
		/// <summary>
		/// Получает список всех входных блоков из данного списка.
		/// </summary>
		/// <returns>Список всех входных блоков из данного списка.</returns>
		public List<CommandInputBlock> GetCommandInputBlocks()
		{
			throw new System.NotImplementedException();
			//return GetBlocksByType(typeof(CommandInputBlock));
		}

		/// <summary>
		/// Получает список всех выходных блоков из данного списка.
		/// </summary>
		/// <returns>Список всех выходных блоков из данного списка.</returns>
		public List<CommandOutputBlock> GetCommandOutputBlocks()
		{
			throw new System.NotImplementedException();
			//return GetBlocksByType(typeof(CommandOutputBlock));
		}

		/// <summary>
		/// Поулчает список всех блоков определенного типа.
		/// </summary>
		/// <param name="type">Тип блоков, которые необходимо получить.</param>
		/// <returns>Список всех блоков определенного типа.</returns>
		private List<Block> GetBlocksByType(Type type)
		{
			List<Block> result = new List<Block>();

			foreach (var block in this)
			{
				if (block.GetType() == type)
				{
					result.Add(block);
				}
			}

			return result;
		}


		/// <summary>
		/// Проверяет, что все входные блоки соединены в цепочку.
		/// </summary>
		/// <returns>Возвращает true, если все входные блоки соединены в цепочку.</returns>
		public bool CommandInputBlocksAreConnectedInChain()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Проверяет, что все выходные блоки соединены в цепочку.
		/// </summary>
		/// <returns>Возвращает true, если все выходные блоки соединены в цепочку.</returns>
		public bool CommandOutputBlocksAreConnectedInChain()
		{
			throw new NotImplementedException();
		}
	}
}
