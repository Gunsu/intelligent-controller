using System.Collections.Generic;
using System.Xml.Linq;
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
		IProject Create(string name, string filePath);

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
		/// Открывает проект по указанному пути.
		/// </summary>
		/// <param name="path">Путь к файлу проекта.</param>
		/// <returns>Возвращает результат выполнения процесса и открытый проект.</returns>
		ProcessResult<IProject> Open(string path);

		/// <summary>
		/// Сохраняет проект, если сохранены все схемы.
		/// </summary>
		/// <param name="project">Проект.</param>
		/// <returns>Если сохранены не все схемы, то возвращает список несохранённых схем.</returns>
		ProcessResult<List<ISchema>> Save(IProject project);

		/// <summary>
		/// Добавляет схему к проекту.
		/// </summary>
		/// <param name="project">Проект.</param>
		/// <param name="schema">Добавляемая схема.</param>
		/// <returns>True, в случае успешного добавления.</returns>
		bool AddSchema(IProject project, ISchema schema);
	}
}
