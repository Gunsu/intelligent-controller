using IC.UI.Infrastructure.Interfaces.ProjectExplorer;

using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для ProjectExplorerView.xaml
	/// </summary>
	[Validate]
	public partial class ProjectExplorerView : IProjectExplorerView
	{
		public ProjectExplorerView()
		{
			InitializeComponent();
		}

		[NotNull]
		public IProjectExplorerPresentationModel Model
		{
			get { return DataContext as IProjectExplorerPresentationModel; }
			set { DataContext = value; }
		}
	}
}
