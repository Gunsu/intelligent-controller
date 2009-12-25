using System;
using System.Windows.Input;
using IC.Core.Entities.UI;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Menu;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace IC.PresentationModels
{
	public sealed class MenuPresentationModel : BasePresentationModel, IMenuPresentationModel
	{
		#region Menu items commands and properties

		public ICommand CreateProjectCommand { get; set; }

		public ICommand CreateSchemaCommand { get; set; }
		private bool _createSchemaCommandIsEnabled;
		public bool CreateSchemaCommandIsEnabled
		{
			get { return _createSchemaCommandIsEnabled; }
			set
			{
				_createSchemaCommandIsEnabled = value;
				OnPropertyChanged("CreateSchemaCommandIsEnabled");
			}
		}

		public ICommand OpenProjectCommand { get; set; }

		public ICommand SaveProjectCommand { get; set; }
		private bool _saveProjectCommandIsEnabled;
		public bool SaveProjectCommandIsEnabled
		{
			get { return _saveProjectCommandIsEnabled; }
			set
			{
				_saveProjectCommandIsEnabled = value;
				OnPropertyChanged("SaveProjectCommandIsEnabled");
			}
		}

		#endregion

		#region Methods for publishing events

		private void CreateProject(EventArgs args)
		{
			_eventAggregator.GetEvent<ProjectCreatingEvent>().Publish(args);
			SaveProjectCommandIsEnabled = true;
		}

		private void CreateSchema(EventArgs args)
		{
			_eventAggregator.GetEvent<SchemaCreatingEvent>().Publish(args);
		}

		private void OpenProject(EventArgs args)
		{
			_eventAggregator.GetEvent<ProjectOpeningEvent>().Publish(args);
			SaveProjectCommandIsEnabled = true;
		}

		private void SaveProject(EventArgs args)
		{
			_eventAggregator.GetEvent<ProjectSavingEvent>().Publish(args);
		}

		#endregion

		#region Methods for handling subscribed events

		private void ProjectOpened(Project project)
		{
			CreateSchemaCommandIsEnabled = true;
		}

		private void ProjectClosed(Project project)
		{
			throw new System.NotImplementedException();
		}

		private void ProjectCreated(Project project)
		{
			CreateSchemaCommandIsEnabled = true;
		}

		#endregion

		public MenuPresentationModel(IEventAggregator eventAggregator)
			: base(eventAggregator)
		{
			_eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe(ProjectOpened);
			_eventAggregator.GetEvent<ProjectClosedEvent>().Subscribe(ProjectClosed);
			_eventAggregator.GetEvent<ProjectCreatedEvent>().Subscribe(ProjectCreated);

			CreateProjectCommand = new DelegateCommand<EventArgs>(CreateProject);
			CreateSchemaCommand = new DelegateCommand<EventArgs>(CreateSchema);
			CreateSchemaCommandIsEnabled = false;
			OpenProjectCommand = new DelegateCommand<EventArgs>(OpenProject);
			SaveProjectCommand = new DelegateCommand<EventArgs>(SaveProject);
			SaveProjectCommandIsEnabled = false;
		}
	}
}