namespace Velura.Models;

public sealed class Dependency(
	string name,
	string author,
	string version,
	string url)
{
	public string Name { get; } = name;
	
	public string Author { get; } = author;
	
	public string Version { get; } = version;
	
	public string Url { get; } = url;
}