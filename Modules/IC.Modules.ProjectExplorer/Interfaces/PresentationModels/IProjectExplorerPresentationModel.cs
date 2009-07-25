using System.Collections.ObjectModel;
using IC.Modules.ProjectExplorer.Interfaces.Views;

namespace IC.Modules.ProjectExplorer.Interfaces.PresentationModels
{
	public interface IProjectExplorerPresentationModel
	{
		IProjectExplorerView View { get; }

		string ProjectName { get; }

	    ObservableCollection<string> SchemasListItems { get; }

        string CurrentSchemaItem { get; }
	}
}
