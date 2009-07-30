using System.Collections.ObjectModel;
using IC.CoreInterfaces.Objects;
using IC.Modules.ProjectExplorer.Interfaces.Views;

namespace IC.Modules.ProjectExplorer.Interfaces.PresentationModels
{
	public interface IProjectExplorerPresentationModel
	{
		IProjectExplorerView View { get; }

		string ProjectName { get; }

		ObservableCollection<ISchema> SchemasListItems { get; }

		ISchema CurrentSchemaItem { get; }
	}
}
