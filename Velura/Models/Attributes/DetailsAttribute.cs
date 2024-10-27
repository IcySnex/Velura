namespace Velura.Models.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class DetailsAttribute(
	string name,
	string description) : NameAttribute(name)
{
	public string Description { get; } = description;
}