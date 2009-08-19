using System;
using System.Windows.Input;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Menu;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using IC.PresentationModels.Properties;

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

		private ICommand CreateSchemaCommand
		{
			get { return _createSchemaCommand; }
			set
			{
				_createSchemaCommand = value;
				OnPropertyChanged("CreateSchemaCommand");
			}
		}

		#endregion

		#region Menu items headers

		private string _fileMenuItemHeader;
		private string _createMenuItemHeader;
		private string _createProjectMenuItemHeader;
		private string _createSchemaMenuItemHeader;


		public string FileMenuItemHeader
		{
			get { return _fileMenuItemHeader; }
			set
			{
				_fileMenuItemHeader = value;
				OnPropertyChanged("FileMenuItemHeader");
			}
		}

		public string CreateMenuItemHeader
		{
			get { return _createMenuItemHeader; }
			set
			{
				_createMenuItemHeader = value;
				OnPropertyChanged("CreateMenuItemHeader");
			}
		}

		public string CreateProjectMenuItemHeader
		{
			get { return _createProjectMenuItemHeader; }
			set
			{
				_createProjectMenuItemHeader = value;
				OnPropertyChanged("CreateProjectMenuItemHeader");
			}
		}

		public string CreateSchemaMenuItemHeader
		{
			get { return _createSchemaMenuItemHeader; }
			set
			{
				_createSchemaMenuItemHeader = value;
				OnPropertyChanged("CreateSchemaMenuItemHeader");
			}
		}

		#endregion

		#region Methods for publishing events

		private void CreateProject(EventArgs args)
		{
			_eventAggregator.GetEvent<ProjectCreatingEvent>().Publish(EventArgs.Empty);
		}

		private void CreateSchema(ISchema schema)
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
			FileMenuItemHeader = Resources.FileMenuItemHeader;
			CreateMenuItemHeader = Resources.CreateMenuItemHeader;
			CreateProjectMenuItemHeader = Resources.CreateProjectMenuItemHeader;
			CreateSchemaMenuItemHeader = Resources.CreateSchemaMenuItemHeader;

			_eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe(ProjectOpened, ThreadOption.UIThread);
			_eventAggregator.GetEvent<ProjectClosedEvent>().Subscribe(ProjectClosed, ThreadOption.UIThread);

			CreateProjectCommand = new DelegateCommand<EventArgs>(CreateProject);
			CreateSchemaCommand = new DelegateCommand<ISchema>(CreateSchema);
		}
	}
}
