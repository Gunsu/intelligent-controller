﻿using System.Collections.Generic;

using IC.CoreInterfaces.Objects;

namespace IC.CoreInterfaces.Processes
{
	/// <summary>
	/// Процессы для работы с проектом.
	/// </summary>
	public interface IProjectProcesses
	{
		/// <summary>
		/// Проверяет все схемы проекта.
		/// </summary>
		/// <param name="errorsToSchemaMap">Список ошибок всех схем, в случае их возникновения.</param>
		/// <returns>Возвращает true, если все схемы успешно скомпилированы.</returns>
		bool Validate(out Dictionary<ISchema, string> errorsToSchemaMap);

		/// <summary>
		/// Компилирует все схемы проекта.
		/// </summary>
		/// <param name="errorsToSchemaMap">Список ошибок всех схем, в случае их возникновения.</param>
		/// <returns>Возвращает true, если все схемы успешно проверены и не содержат ошибок.</returns>
		bool Compile(out Dictionary<ISchema, string> errorsToSchemaMap);
	}
}