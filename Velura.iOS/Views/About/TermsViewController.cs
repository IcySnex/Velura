using Cirrious.FluentLayouts.Touch;
using Velura.Enums;
using Velura.Helpers;
using Velura.Models;

namespace Velura.iOS.Views.About;

public class TermsViewController(
	FormattedText[] terms) : UIViewController
{
	readonly FormattedText[] terms = terms;

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		// Properties
		Title = "about_terms".L10N();
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		
		// UI
		UIScrollView scrollView = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		UILabel label = new()
		{
			AttributedText = GetFormattedText(),
			AdjustsFontForContentSizeCategory = true,
			Lines = 0,
			LineBreakMode = UILineBreakMode.WordWrap,
			TranslatesAutoresizingMaskIntoConstraints = false
		};

		View.AddSubview(scrollView);
		scrollView.AddSubview(label);
		
		// Layout
		View.AddConstraints(
			scrollView.AtLeftOf(View),
			scrollView.AtRightOf(View),
			scrollView.AtTopOfSafeArea(View),
			scrollView.AtBottomOf(View),
			
			label.AtLeftOf(View, 16),
			label.AtRightOf(View, 16),
			label.AtTopOf(scrollView),
			label.AtBottomOf(scrollView)
		);
	}


	readonly NSMutableDictionary contentAttributes = new()
	{
		{ UIStringAttributeKey.Font, UIFont.PreferredCallout },
		{ UIStringAttributeKey.ParagraphStyle,  new NSMutableParagraphStyle() { LineSpacing = 4 } }
	};
	readonly NSMutableDictionary noteAttributes = new()
	{
		{ UIStringAttributeKey.Font, UIFont.PreferredFootnote },
		{ UIStringAttributeKey.ForegroundColor, UIColor.SecondaryLabel },
	};
	readonly NSMutableDictionary headerAttributes = new()
	{
		{ UIStringAttributeKey.Font, UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.BoldSystemFontOfSize(24)) },
		{ UIStringAttributeKey.BaselineOffset, new NSNumber(6) }
	};
	
    NSAttributedString GetFormattedText()
    {
	    NSMutableAttributedString attributedString = new();
		foreach (FormattedText text in terms)
		{
			NSMutableDictionary attributes = text.Type switch
			{
				FormattedTextType.Note => noteAttributes,
				FormattedTextType.Header => headerAttributes,
				_ => contentAttributes
			};
			
			NSMutableAttributedString attributedText = new((text.Type == FormattedTextType.Header ? "\n" : "") + text.Text + '\n', attributes);
			attributedString.Append(attributedText);
		}

		return attributedString;
	}
}