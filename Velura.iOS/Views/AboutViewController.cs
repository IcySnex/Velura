using Cirrious.FluentLayouts.Touch;
using Microsoft.Extensions.DependencyInjection;
using Velura.Helpers;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public class AboutViewController : UIViewController
{
	readonly AboutViewModel viewModel = App.Provider.GetRequiredService<AboutViewModel>();
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		// Properties
		Title = "about_title".L10N();
		View!.BackgroundColor = UIColor.SystemGroupedBackground;

		// UI
		UINavigationBar navigationBar = new()
		{
			Items =
			[
				new(Title)
				{
					RightBarButtonItem = new("close".L10N(), UIBarButtonItemStyle.Done, (_, _) => viewModel.CloseAboutInfoCommand.Execute(null))
					
				}
			],
		};
		
		UILabel label = new()
		{
			Text = viewModel.Version,
			TextColor = UIColor.Label,
		};
		
		View.AddSubviews(navigationBar, label);
		
		// Layout
		View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		View.AddConstraints(
			navigationBar.Height().EqualTo(54),
			navigationBar.AtTopOf(View),
			navigationBar.AtLeftOf(View),
			navigationBar.AtRightOf(View),
			
			label.Below(navigationBar, 15),
			label.WithSameCenterX(View)
		);
	}
}