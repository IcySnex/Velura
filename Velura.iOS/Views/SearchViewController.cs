using Cirrious.FluentLayouts.Touch;
using Microsoft.Extensions.DependencyInjection;
using Velura.iOS.Binding;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public class SearchViewController : UIViewController
{
	readonly SearchViewModel viewModel = App.Provider.GetRequiredService<SearchViewModel>();

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		
		// UI
		UILabel label = new()
		{
			TextColor = UIColor.Label
		};

		UITextField textField = new()
		{
			TextColor = UIColor.Label
		};

		// Binding
		BindingSet<SearchViewModel> set = new(viewModel);
		Binding<SearchViewModel> labelBinding = set.Bind(label, nameof(label.Text), nameof(viewModel.HelloText));
		Binding<SearchViewModel> textFieldBinding = set.Bind(textField, nameof(textField.Text), nameof(viewModel.HelloText));

		
		UIButtonConfiguration buttonConfiguration = UIButtonConfiguration.FilledButtonConfiguration;
		buttonConfiguration.Title = "Press me!";
		UIButton button = new()
		{
			Configuration = buttonConfiguration
		};
		button.TouchUpInside += (_, _) => viewModel.SayHelloCommand.Execute("Sup Gang!");

		
		
		// Layout
		View!.AddSubview(label);
		View!.AddSubview(textField);
		View!.AddSubview(button);
		
		View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		View.AddConstraints(
			label.AtTopOfSafeArea(View),
			label.AtLeftOfSafeArea(View),
			
			textField.Below(label, 4),
			textField.AtLeftOfSafeArea(View),
			
			button.Below(textField, 4),
			button.AtLeftOfSafeArea(View)
		);
		
		
		// Dismiss Keyboard
		View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true))
		{
			CancelsTouchesInView = false,
		});
	}
}