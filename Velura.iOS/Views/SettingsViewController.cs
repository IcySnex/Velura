using Microsoft.Extensions.DependencyInjection;
using Velura.iOS.UI;
using Velura.iOS.Views.Elements;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public class SettingsViewController : UIViewController
{
	readonly SettingsViewModel viewModel = App.Provider.GetRequiredService<SettingsViewModel>();

	readonly Dictionary<string, SettingsGroupViewController> viewControllersCache = [];
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
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