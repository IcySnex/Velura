namespace Velura.iOS.Binding.Abstract;

public interface IBindingConverter
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