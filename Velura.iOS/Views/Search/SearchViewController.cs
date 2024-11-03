using Cirrious.FluentLayouts.Touch;
using Velura.Helpers;

namespace Velura.iOS.Views.Search;

public sealed class SearchViewController : UIViewController
{
	public SearchViewController()
	{
		Title = "search_title".L10N();
		TabBarItem.Image = UIImage.GetSystemImage("magnifyingglass");
		TabBarItem.SelectedImage = UIImage.GetSystemImage("text.magnifyingglass");
	}
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		// Properties
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		
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