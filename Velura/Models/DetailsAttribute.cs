namespace Velura.Models;

[AttributeUsage(AttributeTargets.Property)]
public class DetailsAttribute(
	string name,
	string description) : Attribute
{
	public string Name { get; } = name;
	
	public string Description { get; } = description;
}