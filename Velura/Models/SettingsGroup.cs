using Velura.Models.Attributes;

namespace Velura.Models;

public class SettingsGroup(
	DetailsAttribute details,
	ImageAttribute image,
	IReadOnlyList<SettingsGroup> groups,
	IReadOnlyList<SettingsProperty> properties)
{
	public DetailsAttribute Details { get; } = details;

	public ImageAttribute Image { get; } = image;
	
	public IReadOnlyList<SettingsGroup> Groups { get; } = groups;

	public IReadOnlyList<SettingsProperty> Properties { get; } = properties;
}