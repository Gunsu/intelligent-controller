using IC.Modules.ProjectExplorer.Interfaces.PresentationModels;
using IC.Modules.ProjectExplorer.Interfaces.Views;

namespace IC.Modules.ProjectExplorer.Views
{
	/// <summary>
	/// Логика взаимодействия для ProjectExplorerView.xaml
	/// </summary>
	public partial class ProjectExplorerView : IProjectExplorerView
	{
		public ProjectExplorerView()
		{
			InitializeComponent();
		}

		public IProjectExplorerPresentationModel Model
		{
			get { return DataContext as IProjectExplorerPresentationModel; }
			set { DataContext = value; }
		}
	}
}
