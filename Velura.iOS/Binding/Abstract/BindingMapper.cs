using System.Reflection;

namespace Velura.iOS.Binding.Abstract;

public abstract class BindingMapper
{
	public abstract Type TargetType { get; }

	public abstract string PropertyPath { get; }
	
	public abstract BindingMode SupportedMode { get; }

	
	PropertyInfo? propertyInfo = null;
	
	protected PropertyInfo PropertyInfo => propertyInfo ??= TargetType.GetProperty(PropertyPath) ?? throw new InvalidOperationException($"Property path '{PropertyPath}' is invalid for type '{TargetType.Name}'.");

	
	public virtual object? GetValue(
		UIView target) =>
		PropertyInfo.GetValue(target);

	public virtual void SetValue(
		UIView target,
		object? value) =>
		PropertyInfo.SetValue(target, value);

	
	public event EventHandler? ValueChanged;
	
	protected void OnValueChanged(
		object? sender,
		EventArgs args) =>
		ValueChanged?.Invoke(sender, args);


	public virtual extern void Subscribe(
		UIView target,
		UpdateSourceTrigger updateSourceTrigger);

	public virtual extern void Unsubscribe(
		UIView target);
}