using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public sealed class PathResolver : IPathResolver
{
	static readonly string ApplicationFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
	
	static readonly string CacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Cache"); // DEBUG PURPOSES
	
	
	public string CurrentLogFile { get; } = Path.Combine(CacheFolder, "Logs", "Log-.log");
	
	public string ImageCacheDirectory { get; } = Path.Combine(CacheFolder, "Images");

	public string Database { get; } = Path.Combine(ApplicationFolder, "Database.db3");
}