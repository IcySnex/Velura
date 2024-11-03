using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public sealed class PathResolver : IPathResolver
{
	public string CurrentLogFile { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Logs", "Log-.log");

	public string Database { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Database.db3");
}