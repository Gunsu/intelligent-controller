using System;
using System.Collections.Generic;

using IC.CoreInterfaces.Compilation;
using IC.CoreInterfaces.Schemas;
using IC.Utils.DesignByContract;

namespace IC.Core.Compilation
{
	/// <summary>
	/// Занимается проверкой и компилированием схем.
	/// </summary>
	public class Compiler : ICompiler
	{
		private readonly IProject _project;

		/// <summary>
		/// Проверяет схему.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <param name="errors">Список ошибок, в случае их возникновения.</param>
		/// <returns>Возвращает true, если схема успешно проверена и не содержит ошибок.</returns>
		public bool ValidateSchema(ISchema schema, out List<string> errors)
		{
			Check.Require(schema != null, "schema не может быть равным null.");
			Check.Require(_project.Schemas.Contains(schema), "_project не содержит данной schema.");

			bool result = true;
			errors = new List<string>();

			if (schema.Blocks.GetCommandInputBlocks().Count == 0)
			{
				result = false;
				errors.Add("Необходим хотя бы один блок входной команды.");
			}

			if (schema.Blocks.GetCommandOutputBlocks().Count == 0)
			{
				result = false;
				errors.Add("Необходим хотя бы один блок выходной команды.");
			}

			if (!CommandInputBlocksAreConnectedInChain(schema))
			{
				result = false;
				errors.Add("Не все блоки входной команды соединены в цепочку.");
			}

			if (!CommandOutputBlocksAreConnectedInChain(schema))
			{
				result = false;
				errors.Add("Не все блоки выходной команды соединены в цепочку.");
			}

			foreach (var block in schema.Blocks)
			{
				if (BlockHasHangingInput(block))
				{
					result = false;
					errors.Add(string.Format("Блок по координатам {0} имеет висячие входы.", block.Coordinates));
				}
			}

			return result;
		}

		/// <summary>
		/// Компилирует схему.
		/// </summary>
		/// <param name="schema">Схема для компиляции.</param>
		/// <param name="errors">Список ошибок, в случае их возникновения.</param>
		/// <returns>Возвращает true, если схема успешно скомпилирована.</returns>
		public bool CompileSchema(ISchema schema, out List<string> errors)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Проверяет все схемы проекта.
		/// </summary>
		/// <param name="errorsToSchemaMap">Список ошибок всех схем, в случае их возникновения.</param>
		/// <returns>Возвращает true, если все схемы успешно скомпилированы.</returns>
		public bool ValidateProject(out Dictionary<ISchema, string> errorsToSchemaMap)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Компилирует все схемы проекта.
		/// </summary>
		/// <param name="errorsToSchemaMap">Список ошибок всех схем, в случае их возникновения.</param>
		/// <returns>Возвращает true, если все схемы успешно проверены и не содержат ошибок.</returns>
		public bool CompileProject(out Dictionary<ISchema, string> errorsToSchemaMap)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Проверяет, что все входные блоки соединены в цепочку.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <returns>Возвращает true, если все входные блоки соединены в цепочку.</returns>
		private bool CommandInputBlocksAreConnectedInChain(ISchema schema)
		{
			Check.Require(schema != null, "schema не может быть равным null.");
			Check.Require(_project.Schemas.Contains(schema), "_project не содержит данной schema.");

			return true;
		}

		/// <summary>
		/// Проверяет, что все выходные блоки соединены в цепочку.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <returns>Возвращает true, если все выходные блоки соединены в цепочку.</returns>
		private bool CommandOutputBlocksAreConnectedInChain(ISchema schema)
		{
			Check.Require(schema != null, "schema не может быть равным null.");
			Check.Require(_project.Schemas.Contains(schema), "_project не содержит данной schema.");

			return true;
		}

		/// <summary>
		/// Проверяет блок на висячие входы.
		/// </summary>
		/// <param name="block">Блок для проверки.</param>
		/// <returns>Возвращает true, если блок имеет один или больше висячих входов.</returns>
		private bool BlockHasHangingInput(IBlock block)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Инициализирует класс.
		/// </summary>
		/// <param name="project">Проект, который будет необходимо компилировать.</param>
		public Compiler(IProject project)
		{
			Check.Require(project != null, "project не может быть равным null.");

			_project = project;
		}
	}
}
