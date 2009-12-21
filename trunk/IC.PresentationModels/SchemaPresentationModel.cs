using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using IC.Core.Entities.UI;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Schema;
using Microsoft.Practices.Composite.Events;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.PresentationModels
{
	[Validate]
	public sealed class SchemaPresentationModel : BasePresentationModel, ISchemaPresentationModel
	{
		private Schema _currentSchema;
		
		/// <summary>
		/// Эта штука вынесена наружу, для того чтобы DesignerCanvas имела к ней доступ. Yep, it's a dirty hack.
		/// </summary>
		public IEventAggregator EventAggregator
		{
			get { return base._eventAggregator; }
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

		#region Methods for handling subscribed events

		private void OnCurrentSchemaChanged(Schema schema)
		{
			CurrentSchema = schema;
		}

		private void OnSchemaSaving(XElement uiSchema)
		{
			if (uiSchema != null && CurrentSchema != null)
			{
				CurrentSchema.CurrentUISchema = uiSchema;
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
