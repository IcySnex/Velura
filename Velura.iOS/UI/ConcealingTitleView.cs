namespace Velura.iOS.UI;

public class ConcealingTitleView : UIView
{
	const float BarHeight = 44;
	
	
	readonly UILabel label;

	public ConcealingTitleView(
		string? text)
	{
		label = new()
		{
			Text = text,
			Font = UIFont.SystemFontOfSize(17, UIFontWeight.Semibold),
			TextAlignment = UITextAlignment.Center,
			Lines = 1
		};
        
		CGSize labelSize = label.SizeThatFits(new(float.MaxValue, BarHeight));
		Frame = new(0, 0, labelSize.Width, BarHeight);
		label.Frame = new(0, BarHeight, labelSize.Width, BarHeight);
        
		AddSubview(label);
		ClipsToBounds = true;
	}

    
	public void ScrollViewDidScroll(
		nfloat scrollOffsetY) =>
		label.Frame = new(0, Math.Max(0, BarHeight - scrollOffsetY), label.Frame.Width, BarHeight);
}