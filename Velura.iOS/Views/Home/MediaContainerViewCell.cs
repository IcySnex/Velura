using System.Collections.Concurrent;
using Cirrious.FluentLayouts.Touch;
using ObjCRuntime;
using Velura.iOS.Helpers;
using Velura.Models.Abstract;
using Velura.Services;

namespace Velura.iOS.Views.Home;

public class MediaContainerViewCell : UICollectionViewCell
{
	static readonly UIImage? PlaceholderImage = UIImage.GetSystemImage("photo.badge.exclamationmark")?.Apply(
		new(100, 150), new(36, 36),
		UIColor.FromRGB(102, 102, 102), UIColor.FromRGB(32, 32, 32), UIColor.FromRGBA(200, 200, 200, 138));
	static readonly ConcurrentDictionary<string, UIImage?> CreatedUIImageCache = new();

	
	readonly UIImageView imageView;
	readonly UILabel textLabel;
	readonly UILabel secondaryTextLabel;
	
	public MediaContainerViewCell(
		NativeHandle handle) : base(handle)
	{
		// UI
		imageView = new()
		{
			Image = PlaceholderImage,
			ClipsToBounds = true,
			Layer =
			{
				BorderColor = UIColor.FromRGBA(128, 128, 128, 150).CGColor,
				BorderWidth = 1,
				CornerRadius = 8,
				MasksToBounds = true
			}
		};
		textLabel = new()
		{
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(16)),
			AdjustsFontForContentSizeCategory = true,
			TextColor = UIColor.Label,
			Lines = 2,
			LineBreakMode = UILineBreakMode.TailTruncation
		};
		secondaryTextLabel = new()
		{
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.SystemFontOfSize(13)),
			AdjustsFontForContentSizeCategory = true,
			TextColor = UIColor.SecondaryLabel,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation
		};

		ContentView.AddSubviews(imageView, textLabel, secondaryTextLabel);
		
		// Layout
		ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		ContentView.AddConstraints(
			imageView.Height().EqualTo().WidthOf(imageView).WithMultiplier(1.5f),
			imageView.AtTopOf(ContentView),
			imageView.AtLeftOf(ContentView),
			imageView.AtRightOf(ContentView),
			
			textLabel.Below(imageView, 2),
			textLabel.AtLeftOf(ContentView),
			textLabel.AtRightOf(ContentView),
			
			secondaryTextLabel.Below(textLabel,2),
			secondaryTextLabel.AtLeftOf(ContentView),
			secondaryTextLabel.AtRightOf(ContentView),
			secondaryTextLabel.AtBottomOf(ContentView, 2)
		);
	}
	
	
	void SetText(
		string text,
		string? secondaryText)
	{
		textLabel.Text = text;
		secondaryTextLabel.Text = secondaryText;
	}
	
	async Task SetImageAsync(
		string? url,
		ImageCache imageCache,
		int index)
	{
		if (url is null)
		{
			imageView.Image = PlaceholderImage;
			return;
		}
		if (CreatedUIImageCache.TryGetValue(url, out UIImage? cachedImage))
		{
			imageView.Image = cachedImage;
			return;
		}
		
		string? filePath = await imageCache.GetAsync(url);
		if (index != Tag)
			return;
		
		cachedImage = filePath is null ? PlaceholderImage : UIImage.FromFile(filePath);
		CreatedUIImageCache[url] = cachedImage;
		Transition(imageView, 0.15, UIViewAnimationOptions.TransitionCrossDissolve, () => imageView.Image = cachedImage, null);
	}
	
	
	public async void UpdateCell(
		IMediaContainer mediaContainer,
		ImageCache imageCache,
		int index)
	{
		Tag = index;
		
		SetText(mediaContainer.Title, mediaContainer.ReleaseDate?.ToString("dd. MMM yyyy"));
		await SetImageAsync(mediaContainer.PosterUrl, imageCache, index);
	}
}