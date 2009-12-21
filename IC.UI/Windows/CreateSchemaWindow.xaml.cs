using System.Windows;
using IC.Core.Entities.UI;
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
		
		private Project _currentProject;

		public CreateSchemaWindow([NotNull] IEventAggregator eventAggregator)
		{
			InitializeComponent();
			_eventAggregator = eventAggregator;
		}

		private void Create_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(SchemaName.Text))
			{
				MessageBox.Show("Необходимо указать название схемы");
				return;
			}

			Schema schema = _currentProject.AddSchema(SchemaName.Text);
			_eventAggregator.GetEvent<SchemaCreatedEvent>().Publish(schema);
			this.Close();
		}

		public bool? ShowDialog([NotNull] Project project)
		{
			_currentProject = project;
			return base.ShowDialog();
		}
	}
}
