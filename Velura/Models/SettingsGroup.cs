using Velura.Models.Attributes;

namespace Velura.Models;

public sealed class SettingsGroup(
	DetailsAttribute details,
	ImageAttribute image,
	IReadOnlyList<SettingsGroup> groups,
	IReadOnlyList<SettingsProperty> properties,
	string path)
{
	public DetailsAttribute Details { get; } = details;

	public ImageAttribute Image { get; } = image;
	
	public IReadOnlyList<SettingsGroup> Groups { get; } = groups;

	public IReadOnlyList<SettingsProperty> Properties { get; } = properties;
	
	public string Path { get; } = path;
}