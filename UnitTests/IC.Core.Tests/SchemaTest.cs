using IC.Core.Entities;
using NUnit.Framework;
using IC.Core.Enums;

namespace IC.Core.Tests
{
	[TestFixture]
	public class SchemaTest
	{
		[Test]
		public void Compile_Should_Make_Correct_Result()
		{
			var project = new Project();
			var schema = project.AddSchema("AdcConfig");
            AddBlocks(schema);
            AddBlockConnectionPoints(schema);
            AddOutputsToBlockConnectionPoints(schema);
			short pos = 0;
			project.Schemas[0].Compile(ref pos);
		}

		private void AddBlocks(Schema schema)
		{
			var inBlockType = new BlockType(-1, "In");
			var outBlockType = new BlockType(-1, "Out");
            var sumBlockType = new BlockType(2, "Sum");

			var block0 = new InputCommandBlock() {BlockType = inBlockType, Mask = "_"};
            var block1 = new OutputCommandBlock() { BlockType = outBlockType, Mask = "_" };
            var block2 = new InputCommandBlock() { BlockType = inBlockType, Mask = "_" };
			var block3 = new Block() {BlockType = sumBlockType};
		}

		private void AddBlockConnectionPoints(Schema schema)
		{
            schema.Blocks[0].AddInputPoint(new BlockConnectionPoint() { Size = -1, Name = "" });
            schema.Blocks[0].AddOutputPoint(new BlockConnectionPoint() { Size = -1, Name = "" });
            schema.Blocks[0].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "" });

            schema.Blocks[1].AddInputPoint(new BlockConnectionPoint() { Size = -2, Name = "" });
            schema.Blocks[1].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "" });
            schema.Blocks[1].AddOutputPoint(new BlockConnectionPoint() { Size = -2, Name = "" });

            schema.Blocks[2].AddInputPoint(new BlockConnectionPoint() { Size = -1, Name = "" });
            schema.Blocks[2].AddOutputPoint(new BlockConnectionPoint() { Size = -1, Name = "" });
            schema.Blocks[2].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "" });

            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "V1" });
            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "V2" });
            schema.Blocks[3].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "Res" });
		}

        private void AddOutputsToBlockConnectionPoints(Schema schema)
        {
            schema.Blocks[0].OutputPoints[0].Outputs.Add(schema.Blocks[2].InputPoints[0]);
            schema.Blocks[0].OutputPoints[1].Outputs.Add(schema.Blocks[3].InputPoints[0]);

            schema.Blocks[2].OutputPoints[1].Outputs.Add(schema.Blocks[3].InputPoints[1]);

            schema.Blocks[3].OutputPoints[0].Outputs.Add(schema.Blocks[1].InputPoints[1]);
        }
	}
}
