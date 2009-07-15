using System.Windows.Controls;

using IC.Presenters.ViewInterfaces;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для ToolboxView.xaml
	/// </summary>
	public partial class ToolboxView : UserControl, IToolboxView
	{
		public ToolboxView()
		{
			InitializeComponent();
		}
	}
}
