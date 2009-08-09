using System.Collections.ObjectModel;
using IC.CoreInterfaces.Objects;

namespace IC.Modules.Toolbox.Interfaces.Services
{
	public interface IBlockTypesService
	{
		ObservableCollection<IBlockType> RetreiveBlockTypes();
	}
}
