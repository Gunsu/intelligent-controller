using IC.Modules.ProjectExplorer.Interfaces.PresentationModels;

namespace IC.Modules.ProjectExplorer.Interfaces.Views
{
	public interface IProjectExplorerView
	{
		IProjectExplorerPresentationModel Model { get; set; }
	}
}
