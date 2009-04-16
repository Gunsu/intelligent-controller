using System;
using System.Collections.Generic;

using IC.CoreInterfaces.Compilation;
using IC.CoreInterfaces.SchemaManagement;
using IC.Utils.DesignByContract;

namespace IC.Core.Compilation
{
	/// <summary>
	/// Занимается проверкой и компилированием схем.
	/// </summary>
	public class Compiler : ICompiler
	{
		/// <summary>
		/// Проверяет схему.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <param name="errors">Список ошибок, в случае их возникновения.</param>
		/// <returns>Возвращает true, если схема успешно проверена и не содержит ошибок.</returns>
		public bool Validate(ISchema schema, out List<string> errors)
		{
			throw new NotImplementedException();
			Check.Require(schema != null, "schema не может быть равным null.");
			errors = new List<string>();

			

			return true;
		}

		
	}
}
