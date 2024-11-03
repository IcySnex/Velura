namespace Velura.Services.Abstract;

public interface IPathResolver
{
	string CurrentLogFile { get; }
	
	string Database { get; }
}