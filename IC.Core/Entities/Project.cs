using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
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
		public bool IsSaved { get; set; }

		/// <summary>
		/// Путь на жёстком диске, где лежит файл.
		/// </summary>
		public string FilePath { get; set; }

		public string Name { get; set; }


		public Project()
		{
			Schemas = new List<Schema>();
			IsSaved = false;
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
			Schemas.Add(schema);
			return schema;
		}
	}
}
