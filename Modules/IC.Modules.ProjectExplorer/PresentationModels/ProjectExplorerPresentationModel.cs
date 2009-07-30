using System.Collections.ObjectModel;
using System.ComponentModel;
using IC.CoreInterfaces.Objects;
using IC.Modules.ProjectExplorer.Interfaces.PresentationModels;
using IC.Modules.ProjectExplorer.Interfaces.Views;
using IC.UI.Infrastructure.Events;
using Microsoft.Practices.Composite.Events;

namespace IC.Modules.ProjectExplorer.PresentationModels
{
	public sealed class ProjectExplorerPresentationModel : IProjectExplorerPresentationModel, INotifyPropertyChanged
	{
		private readonly IEventAggregator _eventAggregator;
	    private ObservableCollection<ISchema> _schemasListItems;
		private ISchema _currentSchemaItem;

		#region Члены IProjectExplorerPresentationModel

		public IProjectExplorerView View { get; private set; }

		public string ProjectName
		{
			get { return "Project Explorer - MyProject"; }
		}

		public ObservableCollection<ISchema> SchemasListItems
		{
			get { return _schemasListItems; }
			set
			{
				_schemasListItems = value;
				OnPropertyChanged("SchemasListItems");
			}
		}

		public ISchema CurrentSchemaItem
	    {
            get { return _currentSchemaItem; }
            set
            {
                _currentSchemaItem = value;
                OnPropertyChanged("CurrentSchemaItem");
				_eventAggregator.GetEvent<CurrentSchemaChangedEvent>().Publish(value);
            }
	    }

		#endregion

        #region Члены INotifyPropertyChanged

	    public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ProjectExplorerPresentationModel(IProjectExplorerView view, IEventAggregator eventAggregator)
        {
            View = view;
            view.Model = this;
        	_eventAggregator = eventAggregator;
        }
    }
}
