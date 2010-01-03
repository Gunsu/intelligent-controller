using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IC.Core.Entities.UI;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.ProjectExplorer;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.PresentationModels
{
	[Validate]
	public sealed class ProjectExplorerPresentationModel : BasePresentationModel, IProjectExplorerPresentationModel
	{
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

		public ICommand CreateSchemaCommand { get; set; }
		public ICommand RemoveSchemaCommand { get; set; }
		public bool RemoveSchemaCommandIsEnabled
		{
			get { return CurrentSchemaItem != null; }
		}

		#region Methods for handling subscribed events

		private void OnProjectCreated([NotNull] Project project)
		{
			SchemasListItems = new ObservableCollection<Schema>(project.Schemas);
			_currentProject = project;
		}

		private void OnProjectOpened([NotNull] Project project)
		{
			SchemasListItems = new ObservableCollection<Schema>(project.Schemas);
			_currentProject = project;
		}

		private void OnSchemaCreated([NotNull] Schema schema)
		{
			SchemasListItems = new ObservableCollection<Schema>(_currentProject.Schemas);
			CurrentSchemaItem = schema;
		}

		#endregion

		#region Methods for publishing events

		private void CreateSchema(EventArgs args)
		{
			_eventAggregator.GetEvent<SchemaCreatingEvent>().Publish(args);
		}

		private void RemoveSchema(EventArgs args)
		{
			SchemasListItems.Remove(CurrentSchemaItem);
		}

		#endregion

		public ProjectExplorerPresentationModel([NotNull] IEventAggregator eventAggregator)
			: base(eventAggregator)
        {
			_eventAggregator.GetEvent<ProjectCreatedEvent>().Subscribe(OnProjectCreated);
			_eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe(OnProjectOpened);
			_eventAggregator.GetEvent<SchemaCreatedEvent>().Subscribe(OnSchemaCreated);

			CreateSchemaCommand = new DelegateCommand<EventArgs>(CreateSchema);
			RemoveSchemaCommand = new DelegateCommand<EventArgs>(RemoveSchema);
        }
    }
}
