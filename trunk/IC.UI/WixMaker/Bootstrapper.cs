using System.Windows;

using IC.UI.WixProject;

using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;

namespace IC.UI.WixMaker
{
	public class Bootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell()
		{

			Shell shell = Container.Resolve<Shell>();

			shell.Show();

			return shell;

		}


		protected override IModuleEnumerator GetModuleEnumerator()
		{

			return new StaticModuleEnumerator().

				AddModule(typeof(WixProjectModule));

		}
	}
}
