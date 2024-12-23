using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.iOS.Helpers;
using Velura.iOS.Views.About;
using Velura.iOS.Views.Home;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Services.Abstract;
using Velura.ViewModels;

namespace Velura.iOS.Services;

public class Navigation : INavigation
{
	readonly ILogger<Navigation> logger;
	
	readonly Dictionary<Type, UIViewController> viewControllerCache = new();
	
	public Navigation(
		ILogger<Navigation> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[Navigation-.ctor] Navigation has been initialized.");
	}


	TViewModel GetViewModel<TViewModel>(
		object? viewModel = default) where TViewModel : INotifyPropertyChanged
	{
		if (viewModel is TViewModel typedViewModel)
			return typedViewModel;

		TViewModel? resolvedViewModel = App.Provider.GetService<TViewModel>();
		resolvedViewModel.ThrowIfNull($"Could not resolve ViewModel of type '{typeof(TViewModel).Name}'. Please pass in a ViewModel instance.", logger, "Navigation-GetViewModel");
		
		return resolvedViewModel!;
	}
	
	UIViewController GetViewController<TViewModel>(
		TViewModel? viewModel = default) where TViewModel : INotifyPropertyChanged
	{
		Type viewModelType = typeof(TViewModel);
		if (!viewControllerCache.TryGetValue(viewModelType, out UIViewController? cachedViewController))
		{
			cachedViewController = viewModelType switch
			{
				_ when viewModelType == typeof(HomeViewModel) => IOSApp.MainViewController.ViewControllers![0],
				_ when viewModelType == typeof(SearchViewModel) => IOSApp.MainViewController.ViewControllers![1],
				_ when viewModelType == typeof(SettingsViewModel) => IOSApp.MainViewController.ViewControllers![2],
				
				_ when viewModelType == typeof(MediaSectionViewModel<Movie>) => new MediaSectionViewController<Movie>(GetViewModel<MediaSectionViewModel<Movie>>(viewModel)),
				_ when viewModelType == typeof(MediaSectionViewModel<Show>) => new MediaSectionViewController<Show>(GetViewModel<MediaSectionViewModel<Show>>(viewModel)),
				_ when viewModelType == typeof(AboutViewModel) => new AboutViewController(GetViewModel<AboutViewModel>(viewModel)).WrapInNavController(),
            
				_ => null
			};
			cachedViewController.ThrowIfNull($"Could not find a suitable ViewController for the given ViewModel of type '{viewModelType.Name}'.", logger, "Navigation-GetViewController");
			
			viewControllerCache[viewModelType] = cachedViewController!;
		}
		
		return cachedViewController!;
	}
	
	UINavigationController GetCurrentNavigationController()
	{
		UINavigationController? navigationController = IOSApp.MainViewController.SelectedViewController as UINavigationController;
		navigationController.ThrowIfNull("The current selected ViewController does not support navigation", logger, "Navigation-GetCurrentNavigationController");
		
		return navigationController!;
	}

	
	public void GoTo<TViewModel>(
		TViewModel? viewModel = default) where TViewModel : INotifyPropertyChanged
	{
		logger.LogInformation("[Navigation-GoTo] Going to ViewController...");
		IOSApp.MainViewController.SelectedViewController = GetViewController(viewModel);
	}

	
	public void Push<TViewModel>(
		TViewModel? viewModel = default) where TViewModel : INotifyPropertyChanged
	{
		logger.LogInformation("[Navigation-Push] Pushing ViewController...");
		GetCurrentNavigationController().PushViewController(GetViewController(viewModel), true);
	}

	public void Pop()
	{
		logger.LogInformation("[Navigation-GoBack] Popping...");
		GetCurrentNavigationController().PopViewController(true);
	}

	
	public void Present<TViewModel>(
		TViewModel? viewModel = default) where TViewModel : INotifyPropertyChanged
	{
		logger.LogInformation("[Navigation-Show] Showing modal...");
		GetCurrentNavigationController().PresentViewController(GetViewController(viewModel), true, null);
	}

	public void Dismiss()
	{
		logger.LogInformation("[Navigation-Dismiss] Dismissing modal...");
		GetCurrentNavigationController().DismissViewController(true, null);
	}
}