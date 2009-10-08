using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Practices.Unity;

using IC.Core.Processes;
using IC.Core.Tests.Mocks;
using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using NUnit.Framework;
using Project.Utils.Common;

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
		public void Open_GenericTest()
		{
			string pathToMockProject = string.Empty;
			ProcessResult<IProject> result = _projectProcesses.Open(pathToMockProject);

			Assert.IsNotNull(result.Result);
		}

		[Test]
		public void Save_GenericTest()
		{
			const string path = "testproject.prj";

			var mockSchema1 = new MockSchema();
			mockSchema1.IsSaved = true;
			mockSchema1.Name = "schema1";
			mockSchema1.UISchema = (new XElement("root"));

			var mockSchema2 = new MockSchema();
			mockSchema2.IsSaved = true;
			mockSchema2.Name = "schema2";

			var mockProject = new MockProject();
			mockProject.IsSaved = false;
			mockProject.Path = path;
			mockProject.Schemas = new List<ISchema>() {mockSchema1, mockSchema2};

			ProcessResult<List<ISchema>> result = _projectProcesses.Save(mockProject);

			Assert.IsTrue(result.NoErrors);
		}

		[Test]
		public void Create_GenericTest()
		{
			var result = _projectProcesses.Create(string.Empty, string.Empty);

			Assert.IsNotNull(result);
		}

		[Test]
		public void AddSchemaTest()
		{
			bool result = _projectProcesses.AddSchema(null, null);

			Assert.IsTrue(result);
		}
	}
}