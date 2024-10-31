using System.ComponentModel;

namespace Velura.Services.Abstract;

public interface INavigation
{
	void GoTo<TViewModel>() where TViewModel : INotifyPropertyChanged;
	
	
	void Push<TViewModel>() where TViewModel : INotifyPropertyChanged;

	void Pop();
	
	
	void Present<TViewModel>() where TViewModel : INotifyPropertyChanged;

	void Dismiss();
}