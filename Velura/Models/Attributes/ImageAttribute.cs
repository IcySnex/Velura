namespace Velura.Models.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class ImageAttribute(
	string resourceName,
	string backgroundColor,
	string tintColor) : Attribute
{
	public string ResourceName { get; } = resourceName;

	public string BackgroundColor { get; } = backgroundColor;

	public string TintColor { get; } = tintColor;
}