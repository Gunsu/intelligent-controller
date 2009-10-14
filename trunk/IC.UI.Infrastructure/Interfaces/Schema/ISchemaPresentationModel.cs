using Microsoft.Practices.Composite.Events;

namespace IC.UI.Infrastructure.Interfaces.Schema
{
	public interface ISchemaPresentationModel
	{
		/// <summary>
		/// Эта штука вынесена наружу, для того чтобы DesignerCanvas имела к ней доступ. Yep, it's a dirty hack.
		/// </summary>
		IEventAggregator EventAggregator { get; }
	}
}
