using System;
using System.Collections.Generic;

namespace IC.Core.Entities
{
	/// <summary>
	/// Список блоков с возможностью получить список только нужных блоков, например, только входящих.
	/// </summary>
	public class Blocks : List<Block>
	{
		/// <summary>
		/// Получает список всех входных блоков из данного списка.
		/// </summary>
		/// <returns>Список всех входных блоков из данного списка.</returns>
		public List<InputCommandBlock> GetCommandInputBlocks()
		{
			return GetBlocksByType<InputCommandBlock>();
		}

		/// <summary>
		/// Получает список всех выходных блоков из данного списка.
		/// </summary>
		/// <returns>Список всех выходных блоков из данного списка.</returns>
		public List<OutputCommandBlock> GetCommandOutputBlocks()
		{
			return GetBlocksByType<OutputCommandBlock>();
		}

		/// <summary>
		/// Получает список обычных блоков, являющихся не входными и не выходными.
		/// </summary>
		/// <returns>Список обычных блоков.</returns>
		public List<Block> GetSimpleBlocks()
		{
			return GetBlocksByType<Block>();
		}

		/// <summary>
		/// Поулчает список всех блоков определенного типа.
		/// </summary>
		/// <param name="type">Тип блоков, которые необходимо получить.</param>
		/// <returns>Список всех блоков определенного типа.</returns>
		private List<T> GetBlocksByType<T>() where T: Block
		{
			List<T> result = new List<T>();
			foreach (Block block in this)
			{
				if (block.GetType() == typeof(T))
				{
					result.Add((T)block);
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
