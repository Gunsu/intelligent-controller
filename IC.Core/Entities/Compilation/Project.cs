using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.Serialization;
using ValidationAspects;

namespace IC.Core.Entities
{
	/// <summary>
	/// Проект, являющийся совокупностью схем, с возможностью сохранения и загрузки.
	/// </summary>
	public class Project
	{

		/// <summary>
		/// Набор схем, входящих в проект.
		/// </summary>
		public List<Schema> Schemas { get; set; }

		/// <summary>
		/// Определяет, сохранён ли проект.
		/// </summary>
		[XmlIgnore]
		public bool IsSaved { get; set; }

		/// <summary>
		/// Путь на жёстком диске, где лежит файл.
		/// </summary>
		public string FilePath { get; set; }

		public string Name { get; set; }

		[XmlIgnore]
		public ROMData ROMData { get; private set; }

		[XmlIgnore]
		internal MemoryPool MemoryPool { get; private set; }

		public Project()
		{
			Schemas = new List<Schema>();
			IsSaved = false;
			this.ROMData = new ROMData(Constants.ROM_DATA_SIZE);
			this.MemoryPool = new MemoryPool(Constants.MEMORY_POOL_SIZE);
		}

		public void Compile()
		{
			this.ROMData = new ROMData(Constants.ROM_DATA_SIZE);
			this.MemoryPool = new MemoryPool(Constants.MEMORY_POOL_SIZE);

			int pos = 1; // текущая позиция в ПЗУ со схемами обработки команд
			int posInMasksArray = 1; // текущая позиция в ПЗУ со списком масок

			// первый байт ПЗУ содержит количество масок команд
			ROMData[0] = Convert.ToByte(Schemas.Count);

			// получаем позицию в ПЗУ после описания масок команд
			foreach (var schema in Schemas)
			{
				// 1 байт длины маски + сама маска + 2 байта адреса схемы обработки команды
				pos += schema.GetMask().Length + 3;
			}

			// компилируем каждую схему обработки команды
			foreach (var schema in Schemas)
			{
				ROMData[posInMasksArray] = Convert.ToByte(schema.GetMask().Length);
				posInMasksArray++;
				
				// пишем в ПЗУ тело	текущей маски
				for (int j = 0; j < schema.GetMask().Length; ++j)
					ROMData[posInMasksArray + j] = (byte)schema.GetMask()[j];
				posInMasksArray += schema.GetMask().Length;

				// пишем в ПЗУ 2 байта адреса схемы обработки команды
				ROMData[posInMasksArray] = Convert.ToByte(pos >> 8);
				posInMasksArray++;
				ROMData[posInMasksArray] = Convert.ToByte(pos);
				posInMasksArray++;

				// компилируем схемы обработки команды
				schema.Compile(ref pos);
			}

			ROMData.SaveToBin("rom.bin");
		}
	}
}
