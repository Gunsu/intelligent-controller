﻿using System.Collections.ObjectModel;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.ProjectExplorer;
using Microsoft.Practices.Composite.Events;

namespace IC.PresentationModels
{
	public sealed class ProjectExplorerPresentationModel : BasePresentationModel, IProjectExplorerPresentationModel
	{
	    private ObservableCollection<ISchema> _schemasListItems;
		private ISchema _currentSchemaItem;


		public string ProjectName
		{
			get { return "Обозреватель проектов"; }
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


        public ProjectExplorerPresentationModel(IEventAggregator eventAggregator)
			: base(eventAggregator)
        {
        }
    }
}
