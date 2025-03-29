using Cirrious.FluentLayouts.Touch;
using Velura.Helpers;

namespace Velura.iOS.Views.Info;

public class TextViewController : UIViewController
{
	public TextViewController(
		string text,
		string title,
		UIViewController parentController)
	{
		// Properties
		Title = title;
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationItem.RightBarButtonItem = new("close".L10N(), UIBarButtonItemStyle.Done, (_, _) => parentController.DismissViewController(true, null));
		
		// UI
		UIScrollView scrollView = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		UILabel label = new()
		{
			Text = text,
			AdjustsFontForContentSizeCategory = true,
			Lines = 0,
			LineBreakMode = UILineBreakMode.WordWrap,
			TranslatesAutoresizingMaskIntoConstraints = false
		};

		View.AddSubview(scrollView);
		scrollView.AddSubview(label);
		
		// Layout
		View.AddConstraints(
			scrollView.AtLeftOf(View),
			scrollView.AtRightOf(View),
			scrollView.AtTopOfSafeArea(View),
			scrollView.AtBottomOf(View),
			
			label.AtLeftOf(View, 16),
			label.AtRightOf(View, 16),
			label.AtTopOf(scrollView),
			label.AtBottomOf(scrollView)
		);
	}
}