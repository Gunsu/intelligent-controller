using IC.CoreInterfaces.Processes;
using Project.Utils.Common;
using Project.Utils.DesignByContract;
using System;
using IC.CoreInterfaces.Objects;

namespace IC.Core.Processes
{
	public sealed class BlockProcesses : IBlockProcesses
	{
		/// <summary>
		/// Проверяет, что все входные блоки соединены в цепочку.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <returns>Возвращает true, если все входные блоки соединены в цепочку.</returns>
		public ProcessResult<bool> CommandInputBlocksAreConnectedInChain(IBlocks schema)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Проверяет, что все выходные блоки соединены в цепочку.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <returns>Возвращает true, если все выходные блоки соединены в цепочку.</returns>
		public ProcessResult<bool> CommandOutputBlocksAreConnectedInChain(IBlocks schema)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Проверяет блок на висячие входы.
		/// </summary>
		/// <param name="block">Блок для проверки.</param>
		/// <returns>Возвращает true, если блок имеет один или больше висячих входов.</returns>
		public ProcessResult<bool> BlockHasHangingInput(IBlock block)
		{
			throw new NotImplementedException();
		}
	}
}
