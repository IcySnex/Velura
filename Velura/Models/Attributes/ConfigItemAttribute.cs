namespace Velura.Models.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigItemAttribute<T>(
	string name,
	string description,
	T defaultValue) : Attribute
{
	public string Name { get; } = name;

	public string Description { get; } = description;
	
	public T DefaultValue { get; } = defaultValue;
}