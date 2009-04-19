using IC.CoreInterfaces.Schemas;
using System.Collections.Generic;

namespace IC.Core.Schemas
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

		private Project()
		{
			_schemas = new List<ISchema>();
		}
	}
}
