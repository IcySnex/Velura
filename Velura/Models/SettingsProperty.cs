using Velura.Models.Attributes;

namespace Velura.Models;

public sealed class SettingsProperty(
	DetailsAttribute details,
	string path,
	Type type)
{
	public DetailsAttribute Details { get; } = details;

	public string Path { get; } = path;

	public Type Type { get; } = type;
}