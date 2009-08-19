using System.Windows.Controls;

using IC.UI.Infrastructure.Interfaces.Schema;

using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для SchemaView.xaml
	/// </summary>
	[Validate]
	public partial class SchemaView : UserControl, ISchemaView
	{
		#region ISchemaView members

		[NotNull]
		public ISchemaPresentationModel Model
		{
			get { return DataContext as ISchemaPresentationModel; }
			set { DataContext = value; }
		}

		#endregion

		public SchemaView()
		{
			InitializeComponent();
		}
	}
}
