using System.Collections.Generic;
using IC.CoreInterfaces.SchemaManagement;

namespace IC.CoreInterfaces.Compilation
{
	/// <summary>
	/// Занимается проверкой и компилированием схем.
	/// </summary>
	public interface ICompiler
	{
		/// <summary>
		/// Проверяет схему.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <param name="errors">Список ошибок, в случае их возникновения.</param>
		/// <returns>Возвращает true, если схема успешно проверена и не содержит ошибок.</returns>
		bool ValidateSchema(ISchema schema, out List<string> errors);

		/// <summary>
		/// Компилирует схему.
		/// </summary>
		/// <param name="schema">Схема для компиляции.</param>
		/// <param name="errors">Список ошибок, в случае их возникновения.</param>
		/// <returns>Возвращает true, если схема успешно скомпилирована.</returns>
		bool CompileSchema(ISchema schema, out List<string> errors);

		/// <summary>
		/// Проверяет все схемы проекта.
		/// </summary>
		/// <param name="errorsToSchemaMap">Список ошибок всех схем, в случае их возникновения.</param>
		/// <returns>Возвращает true, если все схемы успешно скомпилированы.</returns>
		bool ValidateProject(out Dictionary<ISchema, string> errorsToSchemaMap);

		/// <summary>
		/// Компилирует все схемы проекта.
		/// </summary>
		/// <param name="errorsToSchemaMap">Список ошибок всех схем, в случае их возникновения.</param>
		/// <returns>Возвращает true, если все схемы успешно проверены и не содержат ошибок.</returns>
		bool CompileProject(out Dictionary<ISchema, string> errorsToSchemaMap);
	}
}
