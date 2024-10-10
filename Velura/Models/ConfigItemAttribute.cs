namespace Velura.Models;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigItem(
	string name,
	string description,
	string group,
	object defaultValue) : Attribute
{
	public string Name { get; } = name;

	public string Description { get; } = description;
	
	public string Group { get; } = group;
	
	public object DefaultValue { get; } = defaultValue;
}