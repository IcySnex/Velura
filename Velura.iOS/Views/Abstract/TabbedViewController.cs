using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;

namespace Velura.iOS.Views.Abstract;

[MvxTabPresentation(WrapInNavigationController = true)]
public abstract class TabbedViewController<TViewModel> : MvxViewController<TViewModel>  where TViewModel : class, IMvxViewModel
{
	protected TabbedViewController(
		string title,
		string iconName,
		string selectedIconName)
	{
		// Properties
		Title = title;
		TabBarItem.Image = UIImage.GetSystemImage(iconName);
		TabBarItem.SelectedImage = UIImage.GetSystemImage(selectedIconName);
	}

	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationController!.NavigationBar.PrefersLargeTitles = true;
	}
}