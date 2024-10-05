using MvvmCross.Platforms.Ios.Core;

namespace Velura.iOS;

[Register(nameof(AppDelegate))]
public class AppDelegate : MvxApplicationDelegate<Setup, App>
{
	public override bool FinishedLaunching(
		UIApplication application,
		NSDictionary launchOptions)
	{
		bool result = base.FinishedLaunching(application, launchOptions);

		UIWindow.Appearance.TintColor = UIColor.FromName("AccentColor");

		return result;
	}
}
