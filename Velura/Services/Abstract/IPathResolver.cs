namespace Velura.Services.Abstract;

public interface IPathResolver
{
	string CurrentLogFile { get; }
	
	string ImageCacheDirectory { get; }
	
	string Database { get; }
}