namespace Velura.Models.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class NameAttribute(
	string name) : Attribute
{
	public string Name { get; } = name;
}