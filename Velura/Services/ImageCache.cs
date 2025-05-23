using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Logging;
using Velura.Services.Abstract;

namespace Velura.Services;

public sealed class ImageCache
{
	readonly ILogger<ImageCache> logger;
	readonly IPathResolver pathResolver;
	readonly HttpClient httpClient;
	
	readonly ConcurrentDictionary<string, Task<string?>> downloadTasksCache = new();

	public ImageCache(
		ILogger<ImageCache> logger,
		IPathResolver pathResolver,
		HttpClient httpClient)
	{
		this.logger = logger;
		this.pathResolver = pathResolver;
		this.httpClient = httpClient;
		
		if (!Directory.Exists(pathResolver.ImageCacheDirectory))
			Directory.CreateDirectory(pathResolver.ImageCacheDirectory);
		
		logger.LogInformation("[ImageCache-.ctor] ImageCache has been initialized.");
	}


	public string GetCachedFilePath(
		string imageUrl)
	{
		string encodedUrl = Convert.ToBase64String(Encoding.UTF8.GetBytes(imageUrl))
			.Replace("/", "_");
    
		return Path.Combine(pathResolver.ImageCacheDirectory, encodedUrl);
	}
	
	public async ValueTask DownloadAsync(
		string imageUrl,
		string filepath)
	{
		await using Stream networkStream = await httpClient.GetStreamAsync(imageUrl);
		await using FileStream fileStream = new(filepath, FileMode.Create, FileAccess.Write, FileShare.None);
			
		await networkStream.CopyToAsync(fileStream);
	}

	
	public async ValueTask<string?> GetAsync(
		string imageUrl)
	{
		if (downloadTasksCache.TryGetValue(imageUrl, out Task<string?>? existingTask))
			return await existingTask;
		
		TaskCompletionSource<string?> taskCompletionSource = new();
		downloadTasksCache[imageUrl] = taskCompletionSource.Task;
		
		try
		{
			string cachedFilePath = GetCachedFilePath(imageUrl);

			if (!File.Exists(cachedFilePath))
				await DownloadAsync(imageUrl, cachedFilePath);
			
			taskCompletionSource.SetResult(cachedFilePath);
			return cachedFilePath;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get image: {exception}", ex.Message);
			taskCompletionSource.SetException(ex);
			return null;
		}
		finally
		{
			downloadTasksCache.TryRemove(imageUrl, out _);
		}
	}
}