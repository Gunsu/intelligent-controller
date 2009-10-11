using System.Windows;
using IC.Core.Abstract;
using IC.Core.Entities;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Windows;
using Microsoft.Practices.Composite.Events;

namespace IC.UI.Windows
{
	/// <summary>
	/// Логика взаимодействия для CreateProjectWindow.xaml
	/// </summary>
	public partial class CreateProjectWindow : Window, ICreateProjectWindow
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly IProjectsRepository _projectsRepository;

		public CreateProjectWindow(IEventAggregator eventAggregator, IProjectsRepository projectsRepository)
		{
			InitializeComponent();
			_eventAggregator = eventAggregator;
			_projectsRepository = projectsRepository;
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(ProjectName.Text))
			{
				MessageBox.Show("Необходимо указать название проекта");
				return;
			}

			Project project = _projectsRepository.Create(ProjectName.Text);
			_eventAggregator.GetEvent<ProjectCreatedEvent>().Publish(project);
			_eventAggregator.GetEvent<SchemaCreatedEvent>().Publish(project.Schemas[0]);
			this.Close();
		}
	}
}
