// ReSharper disable VirtualMemberCallInConstructor

using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;

namespace Velura.iOS.Views.Abstract;

[MvxTabPresentation(WrapInNavigationController = true)]
public abstract class ViewController<TViewModel> : MvxViewController<TViewModel>  where TViewModel : class, IMvxViewModel
{
	protected ViewController(
		string title,
		string iconName,
		string selectedIconName)
	{
		Title = title;
		TabBarItem.Image = UIImage.GetSystemImage(iconName);
		TabBarItem.SelectedImage = UIImage.GetSystemImage(selectedIconName);
	}

	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		View!.BackgroundColor = UIColor.SystemBackground;
		NavigationController!.NavigationBar.PrefersLargeTitles = true;
	}
}