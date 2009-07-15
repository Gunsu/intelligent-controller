using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;

using IC.Core;
using IC.Presenters.ViewInterfaces;

namespace IC.Presenters
{
	public sealed class SchemaPresenter : BasePresenter<ISchemaView>
	{
		public override void InitView()
		{
            var schemaView = _container.Resolve<ISchemaView>();
			_regionManager.AddToRegion(RegionNames.SchemaRegion,
														schemaView);
		}

		public SchemaPresenter(IUnityContainer container,
											IRegionManager regionManager,
											ISchemaView view)
			: base(container, regionManager, view)
		{
		}
	}
}
