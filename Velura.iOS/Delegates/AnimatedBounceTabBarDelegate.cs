using Velura.iOS.Helpers;

namespace Velura.iOS.Delegates;

public class AnimatedBounceTabBarDelegate : UITabBarControllerDelegate
{
	public override bool ShouldSelectViewController(
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
}