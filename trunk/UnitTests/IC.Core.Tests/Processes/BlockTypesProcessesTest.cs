using System.Collections;
using System.Xml;
using IC.Core.Processes;
using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using System.Collections.Generic;

namespace IC.Core.Tests.Processes
{
	[TestFixture]
	public class BlockTypesProcessesTest
	{
		private IUnityContainer _container;

		[SetUp]
		public void SetUpTests()
		{
			_container = new UnityContainer();
			var core = new Core(_container);
			core.Initialize();
			//_container.RegisterType<IBlockTypesProcesses, BlockTypesProcesses>(new ContainerControlledLifetimeManager(),
			//                                                                   new InjectionMember[]
			//                                                                    {
			//                                                                        new InjectionConstructor(
			//                                                                            "BlockTypes.xml")
			//                                                                    });
		}

		[Test]
		public void LoadBlockTypesFromFile_GenericTest()
		{
			var blockTypesProcesses = _container.Resolve<IBlockTypesProcesses>();
			IList<IBlockType> blockTypes = blockTypesProcesses.LoadBlockTypesFromFile();
			
			Assert.AreEqual(31, blockTypes.Count);
			Assert.AreEqual(2, blockTypes[0].ID);
			Assert.AreEqual("Plus", blockTypes[0].Name);
			
			Assert.AreEqual(2, blockTypes[0].InputPoints.Count);
			Assert.AreEqual("V1", blockTypes[0].InputPoints[0].Name);
			Assert.AreEqual(1, blockTypes[0].InputPoints[0].Size);
			Assert.AreEqual("V2", blockTypes[0].InputPoints[1].Name);
			Assert.AreEqual(1, blockTypes[0].InputPoints[1].Size);

			Assert.AreEqual(1, blockTypes[0].OutputPoints.Count);
			Assert.AreEqual("Res", blockTypes[0].OutputPoints[0].Name);
			Assert.AreEqual(1, blockTypes[0].OutputPoints[0].Size);


			Assert.AreEqual(32, blockTypes[30].ID);
			Assert.AreEqual("Const3", blockTypes[30].Name);
			
			Assert.AreEqual(0, blockTypes[30].InputPoints.Count);
			
			Assert.AreEqual(1, blockTypes[30].OutputPoints.Count);
			Assert.AreEqual("C3", blockTypes[30].OutputPoints[0].Name);
			Assert.AreEqual(1, blockTypes[30].OutputPoints[0].Size);
		}
	}
}
