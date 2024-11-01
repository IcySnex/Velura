using System.Diagnostics.CodeAnalysis;
using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using Microsoft.Extensions.DependencyInjection;
using Velura.Helpers;
using Velura.iOS.Helpers;
using Velura.ViewModels;

namespace Velura.iOS.Views.About;

public class AboutViewController : UIViewController
{
	readonly AboutViewModel viewModel = App.Provider.GetRequiredService<AboutViewModel>();

	PrivacyViewController? privacyViewController;
	DependenciesViewController? dependenciesViewController;
	
	CAGradientLayer gradientLayer = default!;
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		privacyViewController = new();
		dependenciesViewController = new(viewModel.Dependencies);
		
		// Properties
		Title = "about_title".L10N();
		NavigationItem.RightBarButtonItem = new("close".L10N(), UIBarButtonItemStyle.Done, viewModel.CloseAboutInfoCommand.ToEvent());

		// Background
		gradientLayer = new()
		{
			Frame = View!.Bounds,
			Colors =
			[
				UIColor.SystemGray5.CGColor,
				UIColor.SystemGray6.CGColor,
			],
			StartPoint = new(0.5, 0),
			EndPoint = new(0.5, 1)
		};
		View.Layer.AddSublayer(gradientLayer);
		
		// UI
		
		UIImageView iconView = new()
		{
			Image =  UIImage.FromBundle("icon.png")
		};
		UILabel titleLabel = new()
		{
			Text = "app_title".L10N(),
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(28, UIFontWeight.Bold)),
			AdjustsFontForContentSizeCategory = true,
			TextAlignment = UITextAlignment.Center,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation,
		};
		UILabel descriptionLabel = new()
		{
			Text = "app_description".L10N(),
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(16)),
			AdjustsFontForContentSizeCategory = true,
			TextAlignment = UITextAlignment.Center,
			Lines = 0,
			LineBreakMode = UILineBreakMode.TailTruncation,
		};
		UIButton privacyButton = UIButtonConfiguration.TintedButtonConfiguration.CreateButton(
			title: "about_privacy".L10N(),
			onPress: _ => NavigationController!.PushViewController(privacyViewController, true));
		UIButton dependenciesButton = UIButtonConfiguration.TintedButtonConfiguration.CreateButton(
			title: "about_dependencies".L10N(),
			onPress: _ => NavigationController!.PushViewController(dependenciesViewController, true));
		UILabel versionLabel = new()
		{
			Text = viewModel.Version,
			TextColor = UIColor.SecondaryLabel,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(14)),
			AdjustsFontForContentSizeCategory = true,
		};
		
		View.AddSubviews(iconView, titleLabel, descriptionLabel, privacyButton, dependenciesButton, versionLabel);
		
		// Layout
		View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		View.AddConstraints(
			iconView.Width().EqualTo(130),
			iconView.Height().EqualTo(130),
			iconView.WithSameCenterX(View),
			iconView.Above(titleLabel),
			
			titleLabel.AtLeftOf(View, 30),
			titleLabel.AtRightOf(View, 30),
			titleLabel.Above(descriptionLabel),
			
			descriptionLabel.AtLeftOf(View, 30),
			descriptionLabel.AtRightOf(View, 30),
			descriptionLabel.WithSameCenterY(View).Minus(20),
			
			privacyButton.AtLeftOf(View, 30),
			privacyButton.AtRightOf(View, 30),
			privacyButton.Below(descriptionLabel, 40),
			
			dependenciesButton.AtLeftOf(View, 30),
			dependenciesButton.AtRightOf(View, 30),
			dependenciesButton.Below(privacyButton, 10),
			
			versionLabel.WithSameCenterX(View),
			versionLabel.AtBottomOf(View, 30)
		);
	}
	
	[SuppressMessage("Interoperability", "CA1422:Validate platform compatibility")]
	public override void TraitCollectionDidChange(
		UITraitCollection? previousTraitCollection)
	{
		base.TraitCollectionDidChange(previousTraitCollection);

		if (!TraitCollection.HasDifferentColorAppearanceComparedTo(previousTraitCollection))
			return;
		
		gradientLayer.Colors =
		[
			UIColor.SystemGray5.CGColor,
			UIColor.SystemGray6.CGColor,
		];
	}
}