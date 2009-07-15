using System;

namespace IC.Presenters.ViewInterfaces
{
	public interface IMenuView : IBaseView
	{
		event EventHandler SaveProjectEventHandler;
	}
}
