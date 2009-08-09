using System.ComponentModel;
using System.Windows.Input;
using IC.CoreInterfaces.Objects;
using IC.Modules.Menu.Interfaces.PresentationModels;
using IC.Modules.Menu.Interfaces.Views;
using IC.Modules.Menu.Properties;
using IC.UI.Infrastructure.Events;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Composite.Presentation.Events;

namespace IC.Modules.Menu.PresentationModels
{
	public sealed class MenuPresentationModel : IMenuPresentationModel, INotifyPropertyChanged
	{
		private readonly IEventAggregator _eventAggregator;
		private ICommand _createProjectCommand;
		private string _createMenuItemHeader;
		private string _createProjectMenuItemHeader;


		public IMenuView View { get; private set; }

		public ICommand CreateProjectCommand
		{
			get
			{
				return _createProjectCommand;
			}
			private set
			{
				_createProjectCommand = value;
				OnPropertyChanged("CreateProjectCommand");
			}
		}

		private string CreateMenuItemHeader
		{
			get { return _createMenuItemHeader; }
			set
			{
				_createMenuItemHeader = value;
				OnPropertyChanged("CreateMenuItemHeader");
			}
		}

		private string CreateProjectMenuItemHeader
		{
			get { return _createProjectMenuItemHeader; }
			set
			{
				_createProjectMenuItemHeader = value;
				OnPropertyChanged("CreateProjectMenuItemHeader");
			}
		}

		
		public event PropertyChangedEventHandler PropertyChanged = delegate { };


		private void CreateProject(IProject project)
		{
			
		}

		private void ProjectOpened(IProject project)
		{
		}

		private void ProjectClosed(IProject project)
		{
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		public MenuPresentationModel(IMenuView view, IEventAggregator eventAggregator)
		{
			View = view;
			View.Model = this;

			CreateMenuItemHeader = Resources.CreateMenuItemHeader;
			CreateProjectMenuItemHeader = Resources.CreateProjectMenuItemHeader;

			_eventAggregator = eventAggregator;
			_eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe(ProjectOpened, ThreadOption.UIThread);
			_eventAggregator.GetEvent<ProjectClosedEvent>().Subscribe(ProjectClosed, ThreadOption.UIThread);

			CreateProjectCommand = new DelegateCommand<IProject>(CreateProject);
		}
	}
}
