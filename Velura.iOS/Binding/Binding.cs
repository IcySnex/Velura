using System.ComponentModel;
using System.Reflection;
using Velura.iOS.Binding.Abstract;
using Velura.iOS.Binding.Converters;
using Velura.iOS.Helpers;

namespace Velura.iOS.Binding;

public sealed class Binding<TViewModel> : IDisposable where TViewModel : INotifyPropertyChanged
{
	static readonly IBindingConverter DefaultConverter = new DefaultBindingConverter();
	
	
	readonly IBindingConverter? converter;
	readonly BindingMapper? mapper;

	readonly PropertyInfo? targetPropertyInfo;
	readonly PropertyInfo sourcePropertyInfo;

	public Binding(
		UIView target,
		TViewModel source,
		string targetPropertyPath,
		string sourcePropertyPath,
		BindingMode mode,
		UpdateSourceTrigger updateSourceTrigger,
		IBindingConverter? converter = null,
		BindingMapper? mapper = null)
	{
		Target = target;
		Source = source;
		TargetPropertyPath = targetPropertyPath;
		SourcePropertyPath = sourcePropertyPath;
		Mode = mode;
		UpdateSourceTrigger = updateSourceTrigger;
		 
		this.converter = converter;
		this.mapper = mapper;
		
		if (mapper is not null)
			mapper.ValueChanged += OnBindingTargetValueChanged;
		else
			targetPropertyInfo = target.GetProperty(targetPropertyPath);
		sourcePropertyInfo = source.GetProperty(sourcePropertyPath);
	}
	
	public UIView Target { get; private set; }
	public TViewModel Source { get; private set; }
	public string TargetPropertyPath { get; }
	public string SourcePropertyPath { get; }
	public BindingMode Mode { get; }
	public UpdateSourceTrigger UpdateSourceTrigger { get; }

	
	public void UpdateSource()
	{
		object? newValue = mapper?.GetValue(Target) ?? targetPropertyInfo!.GetValue(Target);
		newValue = converter is null ? DefaultConverter.ConvertBack(newValue, sourcePropertyInfo.PropertyType) : converter.ConvertBack(newValue, sourcePropertyInfo.PropertyType);
		
		sourcePropertyInfo.SetValue(Source, newValue);
	}

	public void UpdateTarget()
	{
		object? newValue = sourcePropertyInfo.GetValue(Source);
		newValue = converter is null ? DefaultConverter.Convert(newValue, targetPropertyInfo?.PropertyType!) : converter.Convert(newValue, targetPropertyInfo?.PropertyType!);
		
		if (mapper is not null)
			mapper.SetValue(Target, newValue);
		else
			targetPropertyInfo!.SetValue(Target, newValue);
	}


	void OnBindingTargetValueChanged(
		object? sender,
		EventArgs args)
	{
		if (Target == sender)
			UpdateSource();
	}
	
	
	bool isDisposed = false;

	void Dispose(
		bool disposing)
	{
		if (isDisposed)
			return;
		
		if (disposing)
		{
			// Dispose managed state
			if (mapper is not null)
			{
				mapper.Unsubscribe(Target);
				mapper.ValueChanged -= OnBindingTargetValueChanged;
			}
		}
		
		// Free unmanaged resources/Set large fields to null
		Target = default!;
		Source = default!;
		
		isDisposed = true;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	~Binding() =>
		Dispose(false);
}