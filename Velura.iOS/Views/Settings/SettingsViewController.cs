using Microsoft.Extensions.DependencyInjection;
using Velura.Helpers;
using Velura.ViewModels;

namespace Velura.iOS.Views.Settings;

public class SettingsViewController : UIViewController
{
	readonly SettingsViewModel viewModel = App.Provider.GetRequiredService<SettingsViewModel>();

	readonly Dictionary<string, SettingsGroupViewController> viewControllersCache = [];

	public SettingsViewController()
	{
		Title = "settings_title".L10N();
		TabBarItem.Image = UIImage.GetSystemImage("gearshape");
		TabBarItem.SelectedImage = UIImage.GetSystemImage("gearshape.fill");
	}
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		// Properties
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		
		// UI
		ProductHeaderView headerView = new(viewModel.ShowAboutInfoCommand);
		SettingsGroupViewController groupViewController = new(viewModel.Group, headerView, new(viewModel.Config), viewControllersCache);
        
        AddChildViewController(groupViewController);
        View!.AddSubview(groupViewController.View!);
        
        groupViewController.View!.Frame = View.Bounds;
        groupViewController.View!.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
        groupViewController.DidMoveToParentViewController(this);
	}
}