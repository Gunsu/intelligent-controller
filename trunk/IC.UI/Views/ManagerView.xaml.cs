using System.Windows.Controls;

using IC.UI.Infrastructure.Interfaces.Manager;

using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.UI.Views
{
	/// <summary>
	/// View для менеджера главного окна. View сам по себе пустой.
	/// Менеджер отслеживает и обрабатывает глобальные события
	/// такие, как создание проекта, сохранение проекта, закрытие приложения и т. д.
	/// </summary>
	[Validate]
	public partial class ManagerView : UserControl, IManagerView
	{
		#region IManagerView members

		[NotNull]
		public IManagerPresentationModel Model
		{
			get { return DataContext as IManagerPresentationModel; }
			set { DataContext = value; }
		}

		#endregion

		public ManagerView()
		{
			InitializeComponent();
		}
	}
}
