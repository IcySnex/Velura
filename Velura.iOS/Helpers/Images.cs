using System.Collections.Concurrent;
using Velura.Services;

namespace Velura.iOS.Helpers;

public class Images(
	ImageCache imageCache)
{
	readonly ImageCache imageCache = imageCache;
	readonly ConcurrentDictionary<string, UIImage?> created = new();
	
	
	public readonly UIImage? Placeholder = UIImage.GetSystemImage("photo.badge.exclamationmark")?.Apply(
		new(100, 150), new(36, 36),
		UIColor.FromRGB(102, 102, 102), UIColor.FromRGB(32, 32, 32), UIColor.FromRGBA(200, 200, 200, 138));


	public async Task<UIImage?> GetASync(
		string? url)
	{
		if (url is null)
			return Placeholder;
		if (created.TryGetValue(url, out UIImage? cachedImage))
			return cachedImage;
		
		string? filePath = await imageCache.GetAsync(url);
		
		UIImage? image = filePath is null ? Placeholder : UIImage.FromFile(filePath);
		created[url] = image;

		return image;
	}
}