using IC.Core.Entities;
using NUnit.Framework;
using System.IO;

namespace IC.Core.Tests
{
	[TestFixture]
	public class SchemaTest
	{
		/// <summary>
		/// Проверка компилирования схемы.
		/// Схема представляет собой два входных блока, суммирующий блок и выходной блок.
		/// </summary>
		[Test]
		public void Compile_Should_Make_Correct_Result()
		{
			// Настраиваем окружение
			var project = new Project();
			var schema = project.AddSchema("TestSchema");
			AddBlocks(schema);
			AddBlockConnectionPoints(schema);
			AddOutputsToBlockConnectionPoints(schema);

			// Задаём параметры при условии, что проект скомпилировался верно
			int pos = 6;
			project.ROMData.Data = File.ReadAllBytes("correctProjectCompilationForSchemaTest.txt");

			// Компилируем
			project.Schemas[0].Compile(ref pos);
			//project.ROMData.SaveToBin(@"c:\1.txt");

			// Проверяем
			byte[] correctCompilationResult =
				File.ReadAllBytes("correctCompilationResultForSchemaTest.txt");
			for (int i = 0; i < project.ROMData.Data.Length; ++i)
				Assert.AreEqual(correctCompilationResult[i], project.ROMData.Data[i],
					string.Format("Несовпадение с верным результатом компиляции по адресу {0}", i));
		}

		private static void AddBlocks(Schema schema)
		{
			var inBlockType = new BlockType(-1, "In");
			var outBlockType = new BlockType(-1, "Out");
			var sumBlockType = new BlockType(2, "Sum");

			var block0 = new InputCommandBlock() { BlockType = inBlockType, Mask = "_" };
			schema.Blocks.Add(block0);
			var block1 = new OutputCommandBlock() { BlockType = outBlockType, Mask = "_" };
			schema.Blocks.Add(block1);
			var block2 = new InputCommandBlock() { BlockType = inBlockType, Mask = "_" };
			schema.Blocks.Add(block2);
			var block3 = new Block() { BlockType = sumBlockType };
			schema.Blocks.Add(block3);
		}

		private static void AddBlockConnectionPoints(Schema schema)
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

		private static void AddOutputsToBlockConnectionPoints(Schema schema)
		{
			schema.Blocks[0].OutputPoints[0].Outputs.Add(schema.Blocks[2].InputPoints[0]);
			schema.Blocks[0].OutputPoints[1].Outputs.Add(schema.Blocks[3].InputPoints[0]);

			schema.Blocks[2].OutputPoints[1].Outputs.Add(schema.Blocks[3].InputPoints[1]);

			schema.Blocks[3].OutputPoints[0].Outputs.Add(schema.Blocks[1].InputPoints[1]);
		}
	}
}
