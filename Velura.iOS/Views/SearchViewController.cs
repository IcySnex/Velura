using Cirrious.FluentLayouts.Touch;

namespace Velura.iOS.Views;

public sealed class SearchViewController : UIViewController
{
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		UILabel hello = new()
		{
			Text = "Hello World",
			Font = UIFont.PreferredHeadline,
			TextColor = UIColor.Label
		};
		
		View!.AddSubview(hello);
		
		View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		View.AddConstraints(
			hello.AtTopOfSafeArea(View),
			hello.AtLeftOfSafeArea(View)
		);
	}
}