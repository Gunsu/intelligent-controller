using Microsoft.Practices.Unity;

using IC.Core.Processes;
using IC.Core.Tests.Mocks;
using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;

using NUnit.Framework;

namespace IC.Core.Tests.Processes
{
	[TestFixture]
	public sealed class ProjectProcessesTest
	{
		private IProjectProcesses _projectProcesses;

		[SetUp]
		public void SetUpTests()
		{
			var container = new UnityContainer();
			container.RegisterType<IProjectProcesses, ProjectProcesses>();
			_projectProcesses = container.Resolve<IProjectProcesses>();
		}

		[Test]
		public void Save_GenericTest()
		{
			IProject mockProject = new MockProject();
			var result = _projectProcesses.Save(mockProject);

			Assert.IsTrue(result.Result);
		}
	}
}
