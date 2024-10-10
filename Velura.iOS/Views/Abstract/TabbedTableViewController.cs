// ReSharper disable VirtualMemberCallInConstructor

using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;

namespace Velura.iOS.Views.Abstract;

[MvxTabPresentation(WrapInNavigationController = true)]
public abstract class TabbedTableViewController<TViewModel> : MvxTableViewController<TViewModel>  where TViewModel : class, IMvxViewModel
{
	protected TabbedTableViewController(
		string title,
		string iconName,
		string selectedIconName,
		UITableViewStyle style) : base(style)
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