using System.Xml.Linq;
using Microsoft.Practices.Unity;

using IC.Core.Processes;
using IC.Core.Tests.Mocks;
using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;

using NUnit.Framework;

namespace IC.Core.Tests.Processes
{
	[TestFixture]
	public sealed class SchemaProcessesTest
	{
		private ISchemaProcesses _schemaProcesses;

		[SetUp]
		public void SetUpTests()
		{
			var container = new UnityContainer();
			container.RegisterType<ISchemaProcesses, SchemaProcesses>();
			_schemaProcesses = container.Resolve<ISchemaProcesses>();
		}

		[Test]
		public void Compile_GenericTest()
		{
			ISchema mockSchema = new MockSchema();
			var result = _schemaProcesses.Compile(mockSchema);

			Assert.IsTrue(result.Result);
		}

		[Test]
		public void Validate_GenericTest()
		{
			ISchema mockSchema = new MockSchema();
			var result = _schemaProcesses.Validate(mockSchema);

			Assert.IsTrue(result.Result);
		}

		[Test]
		public void Save_GenericTest()
		{
			ISchema mockSchema = new MockSchema() {FilePath = "schema1.scm", UISchema = new XElement("root")};
			bool result = _schemaProcesses.Save(mockSchema, new XElement("root"));

			Assert.IsTrue(mockSchema.IsSaved);
			Assert.IsTrue(result);
		}
	}
}
