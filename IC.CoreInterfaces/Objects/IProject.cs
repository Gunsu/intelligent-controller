using System.Collections.Generic;

namespace IC.CoreInterfaces.Objects
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

		/// <summary>
		/// Определяет, сохранён ли проект.
		/// </summary>
		bool IsSaved { get; }
	}
}
