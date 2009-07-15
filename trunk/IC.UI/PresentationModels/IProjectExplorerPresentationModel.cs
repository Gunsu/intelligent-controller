using IC.UI.Views;

namespace IC.UI.PresentationModels
{
	public interface IProjectExplorerPresentationModel
	{
		IProjectExplorerView View { get; set; }

		string ProjectName { get; }
	}
}
