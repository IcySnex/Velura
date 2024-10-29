using Cirrious.FluentLayouts.Touch;

namespace Velura.iOS.UI;

public class UIPaddedView : UIView
{
	CGRect padding = new(0, 0, 0, 0);

	public CGRect Padding
	{
		get => padding;
		set
		{
			padding = value;

			if (ChildView is null)
				return;
			
			this.RemoveConstraints(Constraints);
			this.AddConstraints(
				ChildView.AtTopOf(this, value.Y),
				ChildView.AtBottomOf(this, value.Height),
				ChildView.AtLeftOf(this, value.X),
				ChildView.AtRightOf(this, value.Width)
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
				value.AtTopOf(this, Padding.Y),
				value.AtBottomOf(this, Padding.Height),
				value.AtLeftOf(this, Padding.X),
				value.AtRightOf(this, Padding.Width)
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
			Frame.Width - Padding.X - Padding.Width,
			Frame.Height - Padding.Y - Padding.Height);
		CGSize actualChildSize = ChildView.SizeThatFits(preferredChildSize);
		
		Frame = new(
			Frame.X,
			Frame.Y,
			actualChildSize.Width + Padding.X + Padding.Width,
			actualChildSize.Height + Padding.Y + Padding.Height);
	}
}