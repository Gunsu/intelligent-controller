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
			
			// создание блоков
			

			short pos = 0;
			project.Schemas[0].Compile(ref pos);
		}

		private void AddBlocks(Schema schema)
		{
			var block0 = new InputCommandBlock() { BlockType = new BlockType(-1, "In"),
												   ObjectType = ObjectType.InputCommandBlock,
												   Mask = new string ('_', 8) };
			schema.Blocks.Add(block0);

			var block1 = new InputCommandBlock() { BlockType = new BlockType(-1, "In"),
												   ObjectType = ObjectType.InputCommandBlock };
			schema.Blocks.Add(block1);

			var block2 = new Block() { BlockType = new BlockType(16, "CRC"),
									   ObjectType = ObjectType.Block };
			schema.Blocks.Add(block2);

			var block3 = new Block() { BlockType = new BlockType(18, "InComBuf"),
									   ObjectType = ObjectType.Block };
			schema.Blocks.Add(block3);

			var block4 = new Block() { BlockType = new BlockType(4, "Hex2Byte"),
									   ObjectType = ObjectType.Block };
			schema.Blocks.Add(block4);

			var block5 = new InputCommandBlock() { BlockType = new BlockType(-1, "In"),
												   ObjectType = ObjectType.InputCommandBlock };
			schema.Blocks.Add(block5);

			var block6 = new InputCommandBlock() { BlockType = new BlockType(5, "Byte2Hex"),
												   ObjectType = ObjectType.Block };
			schema.Blocks.Add(block6);

			var block7 = new OutputCommandBlock() {	BlockType = new BlockType(-1, "Out"),
													ObjectType = ObjectType.OutputCommandBlock };
			schema.Blocks.Add(block7);

			var block8 = new Block() { BlockType = new BlockType(7, "Const1"),
									   ObjectType = ObjectType.Block };
			schema.Blocks.Add(block8);

			var block9 = new OutputCommandBlock() { BlockType = new BlockType(-1, "OutConst"),
													ObjectType = ObjectType.OutputCommandBlock };
			schema.Blocks.Add(block9);
		}

		private void AddBlockConnectionPoints(Schema schema)
		{
			schema.Blocks[0].InputPoints.Add(new BlockConnectionPoint("", -1) { Block = schema.Blocks[0] };
			schema.Blocks[0].OutputPoints.Add(new BlockConnectionPoint("", -1) { Block = schema.Blocks[0] };
			schema.Blocks[0].OutputPoints.Add(new BlockConnectionPoint("", 8) { Block = schema.Blocks[0] };

			schema.Blocks[1].InputPoints.Add(new BlockConnectionPoint("", -1) { Block = schema.Blocks[1] };
			schema.Blocks[1].OutputPoints.Add(new BlockConnectionPoint("", -1) { Block = schema.Blocks[1] };
			schema.Blocks[1].OutputPoints.Add(new BlockConnectionPoint("", 2) { Block = schema.Blocks[1] };

			schema.Blocks[2].InputPoints.Add(new BlockConnectionPoint("BId", 1) { Block = schema.Blocks[2] };
			schema.Blocks[2].InputPoints.Add(new BlockConnectionPoint("Sz", 1) { Block = schema.Blocks[2] };
			schema.Blocks[2].InputPoints.Add(new BlockConnectionPoint("Enbl", 1) { Block = schema.Blocks[2] };
			schema.Blocks[2].InputPoints.Add(new BlockConnectionPoint("Nxt", 1) { Block = schema.Blocks[2] };
			schema.Blocks[2].OutputPoints.Add(new BlockConnectionPoint("Res", 1) { Block = schema.Blocks[2] };

			schema.Blocks[3].OutputPoints.Add(new BlockConnectionPoint("BId", 1) { Block = schema.Blocks[3] };

			schema.Blocks[4].InputPoints.Add(new BlockConnectionPoint("H", 2) { Block = schema.Blocks[4] };
			schema.Blocks[4].OutputPoints.Add(new BlockConnectionPoint("B", 1) { Block = schema.Blocks[4] };

			schema.Blocks[5].InputPoints.Add(new BlockConnectionPoint("", -1) { Block = schema.Blocks[5] };
			schema.Blocks[5].OutputPoints.Add(new BlockConnectionPoint("", -1) { Block = schema.Blocks[5] };
			schema.Blocks[5].OutputPoints.Add(new BlockConnectionPoint("", 3) { Block = schema.Blocks[5] };

			schema.Blocks[6].InputPoints.Add(new BlockConnectionPoint("B", 1) { Block = schema.Blocks[6] };
			schema.Blocks[6].OutputPoints.Add(new BlockConnectionPoint("H", 2) { Block = schema.Blocks[6] };

			schema.Blocks[7].InputPoints.Add(new BlockConnectionPoint("", -2) { Block = schema.Blocks[7] };
			schema.Blocks[7].InputPoints.Add(new BlockConnectionPoint("", 2) { Block = schema.Blocks[7] };
			schema.Blocks[7].OutputPoints.Add(new BlockConnectionPoint("", -2) { Block = schema.Blocks[7] };

			schema.Blocks[8].OutputPoints.Add(new BlockConnectionPoint("C1", 1) { Block = schema.Blocks[8] };

			schema.Blocks[9].InputPoints.Add(new BlockConnectionPoint("", -2) { Block = schema.Blocks[9] };
			schema.Blocks[9].OutputPoints.Add(new BlockConnectionPoint("", -2) { Block = schema.Blocks[9] };
		}
	}
}
