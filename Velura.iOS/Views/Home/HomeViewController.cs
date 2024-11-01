using Microsoft.Extensions.DependencyInjection;
using Velura.ViewModels;

namespace Velura.iOS.Views.Home;

public class HomeViewController : UIViewController
{
	readonly HomeViewModel viewModel = App.Provider.GetRequiredService<HomeViewModel>();
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		UILabel label = new()
		{
			Text = viewModel.HelloText,
			TextColor = UIColor.Label,
		};
		View!.AddSubview(label);
	}
}