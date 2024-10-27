using Microsoft.Extensions.DependencyInjection;
using Velura.Models;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public class SettingsViewController : UIViewController
{
	readonly SettingsViewModel viewModel = App.Provider.GetRequiredService<SettingsViewModel>();
	readonly Config config = App.Provider.GetRequiredService<Config>();

	readonly Dictionary<string, SettingsGroupViewController> viewControllersCache = [];
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

        // UI
        SettingsGroupViewController groupViewController = new(viewModel.Group, new(config), viewControllersCache);
        groupViewController.NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Always;
        
        AddChildViewController(groupViewController);
        View!.AddSubview(groupViewController.View!);
        
        groupViewController.View!.Frame = View.Bounds;
        groupViewController.View!.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
        groupViewController.DidMoveToParentViewController(this);
	}
}