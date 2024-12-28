using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using Velura.iOS.Helpers;
using Velura.iOS.Views.Home;
using Velura.ViewModels;

namespace Velura.iOS.Views.Info;

public class MovieInfoViewController : UIViewController, IUIScrollViewDelegate
{
	readonly MovieInfoViewModel viewModel;

	UIView topContainerView = default!;
	CAGradientLayer topContainerGradient = default!;
	
	UIView? navigationBarBackgroundView;
	readonly UILabel navigationBarTitleView;

	public MovieInfoViewController(
		MovieInfoViewModel viewModel)
	{
		this.viewModel = viewModel;

		// Properties
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;

		// Title
		navigationBarTitleView =  new()
		{
			Alpha = 0,
			Text = viewModel.Movie.Title,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(17, UIFontWeight.Semibold)),
			AdjustsFontForContentSizeCategory = true,
			TextAlignment = UITextAlignment.Center,
			Lines = 1
		};
		
		CGSize labelSize = navigationBarTitleView.SizeThatFits(new(float.MaxValue, 44));
		navigationBarTitleView.Frame = new(0, 0, labelSize.Width, 44);
		
		NavigationItem.TitleView = navigationBarTitleView;
		
		// Transition
		PreferredTransition = UIViewControllerTransition.Zoom(null, context =>
		{
			UICollectionViewController sourceController = (UICollectionViewController)context.SourceViewController;
			MovieInfoViewController thisController = (MovieInfoViewController)context.ZoomedViewController;

			IOSApp.MainViewController.SetTabBarHidden(!thisController.WillDisappear, true);
				
			UICollectionViewCell? cell = sourceController.CollectionView.VisibleCells.FirstOrDefault(
				c => c is MediaContainerViewCell mc && mc.MediaContainer == viewModel.Movie);
			return cell?.ContentView.Subviews[0]!;
		});
	}

	public override async void ViewDidLoad()
	{
		base.ViewDidLoad();

		// Images
		UIImage? posterImage = await IOSApp.Images.GetASync(viewModel.Movie.PosterUrl);
		UIColor averageColor = posterImage?.GetAverageColor() ?? UIColor.Black;

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

		View!.AddSubview(scrollView);
		scrollView.AddSubview(contentView);

		// UI: Top Container
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


	public bool WillDisappear { get; set; } = false;
	
	public override void ViewWillAppear(
		bool animated)
	{
		WillDisappear = false;
		base.ViewWillAppear(animated);
	}
	
	public override void ViewWillDisappear(
		bool animated)
	{
		if (navigationBarBackgroundView is not null)
			navigationBarBackgroundView.Alpha = 1;
		
		WillDisappear = true;
		base.ViewWillDisappear(animated);
	}

	
	public override void ViewDidLayoutSubviews()
	{
		base.ViewDidLayoutSubviews();

		topContainerGradient.Frame = View!.Bounds;
	}
	

	public void Scrolled(
		UIScrollView scrollView)
	{
		int topBarHeight = (int)NavigationController!.NavigationBar.Frame.Height + (int)(View!.Window.WindowScene?.StatusBarManager?.StatusBarFrame.Height ?? 0);
		float value = (float)Math.Clamp((scrollView.ContentOffset.Y - View!.Bounds.Height / 2) / (View!.Bounds.Height - topBarHeight * 2.25), 0, 1) * 2;
		float delayedValue = (float)Math.Clamp((value - 0.9) / 0.1, 0, 1);

		topContainerView.Alpha = 1 - value;

		navigationBarBackgroundView ??= NavigationController!.NavigationBar.Subviews[0];
		navigationBarBackgroundView.Alpha = delayedValue;
		navigationBarTitleView.Alpha = delayedValue; // WTF WHY NOT ALWAYS ZERO?=???
	}
}