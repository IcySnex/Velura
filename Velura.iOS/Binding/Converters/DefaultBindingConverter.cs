using Velura.iOS.Binding.Abstract;

namespace Velura.iOS.Binding.Converters;

public sealed class DefaultBindingConverter : IBindingConverter
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