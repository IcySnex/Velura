using Velura.Helpers;

namespace Velura.iOS.Views.About;

public class PrivacyViewController : UIViewController
{
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		Title = "about_privacy".L10N();
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
	}
}