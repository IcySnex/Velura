using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using Velura.ViewModels.Abstract;

namespace Velura.ViewModels;

public sealed class MainViewModel : ObservableMvxViewModel
{
	readonly ILogger<MainViewModel> logger;
	readonly IMvxNavigationService navigationService;
	
	public MainViewModel(
		ILogger<MainViewModel> logger,
		IMvxNavigationService navigationService)
	{
		this.logger = logger;
		this.navigationService = navigationService;
		
		logger.LogInformation("[MainViewModel-.ctor] MainViewModel has been initialized.");
	}


	public Task SetupTabsAsync()
	{
		logger.LogInformation("[MainViewModel-SetupTabsAsync] Setting up tabs...");

		return Task.WhenAll(
			navigationService.Navigate(typeof(HomeViewModel)),
			navigationService.Navigate(typeof(SearchViewModel)),
			navigationService.Navigate(typeof(SettingsViewModel)));
	}
}