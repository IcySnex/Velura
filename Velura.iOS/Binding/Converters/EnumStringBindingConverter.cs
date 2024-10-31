using System.Collections.Concurrent;
using System.Reflection;
using Velura.iOS.Binding.Abstract;
using Velura.Models.Attributes;

namespace Velura.iOS.Binding.Converters;

public sealed class EnumL10NNameBindingConverter : IPropertyBindingConverter
{
	static readonly ConcurrentDictionary<Type, Dictionary<string, string>> LocalizedNameCache = new();
	
	static Dictionary<string, string> GetNameMappings(
		Type enumType) =>
		LocalizedNameCache.GetOrAdd(enumType, type =>
			type.GetFields(BindingFlags.Public | BindingFlags.Static).ToDictionary(field => field.Name, field => 
				field.GetCustomAttribute<L10NNameAttribute>()?.Name ?? field.Name));

	
	public object? Convert(
		object? value,
		Type targetType,
		object? parameter = null)
	{
		if (value is not Enum enumValue)
			return null;

		Dictionary<string, string> nameMappings = GetNameMappings(enumValue.GetType());
		return nameMappings.TryGetValue(enumValue.ToString(), out string? localizedName) ? localizedName : enumValue.ToString();
	}

	public object? ConvertBack(
		object? value,
		Type targetType,
		object? parameter = null)
	{
		if (value is not string stringValue)
			return null;

		Dictionary<string, string> nameMappings = GetNameMappings(targetType);

		string enumName = nameMappings.FirstOrDefault(kv => kv.Value == stringValue).Key ?? stringValue;
		return Enum.TryParse(targetType, enumName, out object? result) ? result : null;
	}
}