using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Velura.iOS.Views;
using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public class Navigation : INavigation
{
	// readonly MainViewController mainViewController;
	// readonly ILogger<Navigation> logger;
	
	public Navigation(
		MainViewController mainViewController,
		ILogger<Navigation> logger)
	{
		// this.mainViewController = mainViewController;
		// this.logger = logger;
		
		logger.LogInformation("[Navigation-Initialize] Navigation has been initialized.");
	}


	// BaseViewController<TViewModel> CreateViewControllerForViewModel<TViewModel>(
	// 	TViewModel viewModel) where TViewModel : ObservableObject
	// {
	// 	string viewModelName = viewModel.GetType().Name;
	// 	string viewControllerName = viewModelName.Replace("ViewModel", "ViewController");
	//
	// 	Type? viewControllerType = Assembly.GetExecutingAssembly().GetTypes()
	// 		.FirstOrDefault(type => type.Name.Equals(viewControllerName, StringComparison.OrdinalIgnoreCase) && type.IsSubclassOf(typeof(BaseViewController<TViewModel>)));
	// 	if (viewControllerType is null)
	// 	{
	// 		logger.LogError("[Navigation-CreateViewControllerForViewModel] Failed to navigate to ViewModel: {name}. Could not find ViewController type for ViewModel.", viewModelName);
	// 		throw new NullReferenceException("Could not find ViewController type for ViewModel.");
	// 	}
	// 	
	// 	BaseViewController<TViewModel>? viewController = (BaseViewController<TViewModel>?)Activator.CreateInstance(viewControllerType);
	// 	if (viewController is null)
	// 	{
	// 		logger.LogError("[Navigation-CreateViewControllerForViewModel] Failed to navigate to ViewModel: {name}. Could not create type for ViewController.", viewModelName);
	// 		throw new NullReferenceException("Could not find ViewController type for ViewModel.");
	// 	}
	//
	// 	viewController.SetViewModel(viewModel);
	// 	return viewController;
	// }
	
	
	public void NavigateTo<TViewModel>(
		TViewModel viewModel) where TViewModel : ObservableObject
	{
		// BaseViewController<TViewModel> viewController = CreateViewControllerForViewModel(viewModel);
		// mainViewController.NavigationController!.PushViewController(viewController, true);
	}
}