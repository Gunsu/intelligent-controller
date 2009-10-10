using IC.CoreInterfaces.Objects;

namespace IC.UI.Infrastructure.Interfaces.Windows
{
	public interface ICreateSchemaWindow
	{
		/// <summary>
		/// Показывает диалоговое окно с предложением о создании схемы.
		/// </summary>
		/// <param name="project">Текущий проект.</param>
		/// <returns>Стандарнтый результат ShowDialog.</returns>
		bool? ShowDialog(IProject project);
	}
}
