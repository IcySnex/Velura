using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.iOS.Helpers;
using Velura.iOS.Views.Home;
using Velura.iOS.Views.Search;
using Velura.iOS.Views.Settings;
using Velura.Models;
using Velura.Services.Abstract;

namespace Velura.iOS.Views;

public class MainViewController : UITabBarController, IUITabBarControllerDelegate
{
	readonly Config config;
	
	public MainViewController(
		ILogger<MainViewController> logger,
		Config config,
		IThemeManager themeManager)
	{
		this.config = config;
		
		// Properties
		Title = "Velura";
		Delegate = this;
		Mode = UITabBarControllerMode.TabSidebar;

		themeManager.SetMode(config.Appearance.Theme);
		
		// Views
		ViewControllers =
		[
			new HomeViewController().WrapInNavController(config.Appearance.PreferLargeTitles),
			new SearchViewController().WrapInNavController(config.Appearance.PreferLargeTitles),
			new SettingsViewController().WrapInNavController(config.Appearance.PreferLargeTitles)
		];
		logger.LogInformation("[MainViewController-.ctor] MainViewController has been initialized and UI has been created.");
	}

	
	public new bool ShouldSelectViewController(
		UITabBarController tabBarController,
		UIViewController viewController)
	{
		if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Phone || viewController.TabBarItem is null || tabBarController.ViewControllers is null || !config.Appearance.AnimateTabBar)
			return true;
		
		int tabIndex = Array.IndexOf(tabBarController.ViewControllers, viewController);
		if (tabIndex == -1)
			return true;

		UIView? tabBarIcon = tabBarController.TabBar.Subviews[tabIndex + 1].Subviews.FirstOrDefault()?.Subviews.FirstOrDefault()?.Subviews.FirstOrDefault();
		tabBarIcon?.AnimateBounce();
		return true;
	}
}