using Velura.iOS.Views.Abstract;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public sealed class SettingsViewController() : TabbedViewController<SettingsViewModel>("Settings", "gearshape", "gearshape.fill")
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        
        SettingsGroupViewController groupViewController = new(ViewModel!.Group);
        groupViewController.NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Always;
        
        AddChildViewController(groupViewController);
        View!.AddSubview(groupViewController.View!);
        
        groupViewController.View!.Frame = View.Bounds;
        groupViewController.View!.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
        groupViewController.DidMoveToParentViewController(this);
    }
}