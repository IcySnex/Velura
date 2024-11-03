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

	TermsViewController termsViewController = default!;
	DependenciesViewController dependenciesViewController = default!;
	
	CAGradientLayer gradientLayer = default!;
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		termsViewController = new(viewModel.Terms);
		dependenciesViewController = new(viewModel.Dependencies, viewModel.ShowDependencySourceCommand);
		
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
			title: "about_terms".L10N(),
			onPress: _ => NavigationController!.PushViewController(termsViewController, true));
		UIButton dependenciesButton = UIButtonConfiguration.TintedButtonConfiguration.CreateButton(
			title: "about_dependencies".L10N(),
			onPress: _ => NavigationController!.PushViewController(dependenciesViewController, true));
		UILabel versionLabel = new()
		{
			Text = "about_version".L10N(viewModel.Version),
			TextColor = UIColor.SecondaryLabel,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(14)),
			AdjustsFontForContentSizeCategory = true,
		};
		UIView footerDivider = new()
		{
			BackgroundColor = UIColor.SystemGray,
			Layer =
			{
				CornerRadius = 2
			}
		};
		UIButton supportButton = UIButtonConfiguration.PlainButtonConfiguration.CreateButton(
			title: "about_contact".L10N(),
			buttonSize: UIButtonConfigurationSize.Small,
			onPress: viewModel.ShowContactEmailComposerCommand.ToUIAction());
		UIView footerView = new();
		
		footerView.AddSubviews(versionLabel, footerDivider, supportButton);
		View.AddSubviews(iconView, titleLabel, descriptionLabel, privacyButton, dependenciesButton, footerView);
		
		// Layout
		footerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
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
			
			privacyButton.AtLeftOf(View, 16),
			privacyButton.AtRightOf(View, 16),
			privacyButton.Below(descriptionLabel, 40),
			
			dependenciesButton.AtLeftOf(View, 16),
			dependenciesButton.AtRightOf(View, 16),
			dependenciesButton.Below(privacyButton, 10),
			
			footerView.WithSameCenterX(View),
			footerView.AtBottomOf(View, 30),
			footerView.Height().EqualTo(25), 
			footerView.Width().EqualTo(200),
			
			versionLabel.WithSameCenterY(footerView),
			versionLabel.AtLeftOf(footerView, 16),

			footerDivider.Width().EqualTo(4),
			footerDivider.Height().EqualTo(4),
			footerDivider.WithSameCenterY(footerView),
			footerDivider.ToRightOf(versionLabel, 10),

			supportButton.WithSameCenterY(footerView),
			supportButton.ToRightOf(footerDivider)
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