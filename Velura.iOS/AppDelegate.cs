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
		
		Window = IOSApp.MainWindow;
		Window.RootViewController = IOSApp.MainViewController;
		Window.MakeKeyAndVisible();

		UIColor? accentColor = UIColor.FromName("AccentColor");
		UIWindow.Appearance.TintColor = accentColor;
		UISwitch.Appearance.OnTintColor = accentColor;
		
		return true;
	}
}