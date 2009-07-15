using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using System.Windows;

using IC.Presenters;
using IC.Presenters.ViewInterfaces;
using IC.UI.Views;

namespace IC.UI
{
	public sealed class Bootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell()
		{
			var shellPresenter = Container.Resolve<ShellPresenter>();
			IShellView view = shellPresenter.View;
			InitViews();
			view.ShowView();

			return view as DependencyObject;
		}

		protected override void ConfigureContainer()
		{
			Container.RegisterType<IShellView, ShellView>();
			Container.RegisterType<IMenuView, MenuView>();
			Container.RegisterType<IProjectExplorerView, ProjectExplorerView>();
			Container.RegisterType<ISchemaView, SchemaView>();
			Container.RegisterType<IStatusView, StatusView>();
			Container.RegisterType<IToolboxView, ToolboxView>();

			base.ConfigureContainer();
		}

		protected override IModuleCatalog GetModuleCatalog()
		{
			var moduleCatalog = new ModuleCatalog();
			return moduleCatalog;
		}

		private void InitViews()
		{
			Container.Resolve<MenuPresenter>().InitView();
			Container.Resolve<ProjectExplorerPresenter>().InitView();
			Container.Resolve<SchemaPresenter>().InitView();
			Container.Resolve<StatusPresenter>().InitView();
			Container.Resolve<ToolboxPresenter>().InitView();
		}
	}
}
