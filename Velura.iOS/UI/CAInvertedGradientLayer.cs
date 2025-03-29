using CoreAnimation;

namespace Velura.iOS.UI;

public class CAInvertedGradientLayer : CALayer
{
	public bool Hidden { get; set; } = false;
	public nfloat Offset { get; set; } = 0;
	public nfloat LineHeight { get; set; } = 0;
	public nfloat GradWidth { get; set; } = 0;

	public override void DrawInContext(
		CGContext ctx)
	{
		base.DrawInContext(ctx);

		ctx.SetFillColor(UIColor.Gray.CGColor);
		CGRect rect = Bounds;

		if (Hidden)
		{
			ctx.FillRect(rect);
			return;
		}
		
		rect.Height -= LineHeight;
		ctx.FillRect(rect);

		CGColor[] colors = [UIColor.Gray.CGColor, UIColor.Gray.ColorWithAlpha(0.0f).CGColor];
		nfloat[] locations = [0, 1];

		CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();
		using CGGradient gradient = new(colorSpace, colors, locations);

		CGPoint startPoint = new(Bounds.GetMaxX() - GradWidth - Offset, 0);
		CGPoint endPoint = new(Bounds.GetMaxX() - Offset, 0);

		rect.Height = Bounds.Height;
		ctx.AddRect(rect);
		
		ctx.Clip();
		ctx.DrawLinearGradient(gradient, startPoint, endPoint, CGGradientDrawingOptions.DrawsBeforeStartLocation);
	}
}