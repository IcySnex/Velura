using System.Reflection;

namespace Velura.iOS.Binding.Targets.Abstract;

public class BindingMapper
{
	public static BindingMapper DefaultImplementation { get; } = new();
	
	public static Dictionary<(Type TargetType, string PropertyPath, BindingMode SupportedMode), BindingMapper> Implementations { get; } = new()
	{
		{ (UISwitchOnMapper.TargetType, UISwitchOnMapper.PropertyPath, UISwitchOnMapper.SupportedMode), new UISwitchOnMapper() },
		{ (UITextFieldTextMapper.TargetType, UITextFieldTextMapper.PropertyPath, UITextFieldTextMapper.SupportedMode), new UITextFieldTextMapper() },
		{ (UIButtonTitleMapper.TargetType, UIButtonTitleMapper.PropertyPath, UIButtonTitleMapper.SupportedMode), new UIButtonTitleMapper() }
	};

	public static BindingMapper Get(
		Type targetType,
		string propertyPath,
		BindingMode mode) => 
		Implementations.GetValueOrDefault((targetType, propertyPath, mode), DefaultImplementation);


	public static Type TargetType => typeof(UIView);

	public static string PropertyPath => string.Empty;
	
	public static BindingMode SupportedMode => BindingMode.OneWay | BindingMode.OneTime;

	
	public event EventHandler? ValueChanged;
	
	protected void RaiseValueChanged(
		object? sender,
		EventArgs args) =>
		ValueChanged?.Invoke(sender, args);

	
	readonly PropertyInfo? propertyInfo = TargetType.GetProperty(PropertyPath);

	public virtual object? GetValue(
		UIView target) =>
		propertyInfo!.GetValue(target);

	public virtual void SetValue(
		UIView target,
		object? value) =>
		propertyInfo!.SetValue(target, value);


	public virtual extern void Subscribe(
		UIView target,
		UpdateSourceTrigger updateSourceTrigger);

	public virtual extern void Unsubscribe(
		UIView target);
}