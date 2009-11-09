using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using ValidationAspects;

namespace IC.Core.Entities
{
	/// <summary>
	/// Проект, являющийся совокупностью схем, с возможностью сохранения и загрузки.
	/// </summary>
	[Serializable]
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

		/// <summary>
		/// Добавляет схему к проекту.
		/// </summary>
		/// <param name="name">Название схемы.</param>
		/// <returns>True, в случае успешного добавления.</returns>
		public Schema AddSchema([NotNull] string name)
		{
			var schema = new Schema();
			schema.Name = name;
			schema.Save(new XElement("root"));
			schema.Project = this;
			Schemas.Add(schema);
			return schema;
		}
	}
}
