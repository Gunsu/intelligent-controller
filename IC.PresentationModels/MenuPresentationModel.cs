using System;
using System.Windows.Input;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Menu;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace IC.PresentationModels
{
	public sealed class MenuPresentationModel : BasePresentationModel, IMenuPresentationModel
	{       
		#region Commands
		
		private ICommand _createProjectCommand;
		private ICommand _createSchemaCommand;


		public ICommand CreateProjectCommand
		{
			get { return _createProjectCommand; }
			set
			{
				_createProjectCommand = value;
				OnPropertyChanged("CreateProjectCommand");
			}
		}

		public ICommand CreateSchemaCommand
		{
			get { return _createSchemaCommand; }
			set
			{
				_createSchemaCommand = value;
				OnPropertyChanged("CreateSchemaCommand");
			}
		}

		#endregion

		#region Methods for publishing events

		private void CreateProject(EventArgs args)
		{
			_eventAggregator.GetEvent<ProjectCreatingEvent>().Publish(EventArgs.Empty);
		}

		private void CreateSchema(EventArgs args)
		{
			_eventAggregator.GetEvent<SchemaCreatingEvent>().Publish(EventArgs.Empty);
		}

		#endregion

		#region Methods for handling subscribed events

		private void ProjectOpened(IProject project)
		{
			throw new System.NotImplementedException();
		}

		private void ProjectClosed(IProject project)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		public MenuPresentationModel(IEventAggregator eventAggregator)
			: base(eventAggregator)
		{
			_eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe(ProjectOpened, ThreadOption.UIThread);
			_eventAggregator.GetEvent<ProjectClosedEvent>().Subscribe(ProjectClosed, ThreadOption.UIThread);

			CreateProjectCommand = new DelegateCommand<EventArgs>(CreateProject);
			CreateSchemaCommand = new DelegateCommand<EventArgs>(CreateSchema);
		}
	}
}
