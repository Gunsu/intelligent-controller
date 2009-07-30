using System.Collections.ObjectModel;
using System.ComponentModel;
using IC.CoreInterfaces.Objects;
using IC.Modules.Toolbox.Interfaces.PresentationModels;
using IC.Modules.Toolbox.Interfaces.Services;
using IC.Modules.Toolbox.Interfaces.Views;
using IC.Modules.Toolbox.Properties;
using IC.UI.Infrastructure.Events;
using Microsoft.Practices.Composite.Events;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.Modules.Toolbox.PresentationModels
{
	[Validate]
	public sealed class ToolboxPresentationModel : IToolboxPresentationModel, INotifyPropertyChanged
	{
		private ObservableCollection<IBlockType> _blockTypes;
		private IEventAggregator _eventAggregator;
		private string _header;
		private IBlockType _currentBlockType;

		public ObservableCollection<IBlockType> BlockTypes
		{
			get { return _blockTypes; }
			set
			{
				_blockTypes = value;
				OnPropertyChanged("BlockTypes");
			}
		}

		public IBlockType CurrentBlockType
		{
			get { return _currentBlockType; }
			set
			{
				_currentBlockType = value;
				_eventAggregator.GetEvent<CurrentBlockTypeChangedEvent>().Publish(_currentBlockType);
				OnPropertyChanged("CurrentBlockType");
			}
		}

		public string Header
		{
			get { return _header; }
			set
			{
				_header = value;
				OnPropertyChanged("Header");
			}
		}

		#region IToolboxPresentationModel members

		public IToolboxView View { get; private set; }

		#endregion

		private void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#region INotifyPropertyChanged members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public ToolboxPresentationModel([NotNull] IToolboxView view,
			                            [NotNull] IBlockTypesService blockTypesService,
			                            [NotNull] IEventAggregator eventAggregator)
		{
			View = view;
			View.Model = this;
			BlockTypes = blockTypesService.RetreiveBlockTypes();
			_eventAggregator = eventAggregator;
			Header = Resources.Header; 
		}
	}
}
