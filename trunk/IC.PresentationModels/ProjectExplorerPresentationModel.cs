using System.Collections.ObjectModel;
using IC.CoreInterfaces.Objects;
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

		private ObservableCollection<ISchema> _schemasListItems;
		public ObservableCollection<ISchema> SchemasListItems
		{
			get { return _schemasListItems; }
			set
			{
				_schemasListItems = value;
				OnPropertyChanged("SchemasListItems");
			}
		}

		private ISchema _currentSchemaItem;
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


		#region Methods for handling subscribed events

		private void OnSchemaCreated([NotNull] ISchema schema)
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
