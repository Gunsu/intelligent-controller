using System;
using System.Windows.Controls;

using IC.Presenters.ViewInterfaces;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для MenuView.xaml
	/// </summary>
	public partial class MenuView : UserControl, IMenuView
	{
		public MenuView()
		{
			InitializeComponent();
		}

		public event EventHandler SaveProjectEventHandler;
	}
}
