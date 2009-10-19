using IC.Core.Entities;
using NUnit.Framework;

namespace IC.Core.Tests
{
	[TestFixture]
	public class SchemaTest
	{
		[Test]
		public void Compile_Should_Make_Correct_Result()
		{
			var project = new Project();
			project.AddSchema("");
			short pos = 0;
			project.Schemas[0].Compile(ref pos);
		}
	}
}
