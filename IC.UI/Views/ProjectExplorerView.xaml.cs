using IC.UI.Infrastructure.Interfaces.ProjectExplorer;

namespace IC.UI.Views
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
