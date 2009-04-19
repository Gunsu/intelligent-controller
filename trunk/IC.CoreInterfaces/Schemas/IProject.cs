using System.Collections.Generic;

namespace IC.CoreInterfaces.Schemas
{
	/// <summary>
	/// Проект, являющийся совокупностью схем, с возможностью сохранения и загрузки.
	/// </summary>
	public interface IProject
	{
		/// <summary>
		/// Набор схем, входящих в проект.
		/// </summary>
		IList<ISchema> Schemas { get; }
	}
}
