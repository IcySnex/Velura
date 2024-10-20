using Microsoft.Extensions.DependencyInjection;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public class SettingsViewController : UIViewController
{
<<<<<<< HEAD
	readonly SettingsViewModel viewModel = App.Provider.GetRequiredService<SettingsViewModel>();
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

        // UI
        SettingsGroupViewController groupViewController = new(viewModel.Group);//this, viewModel.Group);
=======
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        
        // Settings UI
        SettingsGroupViewController groupViewController = new(this, ViewModel!.Group);
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
        groupViewController.NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Always;
        
        AddChildViewController(groupViewController);
        View!.AddSubview(groupViewController.View!);
        
        groupViewController.View!.Frame = View.Bounds;
        groupViewController.View!.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
        groupViewController.DidMoveToParentViewController(this);
	}
}