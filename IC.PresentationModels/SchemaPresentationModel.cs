using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
		#region View Event Handlers

		private bool _isDragging;

		public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_isDragging = true;
		}

		public void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			_isDragging = false;
		}

		public void OnMouseMove(object sender, MouseEventArgs e)
		{
			//if (_isDragging)
			//{
			//    if (CurrentBlock != null)
			//    {
			//        CurrentBlock.X = Convert.ToInt32(e.GetPosition(null).X);
			//        CurrentBlock.Y = Convert.ToInt32(e.GetPosition(null).Y);
			//    }
			//}
		}

		#endregion

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
#warning temp!
			Blocks = new ObservableCollection<IBlock>();
			var bt = new BlockType(10, "Lol");
			var b = new Block(bt, new Coordinates(50, 50), Orientation.Horizontal);
			b.X = 50;
			b.Y = 50;
			Blocks.Add(b);
			var b2 = new Block(bt, new Coordinates(250, 250), Orientation.Horizontal);
			b2.X = 250;
			b2.Y = 250;
			Blocks.Add(b2);
#warning temp!
			PropertyChanged += OnPropertyChanged;
		}
	}
}
