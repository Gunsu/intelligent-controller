using Project.Utils.Common;
using IC.CoreInterfaces.Objects;

namespace IC.CoreInterfaces.Processes
{
	public interface IBlockProcesses
	{
		ProcessResult<bool> CommandInputBlocksAreConnectedInChain(IBlocks blocks);
		ProcessResult<bool> CommandOutputBlocksAreConnectedInChain(IBlocks blocks);
		ProcessResult<bool> BlockHasHangingInput(IBlock block);
	}
}
