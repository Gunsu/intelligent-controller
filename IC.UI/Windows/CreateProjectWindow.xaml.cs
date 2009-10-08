using System.IO;
using System.Windows;
using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Windows;
using Microsoft.Practices.Composite.Events;
using Microsoft.Win32;

namespace IC.UI.Windows
{
	/// <summary>
	/// Логика взаимодействия для CreateProjectWindow.xaml
	/// </summary>
	public partial class CreateProjectWindow : Window, ICreateProjectWindow
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly IProjectProcesses _projectProcesses;

		public CreateProjectWindow(IEventAggregator eventAggregator, IProjectProcesses projectProcesses)
		{
			InitializeComponent();
			_eventAggregator = eventAggregator;
			_projectProcesses = projectProcesses;
		}

		private void ChoosePath_Click(object sender, RoutedEventArgs e)
		{
			var fileDialog = new SaveFileDialog();
			fileDialog.AddExtension = true;
			fileDialog.DefaultExt = "prj";
			var result = fileDialog.ShowDialog();
			if (result == true)
			{
				ProjectPath.Text = fileDialog.FileName;
			}
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(ProjectName.Text))
			{
				MessageBox.Show("Необходимо указать название проекта");
				return;
			}

			if (string.IsNullOrEmpty(ProjectPath.Text))
			{
				MessageBox.Show("Необходимо указать путь к файлу проекта");
				return;
			}

			IProject project = _projectProcesses.Create(ProjectName.Text, ProjectPath.Text);
			_eventAggregator.GetEvent<ProjectCreatedEvent>().Publish(project);
			this.Close();
		}
	}
}
