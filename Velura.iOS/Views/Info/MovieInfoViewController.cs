using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using Velura.iOS.Helpers;
using Velura.iOS.Views.Home;
using Velura.Models;
using Velura.ViewModels;

namespace Velura.iOS.Views.Info;

public class MovieInfoViewController(
	MovieInfoViewModel viewModel) : UIViewController, IUIScrollViewDelegate
{
	readonly MovieInfoViewModel viewModel = viewModel;

	UIView topContainerView = default!;
	CAGradientLayer topContainerGradient = default!;

	UIButton backFloatingButton = default!;
	float barHeight = 0;
	float viewHeight = 0;
	float navigationBarAlpha = 0;

	public override async void ViewDidLoad()
	{
		base.ViewDidLoad();

		// Properties
		Title = viewModel.Movie.Title;
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;
		
		UIImage? backBarImage = UIImage.GetSystemImage("chevron.backward")?.ApplyConfiguration(UIImageSymbolConfiguration.Create(UIFont.SystemFontOfSize(18, UIFontWeight.Semibold)));
		UIBarButtonItem backBarItem = new(backBarImage, UIBarButtonItemStyle.Plain, viewModel.CloseCommand.ToEvent());
		backBarItem.ImageInsets = new(0, -8, 0, 0);
		backBarItem.TintColor = UIColor.Clear;
		NavigationItem.LeftBarButtonItem = backBarItem;
		
		// Navigation Bar
		barHeight = (float)NavigationController!.NavigationBar.Frame.Height + (float)(IOSApp.MainWindow.WindowScene?.StatusBarManager?.StatusBarFrame.Height ?? 0);
		viewHeight = (float)View!.Frame.Height - barHeight;
		
		NavigationController!.NavigationBar.ScrollEdgeAppearance = new()
		{
			BackgroundColor = null,
			ShadowColor = null,
			BackgroundEffect = null,
			TitleTextAttributes = new() { ForegroundColor = UIColor.Clear }
		};
		
		backFloatingButton = UIButtonConfiguration.TintedButtonConfiguration.CreateButton(
			title: "",
			image: UIImage.GetSystemImage("chevron.backward"),
			buttonSize: UIButtonConfigurationSize.Medium,
			cornerStyle: UIButtonConfigurationCornerStyle.Capsule,
			onPress: viewModel.CloseCommand.ToUIAction());
		backFloatingButton.Frame = new(12, IOSApp.IsIPad ? 33 : barHeight - 38, 32, 32);
		View!.AddSubview(backFloatingButton);

		// UI: Scroll
		UIScrollView scrollView = new()
		{
			ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never,
			Delegate = this,
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		UIView contentView = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};

		View!.InsertSubview(scrollView, 0);
		scrollView.AddSubview(contentView);
		
		// UI: Top Container
		UIImage? posterImage = await IOSApp.Images.GetASync(viewModel.Movie.PosterUrl);
		UIColor averageColor = posterImage?.GetAverageColor() ?? UIColor.Black;
		
		topContainerView = new()
		{
			Layer =
			{
				ShadowColor = averageColor.CGColor,
				ShadowOpacity = 0.5f,
				ShadowRadius = 14f,
				ShadowOffset = new(0, 8),
				ShouldRasterize = true,
				RasterizationScale = UIScreen.MainScreen.Scale
			},
			TranslatesAutoresizingMaskIntoConstraints = false,
		};
		topContainerGradient = new()
		{
			Colors =
			[
				averageColor.ColorWithAlpha(0).CGColor,
				averageColor.ColorWithAlpha(0.3f).CGColor,
				averageColor.ColorWithAlpha(0.5f).CGColor,
				averageColor.ColorWithAlpha(0.8f).CGColor
			],
			Locations =
			[
				0,
				0.35,
				0.40,
				0.55,
				1
			],
			StartPoint = new(0.5, 0),
			EndPoint = new(0.5, 1)
		};
		topContainerView.Layer.InsertSublayer(topContainerGradient, 0);

		UIView imageShadowView = new()
		{
			Layer =
			{
				ShadowColor = UIColor.Black.CGColor,
				ShadowPath = UIBezierPath.FromRoundedRect(new(0, 0, 140, 210), 8f).CGPath,
				ShadowOpacity = 0.5f,
				ShadowRadius = 12f,
				ShadowOffset = new(0, 6),
				ShouldRasterize = true,
				RasterizationScale = UIScreen.MainScreen.Scale,
			}
		};
		UIImageView imageView = new()
		{
			Image = posterImage,
			ClipsToBounds = true,
			Layer =
			{
				BorderColor = UIColor.FromRGBA(128, 128, 128, 150).CGColor,
				BorderWidth = 1,
				CornerRadius = 8,
				MasksToBounds = true
			}
		};
		UILabel titleLabel = new()
		{
			Text = viewModel.Movie.Title,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.BoldSystemFontOfSize(24)),
			AdjustsFontForContentSizeCategory = true,
			TextColor = UIColor.Label,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation,
			TextAlignment = UITextAlignment.Center
		};
		UILabel descriptionLabel = new()
		{
			Text = viewModel.Movie.Description,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(14)),
			AdjustsFontForContentSizeCategory = true,
			TextColor = UIColor.SecondaryLabel,
			Lines = 3,
			LineBreakMode = UILineBreakMode.TailTruncation,
			TextAlignment = UITextAlignment.Center
		};
		
		topContainerView.AddSubviews(imageShadowView, imageView, titleLabel, descriptionLabel);

		// UI: Bottom Container
		UILabel anotherOneLabel = new()
		{
			Text = "Another One\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nANOTHER ONE",
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.BoldSystemFontOfSize(24)),
			Lines = 200,
			TextColor = UIColor.Label,
		};

		contentView.AddSubviews(topContainerView, anotherOneLabel);
		
		
		// Layout
		contentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		topContainerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		View.AddConstraints(			
			// Scroll
			scrollView.AtLeftOfSafeArea(View),
			scrollView.AtRightOfSafeArea(View),
			scrollView.AtTopOf(View),
			scrollView.AtBottomOf(View),
			
			contentView.AtTopOf(scrollView),
			contentView.AtBottomOf(scrollView),
			contentView.Width().EqualTo().WidthOf(scrollView),
			
			// Top Container
			imageShadowView.WithSameCenterX(topContainerView),
			imageShadowView.WithSameCenterY(topContainerView).Minus(40),
			imageShadowView.Width().EqualTo(140),
			imageShadowView.Height().EqualTo(210),
			
			imageView.WithSameCenterX(topContainerView),
			imageView.WithSameCenterY(topContainerView).Minus(40),
			imageView.Width().EqualTo(140),
			imageView.Height().EqualTo(210),

			titleLabel.AtLeftOf(topContainerView, 24),
			titleLabel.AtRightOf(topContainerView, 24),
			titleLabel.Below(imageView, 40),
			
			descriptionLabel.AtLeftOf(topContainerView, 24),
			descriptionLabel.AtRightOf(topContainerView, 24),
			descriptionLabel.Below(titleLabel, 6),
			
			// Structure
			topContainerView.AtLeftOf(contentView),
			topContainerView.AtRightOf(contentView),
			topContainerView.AtTopOf(contentView),
			topContainerView.Height().EqualTo().HeightOf(View),

			anotherOneLabel.WithSameCenterX(contentView),
			anotherOneLabel.Below(topContainerView, 24),
			anotherOneLabel.AtBottomOf(contentView, 24).Minus(IOSApp.MainViewController.TabBar.Frame.Height)
		);
	}

	public override void ViewDidAppear(
		bool animated)
	{
		base.ViewDidAppear(animated);

		NavigationController!.NavigationBar.Alpha = navigationBarAlpha;
		NavigationController!.NavigationBar.ScrollEdgeAppearance = default!;
		NavigationItem.LeftBarButtonItem!.TintColor = default!;
	}


	public override void ViewDidLayoutSubviews()
	{
		base.ViewDidLayoutSubviews();

		topContainerGradient.Frame = View!.Bounds;
		
		barHeight = (float)NavigationController!.NavigationBar.Frame.Height + (float)(IOSApp.MainWindow.WindowScene?.StatusBarManager?.StatusBarFrame.Height ?? 0);
		viewHeight = (float)View!.Frame.Height - barHeight;
	}


	public void Scrolled(
		UIScrollView scrollView)
	{
		float scrollOffset = (float)scrollView.ContentOffset.Y;
		
		float topContainerAlpha = Math.Clamp((scrollOffset - (viewHeight - barHeight) / 1.5f) * 2.5f / viewHeight, 0, 1);
		navigationBarAlpha = Math.Clamp((scrollOffset - viewHeight + barHeight) * 7.5f / (viewHeight - barHeight), 0, 1);
		
		topContainerView.Alpha = 1 - topContainerAlpha;
		backFloatingButton.Frame = new(Math.Max(12 - navigationBarAlpha * 12, 6), backFloatingButton.Frame.Y, backFloatingButton.Frame.Width, backFloatingButton.Frame.Height);
		
		NavigationController!.NavigationBar.Alpha = navigationBarAlpha;
	}
}