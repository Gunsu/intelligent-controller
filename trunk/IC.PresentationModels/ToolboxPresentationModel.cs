using System.Collections.ObjectModel;
using IC.CoreInterfaces.Objects;
using IC.UI.Infrastructure.Events;
using ValidationAspects;
using ValidationAspects.PostSharp;
using IC.UI.Infrastructure.Interfaces.Toolbox;
using Microsoft.Practices.Composite.Events;
using IC.CoreInterfaces.Processes;

namespace IC.PresentationModels
{
	[Validate]
	public sealed class ToolboxPresentationModel : BasePresentationModel, IToolboxPresentationModel
	{
		private ObservableCollection<IBlockType> _blockTypes;
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


		public ToolboxPresentationModel([NotNull] IBlockTypesProcesses blockTypesProcesses,
										[NotNull] IEventAggregator eventAggregator)
			: base(eventAggregator)
		{
			var blockTypesList = blockTypesProcesses.LoadBlockTypesFromFile();
			BlockTypes = new ObservableCollection<IBlockType>(blockTypesList);
			Header = Resources.ToolboxHeader;
		}
	}
}
