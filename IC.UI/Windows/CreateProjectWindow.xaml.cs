using System.Windows;

using IC.UI.Infrastructure.Interfaces.Windows;
using Microsoft.Practices.Composite.Events;

namespace IC.UI.Windows
{
	/// <summary>
	/// Логика взаимодействия для CreateProjectWindow.xaml
	/// </summary>
	public partial class CreateProjectWindow : Window, ICreateProjectWindow
	{
		private readonly IEventAggregator eventAggregator;

		public CreateProjectWindow(IEventAggregator eventAggregator)
		{
			InitializeComponent();
		}
	}
}
