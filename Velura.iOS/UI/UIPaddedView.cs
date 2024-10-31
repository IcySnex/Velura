using Cirrious.FluentLayouts.Touch;

namespace Velura.iOS.UI;

public class UIPaddedView : UIView
{
	UIEdgeInsets padding = new(0, 0, 0, 0);

	public UIEdgeInsets Padding
	{
		get => padding;
		set
		{
			padding = value;

			if (ChildView is null)
				return;
			
			this.RemoveConstraints(Constraints);
			this.AddConstraints(
				ChildView.AtTopOf(this, value.Top),
				ChildView.AtBottomOf(this, value.Bottom),
				ChildView.AtLeftOf(this, value.Left),
				ChildView.AtRightOf(this, value.Right)
			);
		}
	}
	
	
	UIView? childView;
	
	public UIView? ChildView
	{
		get => childView;
		set
		{
			childView = value;
			
			if (value is null)
			{
				UIView? existingView = Subviews.FirstOrDefault();
			
				existingView?.RemoveFromSuperview();
				existingView?.Dispose();
				return;
			}
			
			SetContentHuggingPriority((float)UILayoutPriority.DefaultHigh, UILayoutConstraintAxis.Horizontal);
			SetContentHuggingPriority((float)UILayoutPriority.DefaultHigh, UILayoutConstraintAxis.Vertical);
			AddSubview(value);
		
			value.TranslatesAutoresizingMaskIntoConstraints = false;
			this.AddConstraints(
				value.AtTopOf(this, Padding.Top),
				value.AtBottomOf(this, Padding.Bottom),
				value.AtLeftOf(this, Padding.Left),
				value.AtRightOf(this, Padding.Right)
			);

			LayoutIfNeeded();
		}
	}


	public override void LayoutSubviews()
	{
		base.LayoutSubviews();

		if (ChildView is null)
			return;

		CGSize preferredChildSize = new(
			Frame.Width - Padding.Left - Padding.Right,
			Frame.Height - Padding.Top - Padding.Bottom);
		CGSize actualChildSize = ChildView.SizeThatFits(preferredChildSize);
		
		Frame = new(
			Frame.X,
			Frame.Y,
			actualChildSize.Width + Padding.Left + Padding.Right,
			actualChildSize.Height + Padding.Top + Padding.Bottom);
	}
}