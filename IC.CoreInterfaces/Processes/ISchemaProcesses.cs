using IC.CoreInterfaces.Objects;
using Project.Utils.Common;

namespace IC.CoreInterfaces.Processes
{
	/// <summary>
	/// Процессы для работы со схемой.
	/// </summary>
	public interface ISchemaProcesses
	{
		/// <summary>
		/// Проверяет схему.
		/// </summary>
		/// <param name="schema">Схема для проверки.</param>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		ProcessResult Validate(ISchema schema);

		/// <summary>
		/// Компилирует схему.
		/// </summary>
		/// <param name="schema">Схема для компилирования.</param>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		ProcessResult Compile(ISchema schema);

		/// <summary>
		/// Сохраняет схему.
		/// </summary>
		/// <param name="schema">Схема.</param>
		/// <returns>Возвращает результат выполнения процесса.</returns>
		ProcessResult Save(ISchema schema);
	}
}
