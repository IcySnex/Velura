using Cirrious.FluentLayouts.Touch;
using Microsoft.Extensions.DependencyInjection;
using Velura.Helpers;
using Velura.iOS.Helpers;
using Velura.iOS.Views.Info;
using Velura.Services.Abstract;
using Velura.ViewModels;

namespace Velura.iOS.UI;

public class ExpandableTextView : UIView
{
	readonly UILabel textLabel;
	readonly UILabel moreLabel;
	readonly CAInvertedGradientLayer gradientLayer;

	bool isTruncated = false;
	
	public ExpandableTextView(
		UIViewController parentController)
	{
		// UI
		textLabel = new()
		{
			LineBreakMode = UILineBreakMode.TailTruncation
		};
		moreLabel = new()
		{
			Text = "more".L10N(),
			TextColor = UIColor.FromName("AccentColor")?.ColorWithLightness(1.5f)
		};
		AddSubviews(textLabel, moreLabel);

		gradientLayer = new()
		{
			GradWidth = 60
		};
		textLabel.Layer.AddSublayer(gradientLayer);
		textLabel.Layer.Mask = gradientLayer;

		// Layout
		this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		this.AddConstraints(
			textLabel.AtLeftOf(this),
			textLabel.AtRightOf(this),
			textLabel.AtTopOf(this),
			textLabel.AtBottomOf(this),
			
			moreLabel.AtRightOf(this),
			moreLabel.AtBottomOf(this)
		);

		// Press
		UITapGestureRecognizer tapGesture = new(() =>
		{
			if (!isTruncated || Text is null)
				return;

			UINavigationController viewController = new(new TextViewController(Text, "description".L10N(), parentController));
			parentController.PresentViewController(viewController, true, null);
		});
		AddGestureRecognizer(tapGesture);
	}


	public string? MoreText
	{
		get => moreLabel.Text;
		set
		{
			moreLabel.Text = value;
			
			Update();
		}
	}

	public string? Text
	{
		get => textLabel.Text;
		set
		{
			textLabel.Text = value;
			
			Update();
		}
	}

	int lines = 3;
	public int Lines
	{
		get => lines;
		set
		{
			lines = value;
			textLabel.Lines = lines;
			
			Update();
		}
	}

	public bool AdjustsFontForContentSizeCategory
	{
		get => textLabel.AdjustsFontForContentSizeCategory;
		set
		{
			textLabel.AdjustsFontForContentSizeCategory = value;
			moreLabel.AdjustsFontForContentSizeCategory = value;
			
			Update();
		}
	}
	
	public UIFont Font
	{
		get => textLabel.Font;
		set
		{
			textLabel.Font = value;
			moreLabel.Font = value;
			
			Update();
		}
	}
	
	public UIColor? TextColor 
	{
		get => textLabel.TextColor;
		set => textLabel.TextColor = value;
	}
	
	public UITextAlignment TextAlignment 
	{
		get => textLabel.TextAlignment;
		set => textLabel.TextAlignment = value;
	}
	
	public int HorizontalPadding { get; set; }

	
	void Update()
	{
	}

	public override void LayoutSubviews()
	{
		base.LayoutSubviews();
		
		CGSize maxSize = new(Superview.Frame.Width - HorizontalPadding * 2, nfloat.MaxValue);
		
		CGRect fullTextSize = new NSString(textLabel.Text).GetBoundingRect(maxSize, NSStringDrawingOptions.UsesLineFragmentOrigin, new() { Font = textLabel.Font }, null);
		CGRect moreTextSize = new NSString(moreLabel.Text).GetBoundingRect(maxSize, NSStringDrawingOptions.UsesLineFragmentOrigin, new() { Font = moreLabel.Font }, null);
		
		isTruncated = fullTextSize.Height / Font.LineHeight > Lines;
		moreLabel.Hidden = !isTruncated;
		gradientLayer.Hidden = !isTruncated;
		gradientLayer.Offset = moreTextSize.Width;
		gradientLayer.LineHeight = Font.LineHeight;
		gradientLayer.Frame = new(0, 0, fullTextSize.Width, Font.LineHeight * Lines);
		gradientLayer.SetNeedsDisplay();
	}
}