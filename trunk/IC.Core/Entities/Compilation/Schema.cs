using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Xml.Serialization;
using IC.Core.Enums;
using ValidationAspects;
using System.Text;

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
		/// Точки соединения текущей схемы.
		/// </summary>
		public List<ConnectionPoint> Points { get; set; }

		/// <summary>
		/// Определяет, сохранена ли схема.
		/// </summary>
		[XmlIgnore]
		public bool IsSaved { get; set; }

		/// <summary>
		/// Определяет структуру UI.
		/// </summary>
		public XElement UISchema { get; set; }

		public Project Project { get; set; }

		public string GetMask()
		{
			var maskBuilder = new StringBuilder();
			var block = (InputCommandBlock)GetBlockWithoutInputs(ObjectType.InputCommandBlock);
			while (block != null)
			{
				maskBuilder.Append(block.Mask);
				var point = block.OutputPoints[0];
				block = point.Outputs.Count != 0
					? (InputCommandBlock)((BlockConnectionPoint)point.Outputs[0]).Block
					: null;
			}

			return maskBuilder.ToString();
		}

		[XmlIgnore]
		internal List<MemoryPoolVariable> Variables { get; private set; }

		public Schema()
		{
			Blocks = new Blocks();
			IsSaved = false;

			Variables = new List<MemoryPoolVariable>();
			Points = new List<ConnectionPoint>();
		}

		/// <summary>
		/// Компилирует схему.
		/// </summary>
		public void Compile(ref int pos)
		{
			List<Block> sortedBlocks = new List<Block>(); // массив блоков, упорядоченный по очерёдности обработки блоков
			int schemaHeaderPos; // позиция в ROM описания блоков схемы
			int schemaBlockHeaderPos; // позиция в ROM описания блока
			int inMaskPos; // позиция в маске входной или выходной команды

			Block block; // вспомогательные переменные
			BlockConnectionPoint point;
			int order;
			int blocksCount;

			// Подготовка

			Project.MemoryPool.Free();

			// Очистим временные данные компиляции
			ResetCompileData();

			// Заполним массив блоками в том порядке, в котором они будут обрабатываться
			block = GetBlockWithProcessedInputs();
			while (block != null)
			{
				if (block.ObjectType == ObjectType.Block)
				{
					block.Order = sortedBlocks.Count;
					sortedBlocks.Add(block);
				}
				block = GetBlockWithProcessedInputs();

				// Зададим порядок обработки выходных блоков самыми последними цифрами
				order = sortedBlocks.Count;
				for (int i = 0; i < Blocks.Count; ++i)
				{
					if (Blocks[i].ObjectType == ObjectType.OutputCommandBlock)
					{
						Blocks[i].Processed = true;
						Blocks[i].Order = order;
						order++;
					}
				}
			}

			// Заголовок схемы

			blocksCount = Blocks.GetSimpleBlocks().Count + 2;
			schemaHeaderPos = pos;
			// первый байт заголовка схемы содержит количество блоков (+2 служебных блока 0 и 1)
			Project.ROMData[schemaHeaderPos] = Convert.ToByte(blocksCount);
			schemaHeaderPos++;

			// пропустим заголовок схемы
			pos += Convert.ToInt16(1 + blocksCount * 3);
			// 1 байт количества блоков + 3 байта на каждый блок (1 байт id блока + 2 байта адрес параметров)

			// Выходная команда. Спецблок 0

			// запишем спецблок 0 в заголовок схемы
			Project.ROMData[schemaHeaderPos] = 0; // id блока
			schemaHeaderPos++;
			// адрес параметров
			Project.ROMData[schemaHeaderPos] = Convert.ToByte(pos >> 8);
			schemaHeaderPos++;
			Project.ROMData[schemaHeaderPos] = Convert.ToByte(pos);
			schemaHeaderPos++;

			// обработаем последовательно блоки входной команды
			// для каждого блока входной команды создаём переменную
			// сразу пишем в ROM информацию по спецблоку 0
			blocksCount = 0;
			inMaskPos = 0;
			schemaBlockHeaderPos = pos;
			pos++;
			block = GetBlockWithoutInputs(ObjectType.InputCommandBlock);
			while (block != null)
			{
				// если выход блока с чем-нибудь соединён, то можно добавлять переменную
				if (block.OutputPoints[1].Outputs.Count != 0)
				{
					// добавим переменную
					var varka = AddVariable(block.OutputPoints[1]);
					blocksCount++;

					// пишем информацию по спецблоку 0 в ROM
					Project.ROMData[pos] = (byte)inMaskPos; // откуда
					pos++;
					Project.ROMData[pos] = (byte)varka.Address; // куда
					pos++;
					Project.ROMData[pos] = (byte)varka.Size; // сколько
					pos++;
				}

				inMaskPos += ((InputCommandBlock)block).Mask.Length;

				point = block.OutputPoints[0];
				block = point.Outputs.Count != 0 ? ((BlockConnectionPoint)point.Outputs[0]).Block : null;
			}
			// количество параметров спецблока 0
			Project.ROMData[schemaBlockHeaderPos] = (byte)blocksCount;

			// Основные блоки

			// перебираем основные блоки в порядке обработки
			// создаём переменные, данные сразу пишем в ROM

			for (int i = 0; i < sortedBlocks.Count; ++i)
			{
				// запишем блок в заголовок схемы
				int xxx = sortedBlocks[i].BlockType.ID;
				Project.ROMData[schemaHeaderPos] = (byte)xxx;
				schemaHeaderPos++;
				// адрес параметров
				Project.ROMData[schemaHeaderPos] = (byte)(pos >> 8); // старший байт адреса
				schemaHeaderPos++;
				Project.ROMData[schemaHeaderPos] = (byte)pos; // младший байт адреса
				schemaHeaderPos++;

				// входные параметры
				for (int j = 0; j < sortedBlocks[i].InputPoints.Count; ++j)
				{
					Project.ROMData[pos] = (byte)Variables[sortedBlocks[i].InputPoints[j].VariableIndex].Address;
					pos++;
				}

				// навешиваем переменные на выход и сразу добавляем выходные параметры
				for (int j = 0; j < sortedBlocks[i].OutputPoints.Count; ++j)
				{
					var varka = AddVariable(sortedBlocks[i].OutputPoints[j]);
					Project.ROMData[pos] = (byte)varka.Address;
					pos++;
				}

				// переменные, входящие только в данный блок больше не нужны
				GarbageCollect(sortedBlocks[i].Order);
			}

			// Выходная команда. Спецблок 1

			// запишем спецблок 1 в заголовок схемы
			Project.ROMData[schemaHeaderPos] = 1; // id блока
			schemaHeaderPos++;
			// адрес параметров
			Project.ROMData[schemaHeaderPos] = (byte)(pos >> 8); // старший байт адреса
			schemaHeaderPos++;
			Project.ROMData[schemaHeaderPos] = (byte)pos; // младший байт адреса
			schemaHeaderPos++;

			// обработаем последовательно блоки выходной команды
			// пишем в ром информацию по спецблоку 1
			blocksCount = 0;
			schemaBlockHeaderPos = pos;
			pos++;
			block = GetFirstOutputCommandBlock();
			while (block != null)
			{
				// пишем информацию по спецблоку 1 в ром
				// для трех типов выходных блоков пишется разная информация
				if (block.GetType() == typeof(OutputCommandBlock))
				{
					blocksCount++;

					var varka = Variables[block.InputPoints[1].VariableIndex];

					Project.ROMData[pos] = (byte)varka.Address; // откуда
					pos++;
					Project.ROMData[pos] = (byte)OutParamType.Variable; // модификатор параметра
					pos++;
					Project.ROMData[pos] = (byte)varka.Size; // сколько
					pos++;
				}
#warning здесь можно включить CommandBufBlock
				//else if (block.GetType() == typeof(OutputCommandBufBlock))
				//{
				//    blocksCount++;

				//    // для буфера все аналогично выводу переменной
				//    var varka = Variables[block.InputPoints[1].VariableIndex];

				//    Project.ROMData[pos] = (byte)varka.Address; // откуда
				//    pos++;
				//    Project.ROMData[pos] = (byte)OutParamType.Buf; // модификатор параметра
				//    pos++;
				//    Project.ROMData[pos] = (byte)varka.Size; // сколько
				//    pos++;
				//}
				else if (block.GetType() == typeof(OutputCommandConstBlock))
				{
					// для константы данные берутся прямо из ПЗУ

					for (int i = 0; i < ((OutputCommandBlock)block).Mask.Length; i++)
					{
						blocksCount++;

						Project.ROMData[pos] = (byte)((OutputCommandBlock)block).Mask[i];
						// откуда (на самом деле конкретный байт константы)
						pos++;
						Project.ROMData[pos] = (byte)OutParamType.Const; // модификатор параметра
						pos++;
						Project.ROMData[pos] = 1; // сколько
						pos++;
					}
				}

				point = block.OutputPoints[0];
				block = point.Outputs.Count != 0 ? ((BlockConnectionPoint)point.Outputs[0]).Block : null;
			}

			// кол-во параметров спецблока 1
			Project.ROMData[schemaBlockHeaderPos] = (byte)blocksCount;
		}

		private void GarbageCollect(int lifeTime)
		{
			foreach (var variable in Variables)
			{
				if (variable.Active && variable.LifeTime <= lifeTime)
				{
					variable.Active = false;
					for (int i = variable.Address; i < variable.Address + variable.Size; ++i)
					{
						Project.MemoryPool.FreeByte(i);
					}
				}
			}
		}

		private MemoryPoolVariable AddVariable(ConnectionPoint point)
		{
			var oldestBlock = GetOldestBlockOrderRecursive(point);
			var variable = new MemoryPoolVariable();
			variable.Size = point.Size;
			variable.Address = AllocateMemoryForVariable(variable.Size);
			variable.Active = true;
			variable.LifeTime = oldestBlock != null ? oldestBlock.Order : 0;
			Variables.Add(variable);

			// рекурсивно повесим переменную на данную и все дочерние точки
			point.SetCompileVariableRecursive(Variables.Count - 1);

			return variable;
		}

		/// <summary>
		/// Захватывает память в пуле памяти под новую переменную
		/// возвращает адрес захваченной памяти
		/// </summary>
		private int AllocateMemoryForVariable(int variableSize)
		{
			int freeBytesCount = 0;
			for (int i = 0; i < Project.MemoryPool.Size; ++i)
			{
				if (Project.MemoryPool.IsFreeByte(i))
				{
					freeBytesCount++;

					if (freeBytesCount == variableSize)
					{
						int freeBlockAddress = i - variableSize + 1;
						for (int j = freeBlockAddress; j < freeBlockAddress + variableSize; j++)
							Project.MemoryPool.AllocateByte(j);
						return freeBlockAddress;
					}
				}
				else
					freeBytesCount = 0;
			}
			throw new InvalidOperationException("Невозможно выделить память по переменную. Пул памяти исчерпан. Размер пула памяти " + Project.MemoryPool.Size + " байт. Необходимо уменьшить количество блоков в схеме.");
		}

		/// <summary>
		/// Возвращает для заданной точки соединения самый старший в порядке обработки блок
		/// </summary>
		private Block GetOldestBlockOrderRecursive(ConnectionPoint point)
		{
			List<Block> endBlocks = new List<Block>();
			GetEndBlocksRecursive(point, endBlocks);
			Block result = (endBlocks.Count > 0) ? endBlocks[0] : null;
			foreach (var block in endBlocks)
			{
				if (block.Order > result.Order)
					result = block;
			}

			return result;
		}

		private OutputCommandBlock GetFirstOutputCommandBlock()
		{
			foreach(var block in Blocks)
			{
				if (block is OutputCommandBlock)
				{
					if (GetParentPoint(block.InputPoints[0]) == null)
						return (OutputCommandBlock)block;
				}
			}

			return null;
		}

		/// <summary>
		/// Возвращает для заданной точки соединения массив блоков, с которыми она соединена
		/// </summary>
		private void GetEndBlocksRecursive(ConnectionPoint point, List<Block> endBlocks)
		{
			if (point.ObjectType == ObjectType.BlockOutputPoint)
			{
				endBlocks.Add(((BlockConnectionPoint)point).Block);
				return;
			}

			foreach (var outputPoint in point.Outputs)
			{
				GetEndBlocksRecursive(outputPoint, endBlocks);
			}
		}

		/// <summary>
		/// Сбрасывает данные компиляции для всех объектов в исходные значения
		/// </summary>
		private void ResetCompileData()
		{
			Variables.Clear();

			foreach (var block in Blocks)
			{

				block.Processed = false;
				block.Order = -1;

				foreach(var point in block.InputPoints)
				{
					point.Processed = false;
					point.VariableIndex = -1;
				}

				foreach (var point in block.OutputPoints)
				{
					point.Processed = false;
					point.VariableIndex = -1;
				}
			}
		}

		/// <summary>
		/// Получает блок, у которого все входы помечены как обработанные
		/// рассматриваются только блоки входной команды и простые блоки.
		/// Когда блок найден, он помечается как обработанный,
		/// все его выходы рекурсивно помечаются как обработанные.
		/// </summary>
		/// <returns>Блок, соответствующий описанным условиям.</returns>
		private Block GetBlockWithProcessedInputs()
		{
			foreach (var block in Blocks)
			{
				if (block.Processed) continue;

				// блоки выходных команд всегда пропускаем
				if (block.ObjectType == ObjectType.OutputCommandBlock) continue;

				// блок входной команды сразу пометим как обработанный
				if (block.ObjectType != ObjectType.InputCommandBlock)
				{
					int inputPointIndex;
					// для простого блока потребуется проверка входов на обработонность
					for (inputPointIndex = 0; inputPointIndex < block.InputPoints.Count; ++inputPointIndex)
					{
						if (!block.InputPoints[inputPointIndex].Processed) break;
					}
					if (inputPointIndex < block.InputPoints.Count) continue;
				}

				// блок нашёлся. Пометим его выходы как обработанные
				block.Processed = true;
				foreach (var outputPoint in block.OutputPoints)
				{
					outputPoint.SetProcessedFlagRecursive();
				}

				return block;
			}

			return null;
		}


		private Block GetBlockWithoutInputs(ObjectType objectType)
		{
			foreach (var block in Blocks)
			{
				if (block.ObjectType == objectType)
				{
					int inputPointIndex;
					for (inputPointIndex = 0; inputPointIndex < block.InputPoints.Count; ++inputPointIndex)
					{
						if (GetParentPoint(block.InputPoints[inputPointIndex]) != null)
							break;
					}

					if (inputPointIndex < block.InputPoints.Count)
						continue;

					return block;
				}
			}

			return null;
		}

		private ConnectionPoint GetParentPoint(ConnectionPoint connectionPoint)
		{
			foreach (var block in Blocks)
			{
				var outputPoint = GetNeededOutputPointFromPoints(connectionPoint, block.InputPoints) ??
								  (GetNeededOutputPointFromPoints(connectionPoint, block.OutputPoints) ??
								  GetNeededOutputPointFromPoints(connectionPoint, this.Points));

				if (outputPoint != null)
					return outputPoint;
			}

			return null;
		}

		/// <summary>
		/// Ищет нужную точку среди OutputPoints каждой точки.
		/// </summary>
		/// <param name="neededPoint">Точка, которую ищем.</param>
		/// <param name="points">Точки, в которых ищем.</param>
		/// <returns>Найденная точка или null если не найдена.</returns>
		private static ConnectionPoint GetNeededOutputPointFromPoints<T>(ConnectionPoint neededPoint, IEnumerable<T> points)
			where T : ConnectionPoint
		{
			foreach (var point in points)
			{
				return point.Outputs.Find(x => (x == neededPoint));
			}

			return null;
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
