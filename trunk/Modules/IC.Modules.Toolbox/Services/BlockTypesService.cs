using System.Collections.Generic;
using System.Collections.ObjectModel;
using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using IC.Modules.Toolbox.Interfaces.Services;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.Modules.Toolbox.Services
{
	[Validate]
	public sealed class BlockTypesService : IBlockTypesService
	{
		private readonly IList<IBlockType> _blockTypes;

		#region IBlockTypesService members

		public ObservableCollection<IBlockType> RetreiveBlockTypes()
		{
			return new ObservableCollection<IBlockType>(_blockTypes);
		}

		#endregion

		public BlockTypesService([NotNull] IBlockTypesProcesses blockTypesProcesses)
		{
			_blockTypes = blockTypesProcesses.LoadBlockTypesFromFile();
		}
	}
}
