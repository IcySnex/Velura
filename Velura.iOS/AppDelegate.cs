using Microsoft.Extensions.DependencyInjection;
using Velura.iOS.Views;

namespace Velura.iOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
	public override UIWindow? Window { get; set; }

	public override bool FinishedLaunching(
		UIApplication application,
		NSDictionary launchOptions)
	{
		IOSApp.Initialize();
		
		Window = new(UIScreen.MainScreen.Bounds);
		Window.RootViewController = App.Provider.GetRequiredService<MainViewController>();
		Window.MakeKeyAndVisible();
		
		UIWindow.Appearance.TintColor = UIColor.FromName("AccentColor");
		UISwitch.Appearance.OnTintColor = UIColor.FromName("AccentColor");
<<<<<<< HEAD
		
		return true;
=======

		return result;
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
	}
}