using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using IC.CoreInterfaces.Processes;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;

using IC.Core.Objects;
using IC.CoreInterfaces.Enums;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Schema;
using Project.Utils.Common;

using ValidationAspects;
using ValidationAspects.PostSharp;
using System;

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
			throw new System.NotImplementedException();
		}

		private void OnSchemaSaving(EventArgs args)
		{
			_schemaProcesses.Save(_currentSchema);
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
