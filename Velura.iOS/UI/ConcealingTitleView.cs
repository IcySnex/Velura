namespace Velura.iOS.UI;

public class ConcealingTitleView : UIView
{
	readonly float barHeight = 44;
	
	readonly UILabel label;

	public ConcealingTitleView(
		string? text)
	{
		label = new()
		{
			Text = text,
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(17, UIFontWeight.Semibold)),
			AdjustsFontForContentSizeCategory = true,
			TextAlignment = UITextAlignment.Center,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation
		};
        
		CGSize labelSize = label.SizeThatFits(new(float.MaxValue, barHeight));
		Frame = new(0, 0, labelSize.Width, barHeight);
		label.Frame = new(0, barHeight, labelSize.Width, barHeight);
        
		AddSubview(label);
		ClipsToBounds = true;
	}

    
	public void ScrollViewDidScroll(
		nfloat scrollOffsetY) =>
		label.Frame = new(0, Math.Max(0, barHeight - scrollOffsetY), label.Frame.Width, barHeight);
}