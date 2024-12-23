using System.Reflection;
using System.Windows.Input;
using CoreAnimation;

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
	
	public static EventInfo GetEvent(
		this Type type,
		string eventPath) =>
		type.GetEvent(eventPath) ?? throw new InvalidOperationException($"Event path '{eventPath}' is invalid for type '{type.Name}'.");

	public static EventInfo GetEvent(
		this object instance,
		string eventPath) =>
		GetEvent(instance.GetType(), eventPath);
	
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


	public static EventHandler ToEvent(
		this ICommand command,
		object? parameter = null) =>
		(_, _) =>
		{
			if (command.CanExecute(parameter))
				command.Execute(parameter);
		};

	public static UIActionHandler ToUIActionHandler(
		this ICommand command,
		object? parameter = null) =>
		_ =>
		{
			if (command.CanExecute(parameter))
				command.Execute(parameter);
		};
	
	public static UIAction ToUIAction(
		this ICommand command,
		object? parameter = null) =>
		UIAction.Create(command.ToUIActionHandler(parameter));


	public static UIButton CreateButton(
		this UIButtonConfiguration configuration,
		string title,
		string? subTitle = null,
		UIButtonConfigurationSize buttonSize = UIButtonConfigurationSize.Large,
		UIButtonConfigurationCornerStyle cornerStyle = UIButtonConfigurationCornerStyle.Medium,
		UIImage? image = null,
		float imagePadding = 4f,
		NSDirectionalRectEdge imagePlacement = NSDirectionalRectEdge.Trailing,
		UIAction? onPress = null)
	{
		configuration.Title = title;
		configuration.Subtitle = subTitle;
		configuration.ButtonSize = buttonSize;
		configuration.CornerStyle = cornerStyle;
		configuration.Image = image;
		configuration.ImagePadding = imagePadding;
		configuration.ImagePlacement = imagePlacement;
		
		return UIButton.GetButton(configuration, onPress);
	}

	public static UIButton CreateButton(
		this UIButtonConfiguration configuration,
		string title,
		string? subTitle = null,
		UIButtonConfigurationSize buttonSize = UIButtonConfigurationSize.Large,
		UIButtonConfigurationCornerStyle cornerStyle = UIButtonConfigurationCornerStyle.Medium,
		UIImage? image = null,
		float imagePadding = 4f,
		NSDirectionalRectEdge imagePlacement = NSDirectionalRectEdge.Trailing,
		UIActionHandler? onPress = null) =>
		configuration.CreateButton(title, subTitle, buttonSize, cornerStyle, image, imagePadding, imagePlacement, onPress is null ? null : UIAction.Create(onPress));


	public static UITargetedPreview? CreateTargetedPreview(
		this UIView? view,
		int padding,
		int cornerRadius)
	{
		if (view is null)
			return null;
		
		return new(view, new()
		{
			VisiblePath = UIBezierPath.FromRoundedRect(view.Frame.Inset(-padding, -padding), cornerRadius)
		});
	}
	
	
	public static UINavigationController WrapInNavController(
		this UIViewController viewController,
		bool preferLargeTitles = false) =>
		new(viewController)
		{
			NavigationBar =
			{
				PrefersLargeTitles = preferLargeTitles
			}
		};
	

	public static UIColor ToUIColor(
		this string hex)
	{
		byte r = Convert.ToByte(hex[1..3], 16);
		byte g = Convert.ToByte(hex[3..5], 16);
		byte b = Convert.ToByte(hex[5..7], 16);
		byte a = hex.Length == 9 ? Convert.ToByte(hex[7..9], 16) : (byte)255;
		
		return UIColor.FromRGBA(r, g, b, a);
	}
	
	
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
		CGSize imageSize,
		UIColor? backgroundColor = null,
		UIColor? tintColor = null) =>
		new UIGraphicsImageRenderer(size).CreateImage(context =>
		{
			if (backgroundColor is not null)
			{
				backgroundColor.SetFill();
				context.FillRect(new(0, 0, size.Width, size.Height));
			}

			double ratio = Math.Min(imageSize.Width / image.Size.Width, imageSize.Height / image.Size.Height);
			double newWidth = image.Size.Width * ratio;
			double newHeight = image.Size.Height * ratio;
			
			UIImage imageToDraw = tintColor is null ? image : image.ApplyTintColor(tintColor, UIImageRenderingMode.AlwaysTemplate);
			imageToDraw.Draw(new CGRect((size.Width - newWidth) / 2, (size.Height - newHeight) / 2, newWidth, newHeight));
		});
	
	public static UIImage Apply(
		this UIImage image,
		CGSize size,
		CGSize imageSize,
		UIColor backgroundColorStart,
		UIColor backgroundColorEnd,
		UIColor? tintColor = null) =>
		new UIGraphicsImageRenderer(size).CreateImage(context =>
		{
			using CGGradient gradient = new(CGColorSpace.CreateDeviceRGB(), [backgroundColorStart.CGColor, backgroundColorEnd.CGColor]);
			context.CGContext.DrawLinearGradient(gradient, new(0, 0), new(0, size.Height), CGGradientDrawingOptions.None);

			double ratio = Math.Min(imageSize.Width / image.Size.Width, imageSize.Height / image.Size.Height);
			double newWidth = image.Size.Width * ratio;
			double newHeight = image.Size.Height * ratio;
			
			UIImage imageToDraw = tintColor is null ? image : image.ApplyTintColor(tintColor, UIImageRenderingMode.AlwaysTemplate);
			imageToDraw.Draw(new CGRect((size.Width - newWidth) / 2, (size.Height - newHeight) / 2, newWidth, newHeight));
		});
}