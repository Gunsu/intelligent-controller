using System.Windows.Controls;

using IC.Presenters.ViewInterfaces;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для StatusView.xaml
	/// </summary>
	public partial class StatusView : UserControl, IStatusView
	{
		public StatusView()
		{
			InitializeComponent();
		}
	}
}
