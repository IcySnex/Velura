using Velura.iOS.Binding.Abstract;

namespace Velura.iOS.Binding.Converters;

public sealed class ChangeTypeBindingConverter : IPropertyBindingConverter
{
	public object? Convert(
		object? value,
		Type targetType,
		object? parameter = null) =>
		targetType is null ? value : System.Convert.ChangeType(value, targetType);

	public object? ConvertBack(
		object? value,
		Type targetType,
		object? parameter = null) =>
		targetType is null ? value : System.Convert.ChangeType(value, targetType);
}