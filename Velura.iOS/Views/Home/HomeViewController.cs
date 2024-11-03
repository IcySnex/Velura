using Microsoft.Extensions.DependencyInjection;
using Velura.Helpers;
using Velura.ViewModels;

namespace Velura.iOS.Views.Home;

public class HomeViewController : UIViewController
{
	readonly HomeViewModel viewModel = App.Provider.GetRequiredService<HomeViewModel>();

	public HomeViewController()
	{
		Title = "home_title".L10N();
		TabBarItem.Image = UIImage.GetSystemImage("house");
		TabBarItem.SelectedImage = UIImage.GetSystemImage("house.fill");
	}
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		// Properties
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
	}
}