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
	    public string ProjectName
		{
			get { return "Обозреватель проектов"; }
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
				_eventAggregator.GetEvent<CurrentSchemaChangedEvent>().Publish(value);
            }
		}


		#region Methods for handling subscribed events

		private void OnSchemaCreated([NotNull] Schema schema)
		{
			SchemasListItems.Add(schema);
			CurrentSchemaItem = schema;
		}

		#endregion

		public ProjectExplorerPresentationModel([NotNull] IEventAggregator eventAggregator)
			: base(eventAggregator)
        {
			_eventAggregator.GetEvent<SchemaCreatedEvent>().Subscribe(OnSchemaCreated);
        }
    }
}
