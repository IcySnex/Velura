using System.Reflection;
using Velura.iOS.Helpers;

namespace Velura.iOS.Binding.Abstract;

public abstract class PropertyBindingMapper
{
	public abstract Type TargetType { get; }

	public abstract string PropertyPath { get; }
	
	public abstract BindingMode SupportedMode { get; }

	
	PropertyInfo? propertyInfo = null;
	
	protected PropertyInfo PropertyInfo => propertyInfo ??= Extensions.GetProperty(TargetType, PropertyPath);

	
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


	public virtual void Subscribe(
		UIView target,
		UpdateSourceTrigger updateSourceTrigger)
	{ }

	public virtual void Unsubscribe(
		UIView target)
	{ }
}