namespace Velura.iOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
	public override UIWindow? Window { get; set; }

	public override bool FinishedLaunching(
		UIApplication application,
		NSDictionary launchOptions)
	{
		IOSApp _ = new();
		
		Window = new(UIScreen.MainScreen.Bounds);
		Window.RootViewController = IOSApp.MainViewController;
		Window.MakeKeyAndVisible();
		
		UIWindow.Appearance.TintColor = UIColor.FromName("AccentColor");
		UISwitch.Appearance.OnTintColor = UIColor.FromName("AccentColor");
		
		return true;
	}
}