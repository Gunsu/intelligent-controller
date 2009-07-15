using IC.Presenters.ViewInterfaces;

namespace IC.Presenters
{
	public sealed class ShellPresenter
	{
		private readonly IShellView _view;

		public IShellView View
		{
			get { return _view; }
		}

		public ShellPresenter(IShellView view)
		{
			_view = view;
		}
	}
}
