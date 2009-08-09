using IC.Modules.Toolbox.Interfaces.PresentationModels;

namespace IC.Modules.Toolbox.Interfaces.Views
{
	public interface IToolboxView
	{
		IToolboxPresentationModel Model { get; set; }
	}
}
