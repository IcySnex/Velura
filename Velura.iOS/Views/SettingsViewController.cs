using Velura.iOS.Views.Abstract;
using Velura.Models.Abstract;
using Velura.Models.Models;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public sealed class SettingsViewController() : TabbedViewController<SettingsViewModel>("Settings", "gearshape", "gearshape.fill")
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
    }
}
