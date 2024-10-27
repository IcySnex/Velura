using Velura.iOS.Binding.Abstract;

namespace Velura.iOS.Binding.Converters;

public sealed class EnumStringBindingConverter : IBindingConverter
{
	public object? Convert(
		object? value,
		Type targetType,
		object? parameter = null) =>
		value is Enum enumValue ? enumValue.ToString() : null;

	public object? ConvertBack(
		object? value,
		Type targetType,
		object? parameter = null) =>
		value is string enumName ? Enum.Parse(targetType, enumName, true) : null;
}