using System.Xml.Linq;
using NUnit.Framework;

namespace IC.Core.Tests
{
	[TestFixture]
	public class UISchemaParserTest
	{
		[Test]
		public void Parse_Should_Make_Correct_Result()
		{
			var uiProject = new Entities.UI.Project();
			var uiSchema = uiProject.AddSchema("ExampleSchema");
			uiSchema.CurrentUISchema = XElement.Load("UISchemaExample.xml");

			var parser = new UISchemaParser();
			var compilationProject = parser.Parse(uiProject);

			Assert.IsNotNull(compilationProject);
		}
	}
}
