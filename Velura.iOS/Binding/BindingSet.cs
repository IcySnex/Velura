using System.ComponentModel;
using System.Windows.Input;
using Velura.iOS.Binding.Abstract;
using Velura.iOS.Helpers;

namespace Velura.iOS.Binding;

public sealed class BindingSet<TViewModel> : IDisposable where TViewModel : INotifyPropertyChanged
{
	TViewModel viewModel;
	List<CoreBinding> bindings = [];

	public BindingSet(
		TViewModel viewModel)
	{
		this.viewModel = viewModel;
		viewModel.PropertyChanged += OnViewModelPropertyChanged;
	}
	

	void OnViewModelPropertyChanged(
		object? sender,
		PropertyChangedEventArgs e)
	{
		IEnumerable<PropertyBinding<TViewModel>> affectedBindings = bindings.OfType<PropertyBinding<TViewModel>>().Where(binding =>
			binding.SourcePropertyPath == e.PropertyName &&
			(binding.Mode is BindingMode.OneWay || binding.Mode is BindingMode.TwoWay) &&
			binding.UpdateSourceTrigger != UpdateSourceTrigger.Explicit);

		foreach (PropertyBinding<TViewModel> binding in affectedBindings)
			binding.UpdateTarget();
	}

	
	public PropertyBinding<TViewModel> Bind(
		UIView target,
		string targetPropertyPath,
		string sourcePropertyPath,
		BindingMode mode = BindingMode.OneWay,
		UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
		IPropertyBindingConverter? converter = null)
	{
		PropertyBindingMapper? mapper = IOSApp.PropertyBindingMappers.FirstOrDefault(bm => bm.TargetType == target.GetType() && bm.PropertyPath == targetPropertyPath && bm.SupportedMode.HasFlag(mode));
		if (mapper is null && mode is not (BindingMode.OneWay or BindingMode.OneTime))
			throw new InvalidOperationException($"Could not find BindingMapper for target type '{target.GetType().Name}' with property path '{targetPropertyPath}'. Default bindings only support BindingMode.Oneway or BindingMode.OneTime.");
		
		PropertyBinding<TViewModel> binding = new(target, viewModel, targetPropertyPath, sourcePropertyPath, mode, updateSourceTrigger, converter, mapper);
		bindings.Add(binding);

		if (updateSourceTrigger is not UpdateSourceTrigger.Explicit)
		{
			if (mode is BindingMode.TwoWay or BindingMode.OneWayToSource)
				mapper!.Subscribe(target, updateSourceTrigger);

			switch (mode)
			{
				case BindingMode.OneWay or BindingMode.TwoWay or BindingMode.OneTime:
					binding.UpdateTarget();
					break;
				case BindingMode.OneWayToSource:
					binding.UpdateSource();
					break;
			}
		}
		return binding;
	}

	public EventBinding Bind(
		UIView target,
		string targetEventPath,
		ICommand action)
	{
		EventBinding binding = new(target, targetEventPath, action);
		bindings.Add(binding);

		return binding;
	}
	

	public void Unbind(
		CoreBinding binding)
	{
		binding.Dispose();
		bindings.Remove(binding);
	}

	public void Clear()
	{
		foreach (CoreBinding binding in bindings)
			binding.Dispose();
		bindings.Clear();
	}


	public BindingSet<TSubViewModel> CreateSubSet<TSubViewModel>(
		string propertyPath) where TSubViewModel : INotifyPropertyChanged =>
		new(viewModel.GetPropertyValue<TSubViewModel>(propertyPath));
	
	
	bool isDisposed = false;

	void Dispose(
		bool disposing)
	{
		if (isDisposed)
			return;
		
		if (disposing)
		{
			// Dispose managed state
			Clear();
			viewModel.PropertyChanged -= OnViewModelPropertyChanged;
		}
		
		// Free unmanaged resources/Set large fields to null
		viewModel = default!;
		bindings =  default!;
		
		isDisposed = true;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	~BindingSet() =>
		Dispose(false);
}