namespace IC.UI.Infrastructure.Interfaces.Manager
{
	/// <summary>
	/// View для менеджера главного окна. View сам по себе пустой.
	/// Менеджер отслеживает и обрабатывает глобальные события
	/// такие, как создание проекта, сохранение проекта, закрытие приложения и т. д.
	/// </summary>
	public interface IManagerView
	{
		IManagerPresentationModel Model { get; set; }
	}
}
