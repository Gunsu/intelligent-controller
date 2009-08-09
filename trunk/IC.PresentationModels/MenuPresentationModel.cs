using System.Windows.Input;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Menu;

namespace IC.PresentationModels
{
	public sealed class MenuPresentationModel : BasePresentationModel, IMenuPresentationModel
	{

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

		
		private void CreateProject(IProject project)
		{
			
		}

		private void ProjectOpened(IProject project)
		{
		}

		private void ProjectClosed(IProject project)
		{
		}



		public MenuPresentationModel(IMenuView view, IEventAggregator eventAggregator)
			: base(eventAggregator)
		{
			View = view;
			View.Model = this;

			CreateMenuItemHeader = Resources.CreateMenuItemHeader;
			CreateProjectMenuItemHeader = Resources.CreateProjectMenuItemHeader;

			_eventAggregator.GetEvent<ProjectOpenedEvent>().Subscribe(ProjectOpened, ThreadOption.UIThread);
			_eventAggregator.GetEvent<ProjectClosedEvent>().Subscribe(ProjectClosed, ThreadOption.UIThread);

			CreateProjectCommand = new DelegateCommand<IProject>(CreateProject);
		}
	}
}
