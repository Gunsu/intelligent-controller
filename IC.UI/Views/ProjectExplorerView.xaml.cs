using System.Windows.Controls;

using ValidationAspects;
using IC.UI.PresentationModels;

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

#if Design
			_projectsListBox.Items.Add("Schema1");
			_projectsListBox.Items.Add("Schema2");
			_projectsListBox.Items.Add("Schema3");
#endif
		}

		public IProjectExplorerPresentationModel Model
		{
			get { return DataContext as IProjectExplorerPresentationModel; }
			set { DataContext = value; }
		}
	}
}
