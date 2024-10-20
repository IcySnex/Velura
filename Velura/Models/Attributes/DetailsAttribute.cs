namespace Velura.Models.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public sealed class DetailsAttribute(
	string name,
	string description) : Attribute
{
	public string Name { get; } = name;
	
	public string Description { get; } = description;
}