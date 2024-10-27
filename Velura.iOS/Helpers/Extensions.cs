using System.Reflection;
using CoreAnimation;
using Velura.Models;

namespace Velura.iOS.Helpers;

public static class Extensions
{
	public static PropertyInfo GetProperty(
		this Type type,
		string propertyPath) =>
		type.GetProperty(propertyPath) ?? throw new InvalidOperationException($"Property path '{propertyPath}' is invalid for type '{type.Name}'.");

	public static PropertyInfo GetProperty(
		this object instance,
		string propertyPath) =>
		GetProperty(instance.GetType(), propertyPath);
	
	public static T GetValue<T>(
		this PropertyInfo property,
		object instance)
	{
		if (property.GetValue(instance) is T value)
			return value;
		
		throw new InvalidOperationException($"Property value of '{property.Name}' is not of expected type '{typeof(T).Name}'.");
	}
	
	public static object? GetPropertyValue(
		this object instance,
		string propertyPath)
	{
		PropertyInfo property = instance.GetProperty(propertyPath);
		return property.GetValue(instance);
	}
	
	public static T GetPropertyValue<T>(
		this object instance,
		string propertyPath)
	{
		PropertyInfo property = instance.GetProperty(propertyPath);
		return property.GetValue<T>(instance);
	}
	
	
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
	
	
	public static UIImage Apply(
		this UIImage image,
		CGSize size,
		UIColor? backgroundColor = null,
		UIColor? tintColor = null) =>
		new UIGraphicsImageRenderer(size).CreateImage(context =>
		{
			if (backgroundColor is not null)
			{
				backgroundColor.SetFill();
				context.FillRect(new(0, 0, size.Width, size.Height));
			}

			UIImage imageToDraw = tintColor is null ? image : image.ApplyTintColor(tintColor, UIImageRenderingMode.AlwaysTemplate);
			imageToDraw.Draw(new CGPoint((size.Width - image.Size.Width) / 2, (size.Height - image.Size.Height) / 2));
		});
}