using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using IC.Core.Entities;
using Microsoft.Practices.Composite.Events;

using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Schema;

using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.PresentationModels
{
	[Validate]
	public sealed class SchemaPresentationModel : BasePresentationModel, ISchemaPresentationModel
	{
		private Schema _currentSchema;
		private ObservableCollection<Block> _blocks;
		private Block _currentBlock;

		/// <summary>
		/// Эта штука вынесена наружу, для того чтобы DesignerCanvas имела к ней доступ. Yep, it's a dirty hack.
		/// </summary>
		public IEventAggregator EventAggregator
		{
			get { return base._eventAggregator; }
		}

		[NotNull]
		public ObservableCollection<Block> Blocks
		{
			get { return _blocks; }
			set
			{
				_blocks = value;
				OnPropertyChanged("Blocks");
			}
		}

		public Schema CurrentSchema
		{
			get { return _currentSchema; }
			set
			{
				_currentSchema = value;
				OnPropertyChanged("CurrentSchema");
			}
		}

		public Block CurrentBlock
		{
			get { return _currentBlock; }
			set
			{
				_currentBlock = value;
				OnPropertyChanged("CurrentBlock");
			}
		}

		#region Methods for handling subscribed events

		private void OnCurrentSchemaChanged(Schema schema)
		{
			CurrentSchema = schema;
		}

		private void OnSchemaSaving(XElement uiSchema)
		{
			if (uiSchema != null && CurrentSchema != null)
			{
				CurrentSchema.Save(uiSchema);
				_eventAggregator.GetEvent<SchemaSavedEvent>().Publish(EventArgs.Empty);
			}
		}

		#endregion

		public SchemaPresentationModel([NotNull] IEventAggregator eventAggregator)
			: base(eventAggregator)
		{
			_eventAggregator.GetEvent<CurrentSchemaChangedEvent>().Subscribe(OnCurrentSchemaChanged);
			_eventAggregator.GetEvent<SchemaSavingEvent>().Subscribe(OnSchemaSaving);
		}
	}
}
