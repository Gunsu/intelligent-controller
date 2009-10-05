using System;
using System.Windows.Input;
using System.Xml.Linq;
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

		public ICommand CreateProjectCommand { get; set; }

		public ICommand CreateSchemaCommand { get; set; }

		public ICommand SaveProjectCommand { get; set; }

		public ICommand SaveSchemaCommand { get; set; }

		#endregion

		#region Methods for publishing events

		private void CreateProject(EventArgs args)
		{
			_eventAggregator.GetEvent<ProjectCreatingEvent>().Publish(args);
		}

		private void CreateSchema(EventArgs args)
		{
			_eventAggregator.GetEvent<SchemaCreatingEvent>().Publish(args);
		}

		private void SaveProject(EventArgs args)
		{
			_eventAggregator.GetEvent<ProjectSavingEvent>().Publish(args);
		}

		private void SaveSchema(EventArgs args)
		{
			_eventAggregator.GetEvent<SchemaSavingEvent>().Publish(null);
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
			SaveProjectCommand = new DelegateCommand<EventArgs>(SaveProject);
			SaveSchemaCommand = new DelegateCommand<EventArgs>(SaveSchema);
		}
	}
}