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
		bool Validate(ISchema schema, out List<string> errors);
	}
}
