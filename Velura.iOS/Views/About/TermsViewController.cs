using Velura.Helpers;

namespace Velura.iOS.Views.About;

public class TermsViewController : UIViewController
{
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		Title = "about_terms".L10N();
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
	}
}