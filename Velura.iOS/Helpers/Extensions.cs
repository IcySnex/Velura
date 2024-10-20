using CoreAnimation;
using Velura.Models;

namespace Velura.iOS.Helpers;

public static class Extensions
{
	public static UIColor ToUIColor(
		this Color color) =>
		UIColor.FromRGBA(color.Red, color.Green, color.Blue, color.Alpha);
	
	public static void AnimateBounce(
		this UIView view,
		double duration = 0.1,
		float bounceAmount = 1.2f)
	{
		CABasicAnimation animation = CABasicAnimation.FromKeyPath("transform.scale");
		animation.Duration = duration;
		animation.From = NSNumber.FromFloat(1.0f);
		animation.To = NSNumber.FromFloat(bounceAmount);
		animation.AutoReverses = true;
		animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
        
		view.Layer.AddAnimation(animation, "bounce");
	}
}