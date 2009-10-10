using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using IC.CoreInterfaces.Processes;
using Microsoft.Practices.Composite.Events;

using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Schema;

using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.PresentationModels
{
	[Validate]
	public sealed class SchemaPresentationModel : BasePresentationModel, ISchemaPresentationModel
	{
		private readonly ISchemaProcesses _schemaProcesses;

		private ISchema _currentSchema;
		private ObservableCollection<IBlock> _blocks;
		private IBlock _currentBlock;

		public IEventAggregator EventAggregator
		{
			get { return base._eventAggregator; }
		}

		[NotNull]
		public ObservableCollection<IBlock> Blocks
		{
			get { return _blocks; }
			set
			{
				_blocks = value;
				OnPropertyChanged("Blocks");
			}
		}

		public ISchema CurrentSchema
		{
			get { return _currentSchema; }
			set
			{
				_currentSchema = value;
				OnPropertyChanged("CurrentSchema");
			}
		}

		public IBlock CurrentBlock
		{
			get { return _currentBlock; }
			set
			{
				_currentBlock = value;
				OnPropertyChanged("CurrentBlock");
			}
		}

		#region Methods for handling subscribed events

		private void OnCurrentSchemaChanged(ISchema schema)
		{
			_eventAggregator.GetEvent<SchemaSavingEvent>().Publish(null);
			CurrentSchema = schema;
		}

		private void OnSchemaSaving(XElement uiSchema)
		{
			if (uiSchema != null)
			{
				_schemaProcesses.Save(CurrentSchema, uiSchema);
				_eventAggregator.GetEvent<SchemaSavedEvent>().Publish(EventArgs.Empty);
			}
		}

		#endregion


        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
        	switch (e.PropertyName)
        	{
				case "CurrentBlock":
        			{
						
        				break;
        			}
				default:
        			{
        				break;
        			}
        	}
		}


		public SchemaPresentationModel([NotNull] IEventAggregator eventAggregator,
			                           [NotNull] ISchemaProcesses schemaProcesses)
			: base(eventAggregator)
		{
			_schemaProcesses = schemaProcesses;

			_eventAggregator.GetEvent<CurrentSchemaChangedEvent>().Subscribe(OnCurrentSchemaChanged);
			_eventAggregator.GetEvent<SchemaSavingEvent>().Subscribe(OnSchemaSaving);

			PropertyChanged += OnPropertyChanged;
		}
	}
}
