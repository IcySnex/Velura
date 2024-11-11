using System.ComponentModel;

namespace Velura.Services.Abstract;

public interface INavigation
{
	void GoTo<TViewModel>(
		TViewModel? viewModel = default) where TViewModel : INotifyPropertyChanged;
	
	
	void Push<TViewModel>(
		TViewModel? viewModel = default) where TViewModel : INotifyPropertyChanged;

	void Pop();
	
	
	void Present<TViewModel>(
		TViewModel? viewModel = default) where TViewModel : INotifyPropertyChanged;

	void Dismiss();
}