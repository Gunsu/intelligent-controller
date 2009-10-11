using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
		public IList<Schema> Schemas { get; private set; }

		/// <summary>
		/// Определяет, сохранён ли проект.
		/// </summary>
		public bool IsSaved { get; set; }

		/// <summary>
		/// Путь на жёстком диске, где лежит файл.
		/// </summary>
		public string Path { get; set; }


		private Project()
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
			throw new NotImplementedException();
		}
	}
}
