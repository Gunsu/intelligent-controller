﻿using NUnit.Framework;
using IC.Core.Entities;
using System.IO;
using System.Diagnostics;
namespace IC.Core.Tests
{
	[TestFixture]
	public class ProjectTest
	{
		/// <summary>
		/// Проверка компилирования проекта.
		/// Проект представляет собой две элементарные схемы.
		/// Обе состоят только из входного и выходного блока.
		/// 
		/// Это больше интеграционный тест, потому что здесь также проверяется
		/// правильность компилирования этих элементарных схем.
		/// </summary>
		[Test]
		public void Compile_Should_Make_Correct_Result()
		{
			// Настраиваем окружение
			var project = new Project();
			var schema1 = project.AddSchema("Schema1");
			AddBlocks(schema1, 0);
			AddBlockConnectionPoints(schema1);
			var schema2 = project.AddSchema("Schema2");
			AddBlocks(schema2, 1);
			AddBlockConnectionPoints(schema2);

			// Компилируем
			project.Compile();


			// Проверяем
			byte[] correctCompilationResult =
				File.ReadAllBytes("correctCompilationResultForProjectTest.txt");
			int projectCompilationBytesCount = 11; // Число байтов, относящихся ТОЛЬКО к проекту

			for (int i = 0; i < projectCompilationBytesCount; ++i)
				Assert.AreEqual(correctCompilationResult[i], project.ROMData.Data[i],
					string.Format("Несовпадение с верным результатом компиляции проекта по адресу {0}", i));

			for (int i = projectCompilationBytesCount; i < correctCompilationResult.Length; ++i)
				Assert.AreEqual(correctCompilationResult[i], project.ROMData.Data[i],
					string.Format("Несовпадение с верным результатом компиляции схем по адресу {0}", i));
		}

		/// <summary>
		/// Добавление блоков в схему.
		/// </summary>
		/// <param name="schema">Схема, в которую будут добавляться блоки.</param>
		/// <param name="schemaNumber">Номер схемы в проекте. Нужно для того, чтобы не было повторяющихся масок схем.</param>
		private static void AddBlocks(Schema schema, int schemaNumber)
		{
			var inBlockType = new BlockType(-1, "In");
			var outBlockType = new BlockType(-1, "OutConst");

			var block0 = new InputCommandBlock() { BlockType = inBlockType, Mask = "$" + schemaNumber.ToString() };
			schema.Blocks.Add(block0);
			var block1 = new OutputCommandConstBlock() { BlockType = outBlockType, Mask = "$" + schemaNumber.ToString() };
			schema.Blocks.Add(block1);
		}

		private static void AddBlockConnectionPoints(Schema schema)
		{
			schema.Blocks[0].AddInputPoint(new BlockConnectionPoint() { Size = -1, Name = "" });
			schema.Blocks[0].AddOutputPoint(new BlockConnectionPoint() { Size = -1, Name = "" });
			schema.Blocks[0].AddOutputPoint(new BlockConnectionPoint() { Size = 1, Name = "" });

			schema.Blocks[1].AddInputPoint(new BlockConnectionPoint() { Size = -2, Name = "" });
			schema.Blocks[1].AddInputPoint(new BlockConnectionPoint() { Size = 1, Name = "" });
			schema.Blocks[1].AddOutputPoint(new BlockConnectionPoint() { Size = -2, Name = "" });
		}
	}
}
