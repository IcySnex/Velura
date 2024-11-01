using Cirrious.FluentLayouts.Touch;
using Velura.iOS.Helpers;

namespace Velura.iOS.Views.Settings;

public class SettingsGroupHeaderView : UIView
{
	readonly UILabel textLabel;
	readonly UILabel secondaryTextLabel;
	
	public SettingsGroupHeaderView(
		string text,
		string secondaryText,
		UIImage? image,
		UIColor? imageBackgroundColor = null,
		UIColor? imageTintColor = null)
	{
		// Properties
		BackgroundColor = UIColor.SecondarySystemGroupedBackground;
		Layer.CornerRadius = 8;
		Layer.MasksToBounds = true;

		// UI
		UIImageView imageView = new()
		{
			Image =  image?.Apply(new(56, 56), new(44, 44), imageBackgroundColor, imageTintColor),
			ClipsToBounds = true,
			Layer =
			{
				CornerRadius = 12,
				MasksToBounds = true
			}
		};
		textLabel = new()
		{
			Text = text,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(20, UIFontWeight.Bold)),
			AdjustsFontForContentSizeCategory = true,
			TextAlignment = UITextAlignment.Center,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation,
		};
		secondaryTextLabel = new()
		{
			Text = secondaryText,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(14)),
			AdjustsFontForContentSizeCategory = true,
			TextAlignment = UITextAlignment.Center,
			Lines = 0,
			LineBreakMode = UILineBreakMode.WordWrap,
		};
		
		AddSubviews(imageView, textLabel, secondaryTextLabel);
		
		// Layout
		this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		this.AddConstraints(
			imageView.Width().EqualTo(56),
			imageView.Height().EqualTo(56),
			imageView.WithSameCenterX(this),
			imageView.AtTopOf(this, 25),
			
			textLabel.AtLeftOf(this, 15),
			textLabel.AtRightOf(this, 15),
			textLabel.Below(imageView, 15),
			
			secondaryTextLabel.AtLeftOf(this, 15),
			secondaryTextLabel.AtRightOf(this, 15),
			secondaryTextLabel.Below(textLabel, 6),
			secondaryTextLabel.AtBottomOf(this, 20)
		);
	}


	public override CGSize SizeThatFits(
		CGSize size)
	{
		CGSize textLabelSize = textLabel.SizeThatFits(new(size.Width - 30, float.MaxValue));
		CGSize secondaryTextLabelSize = secondaryTextLabel.SizeThatFits(new(size.Width - 30, float.MaxValue));
		
		return new(size.Width, 25 + 56 + 15 + textLabelSize.Height + 6 + secondaryTextLabelSize.Height + 20);
	}
}