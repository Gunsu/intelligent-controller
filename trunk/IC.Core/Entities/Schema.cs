using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;
using ValidationAspects;

namespace IC.Core.Entities
{
	/// <summary>
	/// Схема, являющаяся совокупностью связанных между собой блоков.
	/// </summary>
	public class Schema
	{
		/// <summary>
		/// Блоки, входящие в схему.
		/// </summary>
		public Blocks Blocks { get; set; }

        /// <summary>
        /// Имя схемы.
        /// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Определяет, сохранена ли схема.
		/// </summary>
		public bool IsSaved { get; set; }

		public MemoryPool MemoryPool { get; set; }

		/// <summary>
		/// Определяет структуру UI.
		/// </summary>
		public XElement UISchema { get; set; }

		/// <summary>
		/// Закрытый конструктор, выполняющий начальную инициализацию класса.
		/// </summary>
		public Schema()
		{
			Blocks = new Blocks();
			IsSaved = false;
		}

		/// <summary>
		/// Компилирует схему.
		/// </summary>
		public void Compile(ref int pos)
		{
			throw new NotImplementedException();
			//List<Block> sortedBlocks; // массив блоков, упорядоченный по очерёдности обработки блоков
			//int schemaHeaderPos; // позиция в ROM описания блоков схемы
			//int schemaBlockHeaderPos; // позиция в ROM описания блока
			//int inMaskPos; // позиция в маске входной или выходной команды

			//Block block; // вспомогательные переменные
			//BlockConnectionPoint point;
			//int order;
			//int blocksCount;

			//// Подготовка

			//MemoryPool.Free();

			//// Очистим временные данные компиляции
			//ResetCompileData();

			//// Заполним массив блоками в том порядке, в котором они будут обрабатываться
			//block = FindBlockWithProcessedInputs();
			//while (block != null)
			//{
			//    if (block.GetType() == ObjectType.Block)
			//    {
			//        block.CompileData.Order = sortedBlocks.Count;
			//        sortedBlocks.Add(block);
			//    }
			//    block = FindBlockWithProcessedInputs();

			//    // Зададим порядок обработки выходны блоков самыми последними цифрами
			//    order = sortedBlocks.Count;
			//    for (int i = 0; i < Blocks.Count; ++i)
			//    {
			//        if (Blocks[i].GetType() == ObjectType.OutputCommandBlock)
			//        {
			//            Blocks[i].CompileData.Processed = true;
			//            Blocks[i].CompileData.Order = order;
			//            order++;
			//        }
			//    }
			//}

			//// Заголовок схемы

			//blocksCount = GetBlocksCount() + 2;
			//schemaHeaderPos = pos;
			//// первый байт заголовка схемы содержит количество блоков (+2 служебных блока 0 и 1)
			//romData[schemaHeaderPos] = blocksCount;

			//// пропустим заголовок схемы

			//pos += 1 + blocksCount*3;
			//// 1 байт количества блоков + 3 байта на каждый блок (1 байт id блока + 2 байта адрес параметров)

			//// Выходная команда. Спецблок 0

			//// запишем спецблок 0 в заголовок схемы
			//romData[schemaHeaderPos] = 0; // id блока
			//schemaHeaderPos++;
			//// адрес параметров
			//romData[schemaHeaderPos] = (char) (pos >> 8);
			//schemaHeaderPos++;
			//romData[schemaHeaderPos] = (char) (pos >> 8);
			//schemaHeaderPos++;

			//// обработаем последовательно блоки входной команды
			//// для каждого блока входной команды создаём переменную
			//// сразу пишем в ROM информацию по спецблоку 0
			//blocksCount = 0;
			//inMaskPos = 0;
			//schemaBlockHeaderPos = pos;
			//pos++;
			//block = FindBlockWithoutInputs(ObjectType.InputCommandBlock);
			//while (block != null)
			//{
			//    // если выход блока с чем-нибудь соединён, то можно добавлять переменную
			//    if (block.GetOutputPoint(1).GetFirstOutputPoint())
			//    {
			//        // добавим переменную
			//        var varka = AddVariable(block.GetOutputPoint(1));
			//        blocksCount++;

			//        // пишем информацию по спецблоку 0 в ROM
			//        romData[pos] = (char) inMaskPos; // откуда
			//        pos++;
			//        romData[pos] = (char) varka.address; // куда
			//        pos++;
			//        romData[pos] = (char) varka.size; // сколько
			//        pos++;
			//    }

			//    inMaskPos += ((InputCommandBlock) block).Mask.Length;

			//    point = (BlockConnectionPoint) block.GetOutputPoint(0).GetFirstOutputPoint();
			//    if (point != null)
			//        block = (InputCommandBlock) point.GetBlock();
			//    else
			//        block = null;
			//}
			//// количество параметров спецблока 0
			//romData[schemaBlockHeaderPos] = blocksCount;

			//// Основные блоки

			//// перебираем основные блоки в порядке обработки
			//// создаём переменные, данные сразу пишем в ROM

			//for (int i = 0; i < sortedBlocks.Count; ++i)
			//{
			//    // запишем блок в заголовок схемы
			//    int xxx = sortedBlocks[i].BlockType.ID;
			//    romData[schemaHeaderPos] = xxx;
			//    schemaHeaderPos++;
			//    // адрес параметров
			//    romData[schemaHeaderPos] = (char) (pos >> 8); // старший байт адреса
			//    schemaHeaderPos++;
			//    romData[schemaHeaderPos] = (char) pos; // младший байт адреса
			//    schemaHeaderPos++;

			//    // входные параметры
			//    for (int j = 0; j < sortedBlocks[i].GetInputPointsCount; ++j)
			//    {
			//        romData[pos] = (char) variables[sortedBlocks[i].GetInputPoint(j).CompileData.VariableIndex].address;
			//        pos++;
			//    }

			//    // навешиваем переменные на выход и сразу добавляем выходные параметры
			//    for (int j = 0; j < sortedBlocks[i].GetOutputPointsCount; ++j)
			//    {
			//        varka = AddVariable(sortedBlocks[i].GetOutputPoint(j));
			//        romData[pos] = (char) varka.Address;
			//        pos++;
			//    }

			//    // переменные, входящие только в данный блок больше не нужны
			//    GarbageCollect(sortedBlocks[i].CompileData.Order);
			//}

			//// Выходная команда. Спецблок 1

			//// запишем спецблок 1 в заголовок схемы
			//romData[schemaHeaderPos] = 1; // id блока
			//schemaHeaderPos++;
			//// адрес параметров
			//romData[schemaHeaderPos] = (char) (pos >> 8); // старший байт адреса
			//schemaHeaderPos++;
			//romData[schemaHeaderPos] = (char) pos; // младший байт адреса
			//schemaHeaderPos++;

			//// обработаем последовательно блоки выходной команды
			//// пишем в ром информацию по спецблоку 1
			//blocksCount = 0;
			//schemaBlockHeaderPos = pos;
			//pos++;
			//block = FindFirstOutputCommandBlock();
			//while (block != null)
			//{
			//    // пишем информацию по спецблоку 1 в ром
			//    // для трех типов выходных блоков пишется разная информация
			//    if (block.GetType() == ObjectType.OutputCommandBlock)
			//    {
			//        blocksCount++;

			//        varka = variable[block.GetInputPoint(1).compileData.variableIndex];

			//        romData[pos] = (char) varka.address; // откуда
			//        pos++;
			//        romData[pos] = OutParamModifier.outVar; // модификатор параметра
			//        pos++;
			//        romData[pos] = (char) varka.size; // сколько
			//        pos++;
			//    }
			//    else if (block.GetType() == ObjectType.OutputCommandBufBlock)
			//    {
			//        blocksCount++;

			//        // для буфера все аналогично выводу переменной
			//        varka = variables[block.GetInputPoint(1).compileData.variableIndex];

			//        romData[pos] = (char) varka.address; // откуда
			//        pos++;
			//        romData[pos] = OutParamModifier.outBuf; // модификатор параметра
			//        pos++;
			//        romData[pos] = (char) var.size; // сколько
			//        pos++;
			//    }
			//    else if (block.GetType() == ObjectType.OutputCommandConstBlock)
			//    {
			//        // для константы данные берутся прямо из ПЗУ

			//        for (int i = 0; i < ((OutputCommandConstBlock) block).Mask.Length; i++)
			//        {
			//            blocksCount++;

			//            romData[pos] = (char) ((OutputCommandConstBlock) block).GetMask.c_str()[i];
			//            // откуда (на самом деле конкретный байт константы)
			//            pos++;
			//            romData[pos] = OutParamModifier.outConst; // модификатор параметра
			//            pos++;
			//            romData[pos] = (char) 1; // сколько
			//            pos++;
			//        }
			//    }

			//    point = (BlockConnectionPoint) block.GetOutputPoint(0).GetFirstOutputPoint();
			//    if (point != null)
			//        block = (OutputCommandBlock) point.GetBlock();
			//    else
			//        block = NULL;
			//}

			//// кол-во параметров спецблока 1
			//romData[schemaBlockHeaderPos] = blocksCount;
		}

		/// <summary>
		/// Проверяет схему.
		/// </summary>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		public void Validate()
		{
			bool noErrors = true;
			StringCollection errors = new StringCollection();

			if (Blocks.GetCommandInputBlocks().Count == 0)
			{
				noErrors = false;
				errors.Add("Необходим хотя бы один блок входной команды.");
			}

			if (Blocks.GetCommandOutputBlocks().Count == 0)
			{
				noErrors = false;
				errors.Add("Необходим хотя бы один блок выходной команды.");
			}

			if (Blocks.CommandInputBlocksAreConnectedInChain() == false)
			{
				noErrors = false;
				errors.Add("Не все блоки входной команды соединены в цепочку.");
			}

			if (Blocks.CommandOutputBlocksAreConnectedInChain() == false)
			{
				noErrors = false;
				errors.Add("Не все блоки выходной команды соединены в цепочку.");
			}

#warning и что делать с координатами?
			//foreach (var block in schema.Blocks)
			//{
			//    if (bp.BlockHasHangingInput(block).Result == true)
			//    {
			//        noErrors = false;
			//        errors.Add(string.Format("Блок по координатам {0} имеет висячие входы.", block.Coordinates));
			//    }
			//}
			if (!noErrors)
				throw new NotImplementedException();
		}

		/// <summary>
		/// Сохраняет схему.
		/// </summary>
		/// <param name="uiSchema">Сериализованный набор компонентов в дизайнере.</param>
		/// <returns>Возвращает true, если схема успешно сохранена.</returns>
		public bool Save([NotNull] XElement uiSchema)
		{
			UISchema = uiSchema;
			IsSaved = true;
			return true;
		}
	}
}
