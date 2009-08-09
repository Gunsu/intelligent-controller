using ValidationAspects;
using ValidationAspects.PostSharp;
using IC.UI.Infrastructure.Interfaces.Toolbox;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для ToolboxView.xaml
	/// </summary>
	[Validate]
	public partial class ToolboxView : IToolboxView
	{
		#region IToolboxView members

		[NotNull]
		public IToolboxPresentationModel Model
		{
			get { return DataContext as IToolboxPresentationModel; }
			set { DataContext = value; }
		}

		#endregion

		public ToolboxView()
		{
			InitializeComponent();
		}
	}
}
