using System.Collections.ObjectModel;
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
		private ISchema _currentSchema;
		private ObservableCollection<IBlock> _blocks;

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

		#region Methods for handling subscribed events

		private void OnCurrentSchemaChanged(ISchema schema)
		{
			throw new System.NotImplementedException();
		}

		#endregion

		public SchemaPresentationModel([NotNull] IEventAggregator eventAggregator)
			: base(eventAggregator)
		{
			_eventAggregator.GetEvent<CurrentSchemaChangedEvent>().Subscribe(OnCurrentSchemaChanged);
		}
	}
}
