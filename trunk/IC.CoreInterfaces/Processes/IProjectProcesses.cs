using System.Collections.Generic;

using IC.CoreInterfaces.Objects;
using Project.Utils.Common;

namespace IC.CoreInterfaces.Processes
{
	/// <summary>
	/// Процессы для работы с проектом.
	/// </summary>
	public interface IProjectProcesses
	{
		/// <summary>
		/// Создание проекта.
		/// </summary>
		/// <returns>Только что созданный проект.</returns>
		IProject CreateProject(string name, string filePath);

		///// <summary>
		///// Проверяет все схемы проекта.
		///// </summary>
		///// <param name="errorsToSchemaMap">Список ошибок всех схем, в случае их возникновения.</param>
		///// <returns>Возвращает true, если все схемы успешно скомпилированы.</returns>
		//bool Validate(out Dictionary<ISchema, string> errorsToSchemaMap);

		///// <summary>
		///// Компилирует все схемы проекта.
		///// </summary>
		///// <param name="errorsToSchemaMap">Список ошибок всех схем, в случае их возникновения.</param>
		///// <returns>Возвращает true, если все схемы успешно проверены и не содержат ошибок.</returns>
		//bool Compile(out Dictionary<ISchema, string> errorsToSchemaMap);

		/// <summary>
		/// Сохраняет проект.
		/// </summary>
		/// <param name="project">Проект.</param>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		ProcessResult Save(IProject project);
	}
}
