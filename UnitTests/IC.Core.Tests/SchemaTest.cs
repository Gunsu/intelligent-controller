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
			var inBlockType = new BlockType(-1, "In");
			var outBlockType = new BlockType(-1, "Out");
			var outConstBlockType = new BlockType(-1, "OutConst");
			var adcMaskBlockType = new BlockType(20, "ADCMask");
			var const0BlockType = new BlockType(6, "Const0");
			var const1BlockType = new BlockType(7, "Const1");
			var bit2ByteBlockType = new BlockType(25, "Bit2Byte");
			var byte2HexBlockType = new BlockType(5, "Byte2Hex");
			var modAddrBlockType = new BlockType(9, "ModAddr");

			var block0 = new InputCommandBlock() {BlockType = inBlockType, Mask = "$5"};
			var block1 = new Block() {BlockType = adcMaskBlockType};
			var block2 = new Block() {BlockType = const1BlockType};
			var block3 = new Block() {BlockType = bit2ByteBlockType};
			var block4 = new Block() {BlockType = const0BlockType};
			var block5 = new Block() {BlockType = const1BlockType};
			var block6 = new OutputCommandConstBlock() {BlockType = outConstBlockType, Mask = "!"};
			var block7 = new OutputCommandBlock() {BlockType = outBlockType, Mask = "__"};
			var block8 = new Block() {BlockType = modAddrBlockType};
			var block9 = new Block() {BlockType = byte2HexBlockType};
			var block10 = new Block() {BlockType = const0BlockType};
			var block11 = new OutputCommandConstBlock() {BlockType = outConstBlockType, Mask = "ADCReg"};
			var block12 = new Block() {BlockType = byte2HexBlockType};
			var block13 = new OutputCommandBlock() {BlockType = outBlockType, Mask = "__"};
		}

		private void AddBlockConnectionPoints(Schema schema)
		{
            schema.Blocks[0].AddInputPoint(new BlockConnectionPoint() { Size = -1 });
            schema.Blocks[0].AddOutputPoint(new BlockConnectionPoint() { Size = -1 });
            schema.Blocks[0].AddOutputPoint(new BlockConnectionPoint() { Size = -1 });

            schema.Blocks[1].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "R0W1" });
            schema.Blocks[1].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "Mask" });
            schema.Blocks[1].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "Nxt" });
            schema.Blocks[1].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "Mask" });
            schema.Blocks[1].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "Nxt" });

            schema.Blocks[2].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "C1" });

            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "b0" });
            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "b1" });
            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "b2" });
            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "b3" });
            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "b4" });
            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "b5" });
            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "b6" });
            schema.Blocks[3].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "b7" });
            schema.Blocks[3].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "B" });

            schema.Blocks[4].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "C0" });

            schema.Blocks[5].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "C1" });
            
            schema.Blocks[6].AddInputPoint(new BlockConnectionPoint() { Size = -2 });
            schema.Blocks[6].AddOutputPoint(new BlockConnectionPoint() { Size = -2 });

            schema.Blocks[7].AddOutputPoint(new BlockConnectionPoint() { Size = -2 });
            schema.Blocks[7].AddInputPoint(new BlockConnectionPoint() { Size = 2 });
            schema.Blocks[7].AddOutputPoint(new BlockConnectionPoint() { Size = -2 });

            schema.Blocks[8].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "R0W1" });
            schema.Blocks[8].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "Addr" });
            schema.Blocks[8].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "Nxt" });
            schema.Blocks[8].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "Addr" });

            schema.Blocks[9].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "B" });
            schema.Blocks[9].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "H" });

            schema.Blocks[10].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "C0" });

            schema.Blocks[11].AddInputPoint(new BlockConnectionPoint() { Size = -2 });
            schema.Blocks[11].AddOutputPoint(new BlockConnectionPoint() { Size = -2 });

            schema.Blocks[12].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "B" });
            schema.Blocks[12].AddOutputPoint(new BlockConnectionPoint() { Size = 2, Name = "H" });

            schema.Blocks[13].AddInputPoint(new BlockConnectionPoint() { Size = -2 });
            schema.Blocks[13].AddInputPoint(new BlockConnectionPoint() { Size = 2 });
            schema.Blocks[13].AddOutputPoint(new BlockConnectionPoint() { Size = -2 });
		}
	}
}
