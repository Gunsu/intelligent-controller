using System.ComponentModel;
using Microsoft.Practices.Composite.Events;

namespace IC.PresentationModels
{
	public abstract class BasePresentationModel : INotifyPropertyChanged
	{
		protected readonly IEventAggregator _eventAggregator;


		public event PropertyChangedEventHandler PropertyChanged = delegate { };


		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}


		protected BasePresentationModel(IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
		}
	}
}
