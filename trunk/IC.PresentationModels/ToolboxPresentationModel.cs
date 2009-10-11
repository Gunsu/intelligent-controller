using System.Collections.ObjectModel;
using IC.Core.Abstract;
using IC.Core.Entities;
using ValidationAspects;
using ValidationAspects.PostSharp;
using IC.UI.Infrastructure.Interfaces.Toolbox;
using Microsoft.Practices.Composite.Events;

namespace IC.PresentationModels
{
	[Validate]
	public sealed class ToolboxPresentationModel : BasePresentationModel, IToolboxPresentationModel
	{
		private ObservableCollection<BlockType> _blockTypes;

		public ObservableCollection<BlockType> BlockTypes
		{
			get { return _blockTypes; }
			set
			{
				_blockTypes = value;
				OnPropertyChanged("BlockTypes");
			}
		}

		public ToolboxPresentationModel([NotNull] IBlockTypesRepository blockTypesRepository,
										[NotNull] IEventAggregator eventAggregator)
			: base(eventAggregator)
		{
			var blockTypesList = blockTypesRepository.LoadBlockTypesFromFile();
			BlockTypes = new ObservableCollection<BlockType>(blockTypesList);
		}
	}
}
