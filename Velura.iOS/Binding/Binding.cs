using System.ComponentModel;
using System.Reflection;
using Velura.iOS.Binding.Targets.Abstract;

namespace Velura.iOS.Binding;

public class Binding<TViewModel>(
	UIView target,
	TViewModel source,
	string sourcePropertyPath,
	BindingMode mode,
	UpdateSourceTrigger updateSourceTrigger,
	BindingMapper mapper) : IDisposable where TViewModel : INotifyPropertyChanged
{
	public UIView Target { get; private set; } = target;
	public TViewModel Source { get; private set; } = source;
	public string SourcePropertyPath { get; } = sourcePropertyPath;
	public BindingMode Mode { get; } = mode;
	public UpdateSourceTrigger UpdateSourceTrigger { get; } = updateSourceTrigger;

	readonly BindingMapper mapper = mapper;
	
	
	readonly PropertyInfo? sourcePropertyInfo = typeof(TViewModel).GetProperty(sourcePropertyPath);

	public void UpdateSource()
	{
		object? newValue = mapper.GetValue(Target);
		sourcePropertyInfo!.SetValue(Source, newValue);
	}

	public void UpdateTarget()
	{
		object? newValue = sourcePropertyInfo!.GetValue(Source);
		mapper.SetValue(Target, newValue);
	}

	
	bool isDisposed = false;
	
	protected virtual void Dispose(
		bool disposing)
	{
		if (isDisposed)
			return;
		
		if (disposing)
		{
			// Dispose managed state
			mapper.Unsubscribe(Target);
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