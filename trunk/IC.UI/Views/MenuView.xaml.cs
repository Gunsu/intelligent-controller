using IC.UI.Infrastructure.Interfaces.Menu;

namespace IC.UI.Views
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
