using System;
using System.Windows.Controls;
using System.Windows.Input;
using IC.UI.Controls;
using IC.UI.Infrastructure.Interfaces.Schema;

using ValidationAspects;
using ValidationAspects.PostSharp;
using System.Windows;

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

		private void ContentPresenter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

			////Model.OnMouseLeftButtonDown(sender, e);
			//listBox.SelectedIndex = 0;
		}

		private void ContentPresenter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			Model.OnMouseLeftButtonUp(sender, e);
		}

		private void ContentPresenter_MouseMove(object sender, MouseEventArgs e)
		{
			Model.OnMouseMove(sender, e);
		}
	}
}
