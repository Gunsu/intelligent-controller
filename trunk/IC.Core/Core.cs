using Microsoft.Practices.Unity;
using IC.CoreInterfaces.Processes;
using IC.Core.Processes;

namespace IC.Core
{
	public sealed class Core
	{
		private readonly IUnityContainer _container;

		public void Initialize()
		{
			_container.RegisterType<IBlockTypesProcesses, BlockTypesProcesses>(new ContainerControlledLifetimeManager(),
			                                                                   new InjectionMember[]
			                                                                   	{
			                                                                   		new InjectionConstructor(
			                                                                   			Constants.BLOCK_TYPES_XML_PATH)
			                                                                   	});
		}

		public Core(IUnityContainer container)
		{
			_container = container;
		}
	}
}
