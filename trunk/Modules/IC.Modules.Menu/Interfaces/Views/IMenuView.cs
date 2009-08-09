using IC.Modules.Menu.Interfaces.PresentationModels;

namespace IC.Modules.Menu.Interfaces.Views
{
	public interface IMenuView
	{
		IMenuPresentationModel Model { get; set; }
	}
}
