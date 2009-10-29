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
			
		}
	}
}
