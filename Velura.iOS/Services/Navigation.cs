using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.iOS.Views;
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
	
	
	UIViewController GetViewController<TViewModel>() where TViewModel : INotifyPropertyChanged
	{
		Type viewModelType = typeof(TViewModel);
		if (!viewControllerCache.TryGetValue(viewModelType, out UIViewController? cachedViewController))
		{
			cachedViewController = viewModelType switch
			{
				_ when viewModelType == typeof(AboutViewModel) => new UINavigationController(new AboutViewController()),
            
				_ when viewModelType == typeof(HomeViewModel) => IOSApp.MainViewController.ViewControllers![0],
				_ when viewModelType == typeof(SearchViewModel) => IOSApp.MainViewController.ViewControllers![1],
				_ when viewModelType == typeof(SettingsViewModel) => IOSApp.MainViewController.ViewControllers![2],

				_ => null
			};
			cachedViewController.ThrowIfNull($"Could not find a suitable ViewController for the given ViewModel of type '{viewModelType.Name}.", logger, "Navigation-GetViewController");
			
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

	
	public void GoTo<TViewModel>() where TViewModel : INotifyPropertyChanged
	{
		logger.LogInformation("[Navigation-GoTo] Going to ViewController...");
		IOSApp.MainViewController.SelectedViewController = GetViewController<TViewModel>();
	}

	
	public void Push<TViewModel>() where TViewModel : INotifyPropertyChanged
	{
		logger.LogInformation("[Navigation-Push] Pushing ViewController...");
		GetCurrentNavigationController().PushViewController(GetViewController<TViewModel>(), true);
	}

	public void Pop()
	{
		logger.LogInformation("[Navigation-GoBack] Popping...");
		GetCurrentNavigationController().PopViewController(true);
	}

	
	public void Present<TViewModel>() where TViewModel : INotifyPropertyChanged
	{
		logger.LogInformation("[Navigation-Show] Showing modal...");
		GetCurrentNavigationController().PresentViewController(GetViewController<TViewModel>(), true, null);
	}

	public void Dismiss()
	{
		logger.LogInformation("[Navigation-Dismiss] Dismissing modal...");
		GetCurrentNavigationController().DismissViewController(true, null);
	}
}