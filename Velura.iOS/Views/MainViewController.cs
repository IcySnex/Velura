using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using Velura.ViewModels;

namespace Velura.iOS.Views;

[MvxRootPresentation(WrapInNavigationController = true)]
public sealed class MainViewController : MvxViewController<MainViewModel>
{
	UILabel? helloLabel;
	UITextField? helloEdit;
	UIButton? clicksButton;
	UIButton? navigateButton;

	public override void LoadView()
	{
		base.LoadView();
		
		if (View is null) 
			throw new NullReferenceException(nameof(View));

		View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true)));
		
		
		View.BackgroundColor = UIColor.SystemBackground;
		Title = "Hello MvvmCross";

		helloLabel = new UILabel
		{
			TranslatesAutoresizingMaskIntoConstraints = false,
		};

		Add(helloLabel);

		helloLabel.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 16).Active = true;
		helloLabel.LeadingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.LeadingAnchor, 16).Active = true;
		helloLabel.TrailingAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TrailingAnchor, -16).Active = true;
		
		helloEdit = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false,
			Placeholder = "Enter text",
		};

		Add(helloEdit);

		helloEdit.TopAnchor.ConstraintEqualTo(helloLabel.BottomAnchor, 16).Active = true;
		helloEdit.LeadingAnchor.ConstraintEqualTo(helloLabel.LeadingAnchor).Active = true;
		helloEdit.TrailingAnchor.ConstraintEqualTo(helloLabel.TrailingAnchor).Active = true;

		clicksButton = new UIButton
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		clicksButton.BackgroundColor = UIColor.FromName("AccentColor");
		clicksButton.SetTitleColor(UIColor.White, UIControlState.Normal);
		clicksButton.Layer.CornerRadius = 8f;
		clicksButton.ClipsToBounds = true;
		clicksButton.Frame = new(50, 100, 200, 50);
		clicksButton.TouchDown += (_, _) => 
		{
			UIView.Animate(0.1,
				() => clicksButton.Transform = CGAffineTransform.MakeScale(0.95f, 0.95f),
				() => UIView.Animate(0.1, () => clicksButton.Transform = CGAffineTransform.MakeScale(1f, 1f)));
		};


		Add(clicksButton);

		clicksButton.TopAnchor.ConstraintEqualTo(helloEdit.BottomAnchor, 32).Active = true;
		clicksButton.LeadingAnchor.ConstraintEqualTo(helloEdit.LeadingAnchor).Active = true;
		clicksButton.TrailingAnchor.ConstraintEqualTo(helloEdit.TrailingAnchor).Active = true;

		navigateButton = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		navigateButton.SetTitle("Navigate to Details View", UIControlState.Normal);
		navigateButton.SetTitleColor(UIColor.LightText, UIControlState.Normal);

		Add(navigateButton);

		navigateButton.TopAnchor.ConstraintEqualTo(clicksButton.BottomAnchor, 32).Active = true;
		navigateButton.LeadingAnchor.ConstraintEqualTo(helloEdit.LeadingAnchor).Active = true;
		navigateButton.TrailingAnchor.ConstraintEqualTo(helloEdit.TrailingAnchor).Active = true;
	}

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();

		using var set = CreateBindingSet();
		set.Bind(helloLabel).To(vm => vm.Hello);
		set.Bind(helloEdit).To(vm => vm.Hello);
		set.Bind(clicksButton).For(v => v.BindTouchUpInside()).To(vm => vm.ClickCommand);
		set.Bind(clicksButton).For(v => v.BindTitle()).To(vm => vm.ClickText);
		set.Bind(navigateButton).For(v => v.BindTouchUpInside()).To(vm => vm.NavigateCommand);
	}
}