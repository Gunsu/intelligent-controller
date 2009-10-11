using System.Collections.Generic;
using IC.Core.Entities;

namespace IC.Core.Abstract
{
	public interface IBlockTypesRepository
	{
		IList<BlockType> LoadBlockTypesFromFile();
	}
}
