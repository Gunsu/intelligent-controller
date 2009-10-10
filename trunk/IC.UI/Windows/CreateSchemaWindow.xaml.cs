using System.Windows;
using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Windows;
using Microsoft.Practices.Composite.Events;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.UI.Windows
{
	/// <summary>
	/// Interaction logic for CreateSchemaWindow.xaml
	/// </summary>
	[Validate]
	public partial class CreateSchemaWindow : Window, ICreateSchemaWindow
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly ISchemaProcesses _schemaProcesses;
		private readonly IProjectProcesses _projectProcesses;
		private IProject _currentProject;

		public CreateSchemaWindow([NotNull] IEventAggregator eventAggregator,
			                      [NotNull] ISchemaProcesses schemaProcesses,
			                      [NotNull] IProjectProcesses projectProcesses)
		{
			InitializeComponent();
			_eventAggregator = eventAggregator;
			_schemaProcesses = schemaProcesses;
			_projectProcesses = projectProcesses;
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(SchemaName.Text))
			{
				MessageBox.Show("Необходимо указать название схемы");
				return;
			}

			ISchema schema = _schemaProcesses.Create(SchemaName.Text);
			_projectProcesses.AddSchema(_currentProject, schema);
			_eventAggregator.GetEvent<SchemaCreatedEvent>().Publish(schema);
			this.Close();
		}

		public bool? ShowDialog([NotNull] IProject project)
		{
			_currentProject = project;
			return base.ShowDialog();
		}
	}
}
