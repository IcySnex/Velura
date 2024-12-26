using Cirrious.FluentLayouts.Touch;
using Velura.ViewModels;

namespace Velura.iOS.Views.Info;

public class MovieInfoViewController : UIViewController
{
	readonly MovieInfoViewModel viewModel;
	
	public MovieInfoViewController(
		MovieInfoViewModel viewModel)
	{
		this.viewModel = viewModel;

		// Properties
		Title = viewModel.Movie.Title;
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;
		
		// UI
		UILabel title = new()
		{
			Text = viewModel.Movie.Title,
			Font = UIFont.PreferredHeadline,
			TextColor = UIColor.Label
		};
		
		View!.AddSubview(title);
		
		// Layout
		View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		View.AddConstraints(
			title.WithSameCenterY(View),
			title.WithSameCenterX(View)
		);
	}
}