using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.iOS.Helpers;

namespace Velura.iOS.Views;

public class MainViewController : UITabBarController, IUITabBarControllerDelegate
{
	public new bool ShouldSelectViewController(
		UITabBarController tabBarController,
		UIViewController viewController)
	{
		if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Phone || viewController.TabBarItem is null || tabBarController.ViewControllers is null)
			return true;

		int tabIndex = Array.IndexOf(tabBarController.ViewControllers, viewController);
		if (tabIndex == -1)
			return true;

		UIView? tabBarIcon = tabBarController.TabBar.Subviews[tabIndex + 1].Subviews.FirstOrDefault()?.Subviews.FirstOrDefault()?.Subviews.FirstOrDefault();
		tabBarIcon?.AnimateBounce();
		return true;
	}
	
	
	readonly ILogger<MainViewController> logger;
	
	public MainViewController(
		ILogger<MainViewController> logger)
	{
		this.logger = logger;
		
		// Properties
		Title = "Velura";
		Delegate = this;
		
		if (UIDevice.CurrentDevice.CheckSystemVersion(18, 0))
			Mode = UITabBarControllerMode.TabSidebar;

		// Views
		ViewControllers =
		[
			CreateNavController<HomeViewController>("home_title".L10N(), "house", "house.fill"),
			CreateNavController<SearchViewController>("search_title".L10N(), "magnifyingglass", "text.magnifyingglass"),
			CreateNavController<SettingsViewController>("settings_title".L10N(), "gearshape", "gearshape.fill"),
		];
		logger.LogInformation("[MainViewController-.ctor] MainViewController has been initialized and UI has been created.");
	}


	UINavigationController CreateNavController<TViewController>(
		string title,
		string iconName,
		string selectedIconName) where TViewController : UIViewController, new()
	{
		TViewController viewController = new()
		{
			Title = title,
			TabBarItem =
			{
				Image = UIImage.GetSystemImage(iconName),
				SelectedImage = UIImage.GetSystemImage(selectedIconName)
			}
		};
		viewController.View!.BackgroundColor = UIColor.SystemGroupedBackground;
		
		UINavigationController navigationWrapper = new(viewController)
		{
			NavigationBar =
			{
				PrefersLargeTitles = true
			}
		};
		
		logger.LogInformation("[MainViewController-CreateNavController] Created wrapper NavController for TViewController: {title}.", title);
		return navigationWrapper;
	}
}