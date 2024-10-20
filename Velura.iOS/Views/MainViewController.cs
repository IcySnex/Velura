using Microsoft.Extensions.Logging;
using Velura.iOS.Delegates;

namespace Velura.iOS.Views;

public class MainViewController : UITabBarController
{
	readonly IUITabBarControllerDelegate animatedBounceDelegate = new AnimatedBounceTabBarDelegate();
	
	readonly ILogger<MainViewController> logger;
	
	public MainViewController(
		ILogger<MainViewController> logger)
	{
		this.logger = logger;
		
		// Properties
		Title = "Velura";
		Delegate = animatedBounceDelegate;
		
		if (UIDevice.CurrentDevice.CheckSystemVersion(18, 0))
			Mode = UITabBarControllerMode.TabSidebar;

		// Views
		ViewControllers =
		[
			CreateNavController<HomeViewController>("Home", "house", "house.fill"),
			CreateNavController<SearchViewController>("Search", "magnifyingglass", "text.magnifyingglass"),
			CreateNavController<SettingsViewController>("Settings", "gearshape", "gearshape.fill"),
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