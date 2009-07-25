using IC.Modules.ProjectExplorer;
using IC.UI.Views;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using System.Windows;

namespace IC.UI
{
	public sealed class Bootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell()
		{
			var shellPresenter = Container.Resolve<ShellPresenter>();
			IShellView view = shellPresenter.View;
			view.ShowView();

			return view as DependencyObject;
		}

		protected override void ConfigureContainer()
		{
			Container.RegisterType<IShellView, Shell>();
		
			base.ConfigureContainer();
		}

		protected override IModuleCatalog GetModuleCatalog()
		{
			var moduleCatalog = new ModuleCatalog();
			moduleCatalog.AddModule(typeof (ProjectExplorerModule));
			return moduleCatalog;
		}
	}
}
