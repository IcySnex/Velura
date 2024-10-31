namespace Velura.iOS.Binding.Abstract;

public interface IPropertyBindingConverter
{
	object? Convert(
		object? value,
		Type targetType,
		object? parameter = null);
	
	object? ConvertBack(
		object? value,
		Type targetType,
		object? parameter = null);
}