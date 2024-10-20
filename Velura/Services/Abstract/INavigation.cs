using CommunityToolkit.Mvvm.ComponentModel;

namespace Velura.Services.Abstract;

public interface INavigation
{
	void NavigateTo<TViewModel>(
		TViewModel viewModel) where TViewModel : ObservableObject;

}