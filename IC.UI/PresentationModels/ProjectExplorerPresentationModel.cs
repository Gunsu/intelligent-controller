using IC.UI.Views;
namespace IC.UI.PresentationModels
{
	public sealed class ProjectExplorerPresentationModel : IProjectExplorerPresentationModel
	{
		#region Члены IProjectExplorerPresentationModel

		public IProjectExplorerView View { get; set; }

		public string ProjectName
		{
			get
			{
				return "Project Explorer - MyProject";
			}
		}

		#endregion
	}
}
