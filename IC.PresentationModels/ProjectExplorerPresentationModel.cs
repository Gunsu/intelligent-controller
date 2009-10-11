using System.Collections.ObjectModel;
using IC.Core.Entities;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.ProjectExplorer;
using Microsoft.Practices.Composite.Events;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.PresentationModels
{
	[Validate]
	public sealed class ProjectExplorerPresentationModel : BasePresentationModel, IProjectExplorerPresentationModel
	{
		private string _header;
		public string Header
		{
			get { return _header; }
			set
			{
				_header = value;
				OnPropertyChanged("Header");
			}
		}

		private ObservableCollection<Schema> _schemasListItems;
		public ObservableCollection<Schema> SchemasListItems
		{
			get { return _schemasListItems; }
			set
			{
				_schemasListItems = value;
				OnPropertyChanged("SchemasListItems");
			}
		}

		private Schema _currentSchemaItem;
		public Schema CurrentSchemaItem
	    {
            get { return _currentSchemaItem; }
            set
            {
                _currentSchemaItem = value;
                OnPropertyChanged("CurrentSchemaItem");
				_eventAggregator.GetEvent<CurrentSchemaChangingEvent>().Publish(value);
            }
		}

		private Project _currentProject;

		#region Methods for handling subscribed events

		private void OnProjectCreated([NotNull] Project project)
		{
			Header = string.Format("Обозреватель проектов - {0}",
			                       project.Name);
			SchemasListItems = new ObservableCollection<Schema>(project.Schemas);
			_currentProject = project;
		}

		private void OnProjectOpened([NotNull] Project project)
		{
			Header = string.Format("Обозреватель проектов - {0}",
								   project.Name);
			SchemasListItems = new ObservableCollection<Schema>(project.Schemas);
			_currentProject = project;
		}

		private void OnSchemaCreated([NotNull] Schema schema)
		{
			SchemasListItems = new ObservableCollection<Schema>(_currentProject.Schemas);
			CurrentSchemaItem = schema;
		}

		#endregion

		public ProjectExplorerPresentationModel([NotNull] IEventAggregator eventAggregator)
			: base(eventAggregator)
        {
			Header = "Обозреватель проектов";

			_eventAggregator.GetEvent<ProjectCreatedEvent>().Subscribe(OnProjectCreated);
			_eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe(OnProjectOpened);
			_eventAggregator.GetEvent<SchemaCreatedEvent>().Subscribe(OnSchemaCreated);
        }
    }
}
