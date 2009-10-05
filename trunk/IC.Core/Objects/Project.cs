using System.Collections.Generic;

using IC.CoreInterfaces.Objects;

namespace IC.Core.Objects
{
	/// <summary>
	/// Проект, являющийся совокупностью схем, с возможностью сохранения и загрузки.
	/// </summary>
	public class Project : IProject
	{
		private readonly IList<ISchema> _schemas;

		/// <summary>
		/// Набор схем, входящих в проект.
		/// </summary>
		public IList<ISchema> Schemas
		{
			get
			{
				return _schemas;
			}
		}

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
			_schemas = new List<ISchema>();
			IsSaved = false;
		}
	}
}
