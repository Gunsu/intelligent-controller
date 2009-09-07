using System.Windows.Input;

namespace IC.UI.Infrastructure.Interfaces.Schema
{
	public interface ISchemaPresentationModel
	{
		void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e);
		void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e);
		void OnMouseMove(object sender, MouseEventArgs e);
	}
}
