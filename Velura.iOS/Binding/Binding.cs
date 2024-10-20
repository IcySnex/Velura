using System.ComponentModel;
using System.Reflection;

namespace Velura.iOS.Binding;

public class Binding<TViewModel>(
	TViewModel source,
	UIView target,
	string sourcePropertyPath,
	string targetPropertyPath,
	BindingMode mode,
	UpdateSourceTrigger updateSourceTrigger) where TViewModel : INotifyPropertyChanged
{
	readonly PropertyInfo sourceProperty = source.GetType().GetProperty(sourcePropertyPath) ?? throw new InvalidOperationException($"Invalid source property path: {sourcePropertyPath}");
	readonly PropertyInfo targetProperty = target.GetType().GetProperty(targetPropertyPath) ?? throw new InvalidOperationException($"Invalid target property path: {targetPropertyPath}");

	public TViewModel Source { get; } = source;
	public UIView Target { get; } = target;
	public string SourcePropertyName => sourceProperty.Name;
	public string TargetPropertyName => targetProperty.Name;
	public BindingMode Mode { get; } = mode;
	public UpdateSourceTrigger UpdateSourceTrigger { get; } = updateSourceTrigger;


	public void UpdateSource()
	{
		object? newValue = targetProperty.GetValue(Target);
		sourceProperty.SetValue(Source, newValue);
	}

	public void UpdateTarget()
	{
		object? newValue = sourceProperty.GetValue(Source);
		targetProperty.SetValue(Target, newValue);
	}
}