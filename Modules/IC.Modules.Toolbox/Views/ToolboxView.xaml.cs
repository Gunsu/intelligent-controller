using IC.Modules.Toolbox.Interfaces.PresentationModels;
using IC.Modules.Toolbox.Interfaces.Views;
using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.Modules.Toolbox.Views
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
