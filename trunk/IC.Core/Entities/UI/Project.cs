using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.Serialization;
using ValidationAspects;

namespace IC.Core.Entities.UI
{
	/// <summary>
	/// Проект, являющийся совокупностью схем, с возможностью сохранения и загрузки.
	/// </summary>
	[Serializable]
	public class Project
	{
		private List<Schema> _schemas;

		/// <summary>
		/// Набор схем, входящих в проект.
		/// </summary>
		public ReadOnlyCollection<Schema> Schemas
		{
			get { return _schemas.AsReadOnly(); }
		}

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

		public Project()
		{
			_schemas = new List<Schema>();
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
			schema.Project = this;
			_schemas.Add(schema);
			return schema;
		}
	}
}
