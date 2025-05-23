using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using Velura.Helpers;
using Velura.iOS.Helpers;
using Velura.iOS.UI;
using Velura.ViewModels;

namespace Velura.iOS.Views.Info;

public class MovieInfoViewController(
	MovieInfoViewModel viewModel) : UIViewController, IUIScrollViewDelegate
{
	readonly MovieInfoViewModel viewModel = viewModel;

	NSLayoutConstraint backdropViewHeightConstraint = default!;
	NSLayoutConstraint backdropViewTopConstraint = default!;
	NSLayoutConstraint backdropBottomViewTopConstraint = default!;
	
	UIView topContainerView = default!;
	CAGradientLayer topContainerGradient = default!;
	
	UIView fadeInView = default!;
	CAGradientLayer fadeInGradient = default!;

	CAGradientLayer blurMaskLayer = default!;
	
	CAGradientLayer containerGloomGradient = default!;
	
	UIScrollView scrollView = default!;

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
		
		// Resources
		UIImage? backdropImage = await IOSApp.Images.GetASync(viewModel.Movie.BackdropUrl, false);
		UIImage? posterImage = await IOSApp.Images.GetASync(viewModel.Movie.PosterUrl);

		UIColor backgroundColor = backdropImage is not null ?
			backdropImage.GetPrimaryColor(new(backdropImage.Size.Width / 4, 0, backdropImage.Size.Width / 2, backdropImage.Size.Height))?.AdjustForWhiteForeground() ?? UIColor.Black :
			posterImage?.GetPrimaryColor(new(0, 0, posterImage.Size.Width, posterImage.Size.Height))?.AdjustForWhiteForeground() ?? UIColor.Black;
		

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
			image: UIImage.GetSystemImage("chevron.backward")?.ApplyConfiguration(UIImageSymbolConfiguration.Create(UIFont.SystemFontOfSize(16, UIFontWeight.Semibold))),
			buttonSize: UIButtonConfigurationSize.Medium,
			cornerStyle: UIButtonConfigurationCornerStyle.Capsule,
			foregroundColor: UIColor.Black.ColorWithAlpha(0.625f),
			backgroundColor: UIColor.White.ColorWithAlpha(0.5f),
			onPress: viewModel.CloseCommand.ToUIAction());
		backFloatingButton.Frame = new(12, IOSApp.IsIPad ? 33 : barHeight - 38, 32, 32);
		View!.AddSubview(backFloatingButton);

		// UI: Scroll
		scrollView = new()
		{
			ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never,
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		UIView contentView = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};

		UIImageView backdropView = new()
		{
			Image = backdropImage,
			BackgroundColor = backgroundColor,
			ContentMode = UIViewContentMode.ScaleAspectFill
		};
		UIImageView backdropBottomView = new()
		{
			Image = backdropImage?.CGImage is null ? null : UIImage.FromImage(backdropImage.CGImage, backdropImage.CurrentScale, UIImageOrientation.DownMirrored),
			BackgroundColor = backgroundColor,
			ContentMode = UIViewContentMode.ScaleAspectFill
		};
		
		fadeInView = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false,
		};
		fadeInGradient = new()
		{
			Colors =
			[
				UIColor.Black.ColorWithAlpha(0.5f).CGColor,
				UIColor.Black.ColorWithAlpha(0).CGColor
			],
			Locations =
			[
				0,
				1
			],
			StartPoint = new(0.5, 0),
			EndPoint = new(0.5, 1),
			Frame = new(View!.Frame.X, View!.Frame.Y, View!.Frame.Width, barHeight)
		};
		fadeInView.Layer.InsertSublayer(fadeInGradient, 0);

		View!.InsertSubview(scrollView, 0);
		scrollView.AddSubview(contentView);
		
		// UI: Top Container
		UIVisualEffectView blurEffectView = new(UIBlurEffect.FromStyle(UIBlurEffectStyle.SystemMaterialDark))
		{
			AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
		};

		blurMaskLayer = new()
		{
			Colors =
			[
				UIColor.Clear.CGColor,
				UIColor.White.CGColor
			],
			Locations =
			[
				0.3,
				0.5,
			],
			StartPoint = new(0.5, 0),
			EndPoint = new(0.5, 1),
			Frame = View!.Frame
		};
		blurEffectView.Layer.Mask = blurMaskLayer;
		
		topContainerView = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false,
		};
		topContainerView.AddSubviews(backdropView, backdropBottomView, blurEffectView);
		topContainerGradient = new()
		{
			Colors =
			[
				backgroundColor.ColorWithAlpha(0).CGColor,
				backgroundColor.ColorWithAlpha(0.7f).CGColor,
				backgroundColor.ColorWithLightness(0.7f).ColorWithAlpha(0.7f).CGColor,
			],
			Locations =
			[
				0.35,
				0.45,
				1,
			],
			StartPoint = new(0.5, 0),
			EndPoint = new(0.5, 1),
			Frame = View!.Frame
		};
		topContainerView.Layer.AddSublayer(topContainerGradient);
		
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
			TextColor = UIColor.White,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation,
			TextAlignment = UITextAlignment.Center
		};
		UILabel infoLabel = new()
		{
			Text = $"{(viewModel.Genres.Count > 0 ? viewModel.Genres.Join() + " • " : "")}{viewModel.Movie.ReleaseDate:yyyy} • {viewModel.Movie.Duration.L10N()}",
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(14)),
			AdjustsFontForContentSizeCategory = true,
			TextColor = UIColor.White.ColorWithAlpha(0.375f),
			Lines = 2,
			LineBreakMode = UILineBreakMode.WordWrap,
			TextAlignment = UITextAlignment.Center
		};
		ExpandableTextView descriptionLabel = new(this)
		{
			Text = viewModel.Movie.Description,
			Lines = 3,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(14)),
			AdjustsFontForContentSizeCategory = true,
			TextColor = UIColor.White.ColorWithAlpha(0.6f),
			TextAlignment = UITextAlignment.Center,
			HorizontalPadding = 24
		};
		UIButton playButton = UIButtonConfiguration.FilledButtonConfiguration.CreateButton(
			title: "media_play".L10N(),
			cornerStyle: UIButtonConfigurationCornerStyle.Capsule,
			image: UIImage.GetSystemImage("play.fill"),
			imagePadding: 8,
			imagePlacement: NSDirectionalRectEdge.Leading,
			foregroundColor: UIColor.Black,
			backgroundColor: UIColor.White.ColorWithAlpha(0.75f),
			onPress: _ => { });
		
		topContainerView.AddSubviews(imageShadowView, imageView, titleLabel, infoLabel, descriptionLabel, playButton, fadeInView);

		// UI: Bottom Container
		UIView bottomContainerView = new()
		{
			BackgroundColor = UIColor.SystemGroupedBackground,
			TranslatesAutoresizingMaskIntoConstraints = false,
		};
		containerGloomGradient = new()
		{
			Colors =
			[
				backgroundColor.ColorWithAlpha(0.3f).CGColor,
				backgroundColor.ColorWithAlpha(0).CGColor
			],
			Locations =
			[
				0,
				1
			],
			StartPoint = new(0.5, 0),
			EndPoint = new(0.5, 1),
			Frame = new(View!.Frame.X, View!.Frame.Y, View!.Frame.Width, 20)
		};
		bottomContainerView.Layer.InsertSublayer(containerGloomGradient, 0);

		UILabel anotherOneLabel = new()
		{
			Text = "Another One\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nANOTHER ONE",
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.BoldSystemFontOfSize(24)),
			Lines = 200,
			TextColor = UIColor.Label,
		};
		
		bottomContainerView.AddSubviews(anotherOneLabel);

		// Layout
		contentView.AddSubviews(topContainerView, bottomContainerView);
		
		topContainerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		bottomContainerView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		View.AddConstraints(
			// Scroll
			scrollView.AtLeftOfSafeArea(View),
			scrollView.AtRightOfSafeArea(View),
			scrollView.AtTopOf(View),
			scrollView.AtBottomOf(View),

			contentView.AtLeftOfSafeArea(View),
			contentView.AtRightOfSafeArea(View),
			contentView.AtTopOf(scrollView),
			contentView.AtBottomOf(scrollView),
			
			backdropView.Width().EqualTo().WidthOf(View),
			//backdropView.Height().EqualTo().HeightOf(View).WithMultiplier(0.45f),
			//backdropView.AtTopOf(contentView),
			
			backdropBottomView.Width().EqualTo().WidthOf(View),
			backdropBottomView.Height().EqualTo().HeightOf(backdropView),
			// backdropBottomView.Below(backdropView),
			
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
			titleLabel.Below(imageView, 32),
			
			infoLabel.AtLeftOf(topContainerView, 24),
			infoLabel.AtRightOf(topContainerView, 24),
			infoLabel.Below(titleLabel, 2),

			descriptionLabel.AtLeftOf(topContainerView, 24),
			descriptionLabel.AtRightOf(topContainerView, 24),
			descriptionLabel.Below(infoLabel, 12),
			
			playButton.WithSameCenterX(topContainerView),
			playButton.Below(descriptionLabel, 32),
			
			// Structure
			fadeInView.AtLeftOf(View),
			fadeInView.AtRightOf(View),
			fadeInView.AtTopOf(View),
			fadeInView.Height().EqualTo(barHeight),

			topContainerView.AtLeftOf(contentView),
			topContainerView.AtRightOf(contentView),
			topContainerView.AtTopOf(contentView),
			topContainerView.Height().EqualTo().HeightOf(View),
			
			blurEffectView.AtLeftOf(topContainerView),
			blurEffectView.AtRightOf(topContainerView),
			blurEffectView.AtTopOf(topContainerView),
			blurEffectView.AtBottomOf(topContainerView),
			
			bottomContainerView.Height().EqualTo().HeightOf(View),
			bottomContainerView.AtLeftOf(contentView),
			bottomContainerView.AtRightOf(contentView),
			bottomContainerView.Below(topContainerView),

			anotherOneLabel.WithSameCenterX(bottomContainerView),
			anotherOneLabel.AtTopOf(bottomContainerView, 24),
			anotherOneLabel.AtBottomOf(contentView, 24)
		);
		
		backdropViewHeightConstraint = backdropView.HeightAnchor.ConstraintEqualTo(View!.Frame.Height * 0.5f);
		backdropViewTopConstraint = backdropView.TopAnchor.ConstraintEqualTo(contentView.TopAnchor, 0);
		backdropBottomViewTopConstraint = backdropBottomView.TopAnchor.ConstraintEqualTo(contentView.TopAnchor, 0);
		View.AddConstraints(backdropViewHeightConstraint, backdropViewTopConstraint, backdropBottomViewTopConstraint);
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
		
		barHeight = (float)NavigationController!.NavigationBar.Frame.Height + (float)(IOSApp.MainWindow.WindowScene?.StatusBarManager?.StatusBarFrame.Height ?? 0);
		viewHeight = (float)View!.Frame.Height - barHeight;

		if (topContainerGradient is not null)
			topContainerGradient.Frame = View!.Frame;
		if (blurMaskLayer is not null)
			blurMaskLayer.Frame = View!.Frame;
		if (fadeInGradient is not null)
			fadeInGradient.Frame = new(View!.Frame.X, View!.Frame.Y, View!.Frame.Width, barHeight);
		
		
		if (scrollView is not null && backdropViewHeightConstraint is not null)
			Scrolled(scrollView);
	}


	public void Scrolled(
		UIScrollView scrollView)
	{
		float scrollOffset = (float)scrollView.ContentOffset.Y;
		
		float topContainerAlpha = Math.Clamp((scrollOffset - (viewHeight - barHeight) / 1.5f) * 2.5f / viewHeight, 0, 1);
		navigationBarAlpha = Math.Clamp((scrollOffset - viewHeight + barHeight) * 7.5f / (viewHeight - barHeight), 0, 1);
		
		topContainerView.Alpha = 1 - topContainerAlpha;
		containerGloomGradient.Opacity = 1 - topContainerAlpha;
		backFloatingButton.Frame = new(Math.Max(12 - navigationBarAlpha * 12, 6), backFloatingButton.Frame.Y, backFloatingButton.Frame.Width, backFloatingButton.Frame.Height);
		
		NavigationController!.NavigationBar.Alpha = navigationBarAlpha;
		backFloatingButton.Alpha = 1 - navigationBarAlpha;

		backdropViewHeightConstraint.Constant = (viewHeight + barHeight) * 0.5f - Math.Min(scrollOffset, 0);
		backdropViewTopConstraint.Constant = scrollOffset < 0 ? scrollOffset : scrollOffset / 2;
		backdropBottomViewTopConstraint.Constant = View!.Frame.Height < View!.Frame.Width ? backdropViewHeightConstraint.Constant * 1.6f + (scrollOffset < 0 ? scrollOffset * 2.6f : scrollOffset / 2) : backdropViewHeightConstraint.Constant + (scrollOffset < 0 ? scrollOffset : scrollOffset / 2);
	}
}