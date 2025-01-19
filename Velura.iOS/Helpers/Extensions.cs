using System.Collections.Specialized;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;
using CoreAnimation;
using CoreImage;
using CoreVideo;
using Velura.iOS.Models;

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
		UIColor? foregroundColor = null,
		UIColor? backgroundColor = null,
		UIAction? onPress = null)
	{
		configuration.Title = title;
		configuration.Subtitle = subTitle;
		configuration.ButtonSize = buttonSize;
		configuration.CornerStyle = cornerStyle;
		configuration.Image = image;
		configuration.ImagePadding = imagePadding;
		configuration.ImagePlacement = imagePlacement;
		if (foregroundColor is not null)
			configuration.BaseForegroundColor = foregroundColor;
		if (backgroundColor is not null)
			configuration.Background.BackgroundColor = backgroundColor;
		
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
		UIColor? foregroundColor = null,
		UIColor? backgroundColor = null,
		UIActionHandler? onPress = null) =>
		configuration.CreateButton(title, subTitle, buttonSize, cornerStyle, image, imagePadding, imagePlacement, foregroundColor, backgroundColor, onPress is null ? null : UIAction.Create(onPress));


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
		bool preferLargeTitles = false,
		UIViewControllerTransition? preferredTransition = null) =>
		new(viewController)
		{
			NavigationBar =
			{
				PrefersLargeTitles = preferLargeTitles
			},
			PreferredTransition = preferredTransition
		};


	public static void ReloadData(
		this UICollectionView collectionView,
		int sectionIndex,
		NotifyCollectionChangedEventArgs e)
	{
		switch (e.Action)
		{
			case NotifyCollectionChangedAction.Move when e.NewItems is not null && e.OldItems is not null:
				if (e.NewItems.Count != 1 && e.OldItems.Count != 1)
				{
					collectionView.ReloadData();
					break;
				}
				
				NSIndexPath beforeMoveIndexPath = NSIndexPath.FromItemSection(e.OldStartingIndex, sectionIndex);
				NSIndexPath afterMoveIndexPath = NSIndexPath.FromItemSection(e.NewStartingIndex, sectionIndex);
				
				collectionView.MoveItem(beforeMoveIndexPath, afterMoveIndexPath);
				break;
			case NotifyCollectionChangedAction.Replace when e.NewItems is not null && e.OldItems is not null:
				NSIndexPath[] replacedIndexPath = [NSIndexPath.FromItemSection(e.OldStartingIndex, sectionIndex)];
				
				collectionView.ReloadItems(replacedIndexPath);
				break;
			case NotifyCollectionChangedAction.Add when e.NewItems is not null:
				NSIndexPath[] addIndexPaths = new NSIndexPath[e.NewItems.Count];
				for (int index = 0; index < addIndexPaths.Length; index++)
					addIndexPaths[index] = NSIndexPath.FromItemSection(e.NewStartingIndex + index, sectionIndex);
					
				collectionView.InsertItems(addIndexPaths);
				break;
			case NotifyCollectionChangedAction.Remove when e.OldItems is not null:
				NSIndexPath[] removeIndexPaths = new NSIndexPath[e.OldItems.Count];
				for (int index = 0; index < removeIndexPaths.Length; index++)
					removeIndexPaths[index] = NSIndexPath.FromItemSection(e.OldStartingIndex + index, sectionIndex);
					
				collectionView.DeleteItems(removeIndexPaths);
				break;
			case NotifyCollectionChangedAction.Reset when e.OldItems is not null:
				NSIndexPath[] resetIndexPaths = new NSIndexPath[e.OldItems.Count];
				for (int index = 0; index < resetIndexPaths.Length; index++)
					resetIndexPaths[index] = NSIndexPath.FromItemSection(index, sectionIndex);
					
				collectionView.DeleteItems(resetIndexPaths);
				break;
			default:
				collectionView.ReloadData();
				break;
		}
	}
	

	public static UIColor ToUIColor(
		this string hex)
	{
		byte r = Convert.ToByte(hex[1..3], 16);
		byte g = Convert.ToByte(hex[3..5], 16);
		byte b = Convert.ToByte(hex[5..7], 16);
		byte a = hex.Length == 9 ? Convert.ToByte(hex[7..9], 16) : (byte)255;
		
		return UIColor.FromRGBA(r, g, b, a);
	}

	public static UIColor Invert(
		this UIColor color)
	{
		color.GetRGBA(out nfloat red, out nfloat green, out nfloat blue, out nfloat alpha);

		red = 1 - red;
		green = 1 - green;
		blue = 1 - blue;

		return new(red, green, blue, alpha);
	}
	
	public static UIColor ColorWithLightness(
		this UIColor color,
		float lightnessMultiplier)
	{
		color.GetRGBA(out nfloat red, out nfloat green, out nfloat blue, out nfloat alpha);
		
		red = Math.Clamp((float)red * lightnessMultiplier, 0, 1);
		green = Math.Clamp((float)green * lightnessMultiplier, 0, 1);
		blue = Math.Clamp((float)blue * lightnessMultiplier, 0, 1);

		return new(red, green, blue, alpha);
	}

	public static double CalculateLuminance(
		this UIColor color)
	{
		color.GetRGBA(out nfloat red, out nfloat green, out nfloat blue, out nfloat _);
		
		double redRatio = red <= 0.03928f ? red / 12.92f : Math.Pow((red + 0.055f) / 1.055f, 2.4f);
		double greenRatio = green <= 0.03928f ? green / 12.92f : Math.Pow((green + 0.055f) / 1.055f, 2.4f);
		double blueRatio = blue <= 0.03928f ? blue / 12.92f : Math.Pow((blue + 0.055f) / 1.055f, 2.4f);
		
		return 0.2126 * redRatio + 0.7152 * greenRatio + 0.0722 * blueRatio;
	}

	public static double CalculateContrast(
		this UIColor color,
		UIColor otherColor)
	{
		double luminance1 = color.CalculateLuminance();
		double luminance2 = otherColor.CalculateLuminance();
		
		if (luminance1 < luminance2)
			(luminance1, luminance2) = (luminance2, luminance1);
		
		return (luminance1 + 0.05) / (luminance2 + 0.05);
	}

	public static UIColor GetForegroundColor(
		this UIColor backgroundColor)
	{
		UIColor white = UIColor.White;
		UIColor black = UIColor.Black;
		
		return backgroundColor.CalculateContrast(white) >= backgroundColor.CalculateContrast(black) ? white : black;
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

	
	public static CGImage? Resize(
		this UIImage image,
		CGSize size,
		CGRect bounds)
	{
		if (image.CGImage is not CGImage inputImage)
			return null;

		if (inputImage.WithImageInRect(bounds) is not CGImage croppedImage)
			return null;

		UIGraphicsImageRendererFormat format = new()
		{
			Scale = 1,
			PreferredRange = UIGraphicsImageRendererFormatRange.Standard
		};
		UIGraphicsImageRenderer renderer = new(size, format);
		
		UIImage resizedImage = renderer.CreateImage(_ => UIImage.FromImage(croppedImage).Draw(new CGRect(0, 0, size.Width, size.Height)));
		return resizedImage.CGImage;
	}

	public static List<Vector3> GetPixels(
		this CGImage cgImage)
	{
		CGDataProvider dataProvider = cgImage.DataProvider;
		if (dataProvider is null)
			return [];
        
		NSData? imageData = dataProvider.CopyData();
		if (imageData is null)
			return [];
		
		CGImageByteOrderInfo byteOrder = cgImage.ByteOrderInfo;
		int width = (int)cgImage.Width;
		int height = (int)cgImage.Height;
		int size = width * height;

		byte[] buffer = new byte[size * 4];
		Marshal.Copy(imageData.Bytes, buffer, 0, buffer.Length);

		List<Vector3> result = new(size);
		for (int i = 0; i < size; i++)
		{
			int offset = i * 4;
			byte r, g, b;

			switch (byteOrder)
			{
				case CGImageByteOrderInfo.ByteOrderDefault:
				case CGImageByteOrderInfo.ByteOrder32Big:
					r = buffer[offset];
					g = buffer[offset + 1];
					b = buffer[offset + 2];
					break;
				case CGImageByteOrderInfo.ByteOrder32Little:
					r = buffer[offset + 2];
					g = buffer[offset + 1];
					b = buffer[offset];
					break;
				default:
					throw new NotSupportedException("Unsupported byte order.");
			}

			Vector3 color = new(r / 255.0f, g / 255.0f, b / 255.0f);
			result.Add(color);
		}

		return result;
	}
	
	public static UIColor ToUIColor(
		this Vector3 vector) =>
		UIColor.FromRGB(vector.X, vector.Y, vector.Z);

	public static Vector3 ToVector3(
		this UIColor color)
	{
		color.GetRGBA(out nfloat r, out nfloat g, out nfloat b, out nfloat _);
		return new((float)r, (float)g, (float)b);
	}

	public static UIColor? GetPrimaryColor(
		this UIImage image,
		CGRect bounds)
	{
		Stopwatch watch = new();
		watch.Start();
		
		if (image.Resize(new(100, 100), bounds) is not CGImage resizedImage)
			return null;
		
		// string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		// string filePath = Path.Combine(documentsPath, "hello.png");
		// UIImage.FromImage(resizedImage).AsPNG().Save(filePath, false);
		
		// List<Vector3> pixels = resizedImage.GetPixels();
		// IEnumerable<Cluster> clusters = Cluster.Create(pixels, 3).OrderByDescending(c => c.Points.Count);
		//
		// Cluster? primaryCluster = clusters.FirstOrDefault();
		// UIColor? color = primaryCluster?.Center.ToUIColor();

		CIFilter? filter = CIFilter.FromName("CIKMeans");
		if (filter is null)
			return null;
		
		filter.SetValueForKey((CIImage)resizedImage, CIFilterInputKey.Image);
		filter.SetValueForKey(CIVector.Create(bounds), CIFilterInputKey.Extent);
		filter.SetValueForKey(NSArray.FromObjects(CIColor.RedColor), new("inputMeans"));
		filter.SetValueForKey(NSNumber.FromInt32(3), new("inputCount"));
		filter.SetValueForKey(NSNumber.FromInt32(100), new("inputPasses"));
		filter.SetValueForKey(NSNumber.FromBoolean(false), new("inputPerceptual"));
		
		CIImage? outputImage = filter.OutputImage;
		if (outputImage is null)
			return null;
		
		byte[] bitmap = new byte[4];
		unsafe
		{
			CIContext context = CIContext.FromOptions(null);
		
			fixed (byte* bitmapPtr = bitmap)
				context.RenderToBitmap(outputImage.CreateBySettingAlphaOne(outputImage.Extent), (IntPtr)bitmapPtr, 4, new(0, 0, 1, 1), (int)CIFormat.kRGBA8, null);
		}
		
		UIColor color = new(bitmap[0] / 255f, bitmap[1] / 255f, bitmap[2] / 255f, bitmap[3] / 255f);
		
		Console.WriteLine(watch.ElapsedMilliseconds);
		watch.Stop();
		
		return color;
	}
	
	public static UIColor? GetAverageColor(
		this UIImage image)
	{
		CIImage inputImage = CIImage.FromCGImage(image.CGImage!);
		if (inputImage == null)
			return null;
		
		CIFilter? filter = CIFilter.FromName("CIAreaAverage");
		if (filter is null)
			return UIColor.Black;
		
		filter.SetValueForKey(inputImage, CIFilterInputKey.Image);
		filter.SetValueForKey(CIVector.Create(inputImage.Extent), CIFilterInputKey.Extent);
		
		CIImage? outputImage = filter.OutputImage;
		if (outputImage is null)
			return null;
		
		CIContext context = CIContext.FromOptions(new()
		{
			WorkingColorSpace = CGColorSpace.CreateDeviceRGB()
		});
		byte[] bitmap = new byte[4];
		
		unsafe
		{
			fixed (byte* bitmapPtr = bitmap)
				context.RenderToBitmap(outputImage, (IntPtr)bitmapPtr, 4, new(0, 0, 1, 1), (int)CIFormat.kRGBA8, null);
		}
		
		return new(bitmap[0] / 255f, bitmap[1] / 255f, bitmap[2] / 255f, bitmap[3] / 255f);
	}
}