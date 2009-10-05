using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;
using IC.CoreInterfaces.Objects;
using IC.CoreInterfaces.Processes;
using Project.Utils.Common;
using Project.Utils.DesignByContract;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.Core.Processes
{
	/// <summary>
	/// Процессы для работы со схемой.
	/// </summary>
	[Validate]
	public sealed class SchemaProcesses : ISchemaProcesses
	{
		private readonly IProject _project;

		/// <summary>
		/// Компилирует схему.
		/// </summary>
		/// <param name="schema">Схема для компилирования.</param>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		public ProcessResult Compile(ISchema schema)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Проверяет схему.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		public ProcessResult Validate(ISchema schema)
		{
			Check.Require(schema != null, "schema не может быть равным null.");
			Check.Require(_project.Schemas.Contains(schema), "_project не содержит данной schema.");

			bool noErrors = true;
			StringCollection errors = new StringCollection();

			if (schema.Blocks.GetCommandInputBlocks().Count == 0)
			{
				noErrors = false;
				errors.Add("Необходим хотя бы один блок входной команды.");
			}

			if (schema.Blocks.GetCommandOutputBlocks().Count == 0)
			{
				noErrors = false;
				errors.Add("Необходим хотя бы один блок выходной команды.");
			}

			IBlockProcesses bp = new BlockProcesses();
			if (bp.CommandInputBlocksAreConnectedInChain(schema.Blocks).Result == false)
			{
				noErrors = false;
				errors.Add("Не все блоки входной команды соединены в цепочку.");
			}

			if (bp.CommandOutputBlocksAreConnectedInChain(schema.Blocks).Result == false)
			{
				noErrors = false;
				errors.Add("Не все блоки выходной команды соединены в цепочку.");
			}

#warning и что делать с координатами?
			//foreach (var block in schema.Blocks)
			//{
			//    if (bp.BlockHasHangingInput(block).Result == true)
			//    {
			//        noErrors = false;
			//        errors.Add(string.Format("Блок по координатам {0} имеет висячие входы.", block.Coordinates));
			//    }
			//}

			return new ProcessResult() {NoErrors = noErrors, ErrorMessages = errors};
		}

		/// <summary>
		/// Сохраняет схему.
		/// </summary>
		/// <param name="schema">Схема.</param>
		/// <param name="uiSchema">Сериализованный набор компонентов в дизайнере.</param>
		/// <returns>Возвращает true, если схема успешно сохранена.</returns>
		public bool Save([NotNull] ISchema schema, [NotNull] XElement uiSchema)
		{
			schema.UISchema = uiSchema;
			schema.IsSaved = true;
			return true;
		}
	}
}
