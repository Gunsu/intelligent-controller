using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Practices.Composite.Events;

using IC.Core.Objects;
using IC.CoreInterfaces.Enums;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using IC.UI.Infrastructure.Interfaces.Schema;
using Project.Utils.Common;

using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.PresentationModels
{
	[Validate]
	public sealed class SchemaPresentationModel : BasePresentationModel, ISchemaPresentationModel
	{
		private ISchema _currentSchema;
		private ObservableCollection<IBlock> _blocks;
		private IBlock _currentBlock;

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


		public SchemaPresentationModel([NotNull] IEventAggregator eventAggregator)
			: base(eventAggregator)
		{
			_eventAggregator.GetEvent<CurrentSchemaChangedEvent>().Subscribe(OnCurrentSchemaChanged);
			Blocks = new ObservableCollection<IBlock>();
			var bt = new BlockType(10, "Lol");
			var b = new Block(bt, new Coordinates(50, 50), Orientation.Horizontal);
			PropertyChanged += OnPropertyChanged;
            Blocks.Add(b);
			var b2 = new Block(bt, new Coordinates(250, 250), Orientation.Horizontal);
			Blocks.Add(b2);
		}
	}
}
