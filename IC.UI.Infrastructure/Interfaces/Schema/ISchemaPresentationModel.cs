using Microsoft.Practices.Composite.Events;

namespace IC.UI.Infrastructure.Interfaces.Schema
{
	public interface ISchemaPresentationModel
	{
		IEventAggregator EventAggregator { get; }
	}
}
