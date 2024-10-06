using CoreAnimation;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using Velura.ViewModels;

namespace Velura.iOS.Views;

[MvxRootPresentation]
public sealed class MainViewController : MvxTabBarViewController<MainViewModel>, IUITabBarControllerDelegate
{
	public MainViewController()
	{
		Title = "Velura";
	}
	
	bool isInitialized = false;
	
	public override async void ViewWillAppear(
		bool animated)
	{
		base.ViewWillAppear(animated);

		if (isInitialized || ViewModel is null)
			return;
		
		await ViewModel.SetupTabsAsync();
		isInitialized = true;
	}
	
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		Delegate = this;
		
		if (UIDevice.CurrentDevice.CheckSystemVersion(18, 0))
			Mode = UITabBarControllerMode.TabSidebar;
	}
	
	[Export("tabBarController:shouldSelectViewController:")]
	public new bool ShouldSelectViewController(
		UITabBarController tabBarController,
		UIViewController viewController)
	{
		if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Phone || viewController?.TabBarItem is null || tabBarController.ViewControllers is null)
			return true;

		int tabIndex = Array.IndexOf(tabBarController.ViewControllers, viewController);
		if (tabIndex == -1)
			return true;

		UIView? tabBarIcon = tabBarController.TabBar.Subviews[tabIndex + 1].Subviews.FirstOrDefault()?.Subviews.FirstOrDefault()?.Subviews.FirstOrDefault();
		if (tabBarIcon is null)
			return true;
		
		BounceAnimation(tabBarIcon);
		return true;
	}
	
	static void BounceAnimation(
		UIView view)
    {
        CABasicAnimation animation = CABasicAnimation.FromKeyPath("transform.scale");
        animation.Duration = 0.1;
        animation.From = NSNumber.FromFloat(1.0f);
        animation.To = NSNumber.FromFloat(1.2f);
        animation.AutoReverses = true;
        animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
        
        view.Layer.AddAnimation(animation, "bounce");
    }
}