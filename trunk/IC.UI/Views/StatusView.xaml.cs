using System.Windows.Controls;

using IC.UI.Infrastructure.Interfaces.Status;

using ValidationAspects;
using ValidationAspects.PostSharp;

namespace IC.UI.Views
{
	/// <summary>
	/// Логика взаимодействия для StatusView.xaml
	/// </summary>
	[Validate]	
	public partial class StatusView : UserControl, IStatusView
	{
		#region IStatusView members

		[NotNull]
		public IStatusPresentationModel Model
		{
			get { return DataContext as IStatusPresentationModel; }
			set { DataContext = value; }
		}

		#endregion

		public StatusView()
		{
			InitializeComponent();
		}
	}
}
