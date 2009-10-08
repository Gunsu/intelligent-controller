using System.Xml.Linq;
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
		/// <param name="uiSchema">Сериализованный набор компонентов в дизайнере.</param>
		/// <returns>Возвращает true, если схема успешно сохранена.</returns>
		bool Save(ISchema schema, XElement uiSchema);

		/// <summary>
		/// Создаёт схему.
		/// </summary>
		/// <param name="name">Название схемы.</param>
		/// <returns>Созданная схема.</returns>
		ISchema Create(string name);
	}
}
