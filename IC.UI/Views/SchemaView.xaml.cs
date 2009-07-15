using System.Windows.Controls;

using IC.Presenters.ViewInterfaces;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для SchemaView.xaml
	/// </summary>
	public partial class SchemaView : UserControl, ISchemaView
	{
		public SchemaView()
		{
			InitializeComponent();
		}
	}
}
