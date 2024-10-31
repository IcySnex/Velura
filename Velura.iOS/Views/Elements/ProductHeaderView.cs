using System.Windows.Input;
using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using Velura.Helpers;

namespace Velura.iOS.Views.Elements;

public class ProductHeaderView : UIView
{
	readonly ICommand showAboutInfoCommand;
	
	readonly UILabel textLabel;
	readonly UILabel secondaryTextLabel;
	
	public ProductHeaderView(
		ICommand showAboutInfoCommand)
	{
		this.showAboutInfoCommand = showAboutInfoCommand;
		
		// Properties
		BackgroundColor = UIColor.SecondarySystemGroupedBackground;
		Layer.CornerRadius = 8;
		Layer.MasksToBounds = true;

		// UI
		UIImageView imageView = new()
		{
			Image =  UIImage.FromBundle("icon.png")
		};
		textLabel = new()
		{
			Text = "app_title".L10N(),
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(20, UIFontWeight.Bold)),
			AdjustsFontForContentSizeCategory = true,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation,
		};
		secondaryTextLabel = new()
		{
			Text = "app_description".L10N(),
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(14)),
			AdjustsFontForContentSizeCategory = true,
			AdjustsFontSizeToFitWidth = true,
			MinimumScaleFactor = 0.8f,
			Lines = 2,
			LineBreakMode = UILineBreakMode.TailTruncation,
		};
		UIImageView chevronView = new()
		{
			Image =  UIImage.GetSystemImage("chevron.compact.right")?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate)
		};
		
		AddSubviews(imageView, textLabel, secondaryTextLabel, chevronView);
		
		// Layout
		this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		this.AddConstraints(
			imageView.Width().EqualTo(70),
			imageView.Height().EqualTo(70),
			imageView.WithSameCenterY(this),
			imageView.AtLeftOf(this, 10),
			
			textLabel.ToRightOf(imageView, 5),
			textLabel.AtRightOf(chevronView, 20),
			textLabel.AtTopOf(this, 12),
			
			secondaryTextLabel.ToRightOf(imageView, 5),
			secondaryTextLabel.AtRightOf(chevronView, 30),
			secondaryTextLabel.Below(textLabel, -4),
			secondaryTextLabel.AtBottomOf(this, 16),
			
			chevronView.Width().EqualTo(14),
			chevronView.Height().EqualTo(16),
			chevronView.WithSameCenterY(this),
			chevronView.AtRightOf(this, 16)
		);
	}


	public override CGSize SizeThatFits(
		CGSize size)
	{
		CGSize textLabelSize = textLabel.SizeThatFits(new(size.Width - 30, float.MaxValue));
		CGSize secondaryTextLabelSize = secondaryTextLabel.SizeThatFits(new(size.Width - 30, float.MaxValue));
		
		return new(size.Width, 20 + textLabelSize.Height - 4 + secondaryTextLabelSize.Height + 20);
	}
	
	
	public override void TouchesBegan(NSSet touches, UIEvent? evt)
	{
		base.TouchesBegan(touches, evt);
		
		BackgroundColor = UIColor.SystemGray4;
	}

	public override void TouchesEnded(NSSet touches, UIEvent? evt)
	{
		base.TouchesEnded(touches, evt);
		
		showAboutInfoCommand.Execute(null);
		Animate(CATransaction.AnimationDuration, () => BackgroundColor = UIColor.SecondarySystemGroupedBackground);
	}

	public override void TouchesCancelled(NSSet touches, UIEvent? evt)
	{
		base.TouchesCancelled(touches, evt);
		
		BackgroundColor = UIColor.SecondarySystemGroupedBackground;
	}
}