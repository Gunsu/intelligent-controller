using IC.Modules.Menu.Interfaces.PresentationModels;
using IC.Modules.Menu.Interfaces.Views;

namespace IC.Modules.Menu.Views
{
	/// <summary>
	/// Логика взаимодействия для MenuView.xaml
	/// </summary>
	public partial class MenuView : IMenuView
	{
        public MenuView()
		{
			InitializeComponent();
		}

		#region IMenuView members

		public IMenuPresentationModel Model
		{
			get { return DataContext as IMenuPresentationModel; }
			set { DataContext = value; }
		}

		#endregion
	}
}
