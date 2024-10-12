namespace Velura.Models.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class ImageAttribute(
	string resourceName,
	string backgroundColor,
	string? tintColor) : Attribute
{
	public string ResourceName { get; } = resourceName;

	public Color BackgroundColor { get; } = Color.FromHex(backgroundColor);

	public Color? TintColor { get; } = tintColor is null ? null : Color.FromHex(tintColor);
}