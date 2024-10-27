using System.ComponentModel;
using Velura.iOS.Binding.Abstract;
using Velura.iOS.Helpers;

namespace Velura.iOS.Binding;

public sealed class BindingSet<TViewModel> : IDisposable where TViewModel : INotifyPropertyChanged
{
	TViewModel viewModel;
	List<Binding<TViewModel>> bindings = [];

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
		IEnumerable<Binding<TViewModel>> affectedBindings = bindings.Where(b => 
			b.SourcePropertyPath == e.PropertyName &&
			(b.Mode is BindingMode.OneWay || b.Mode is BindingMode.TwoWay) &&
			b.UpdateSourceTrigger != UpdateSourceTrigger.Explicit);

		foreach (Binding<TViewModel> binding in affectedBindings)
			binding.UpdateTarget();
	}

	
	public Binding<TViewModel> Bind(
		UIView target,
		string targetPropertyPath,
		string sourcePropertyPath,
		BindingMode mode = BindingMode.OneWay,
		UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
		IBindingConverter? converter = null)
	{
		BindingMapper? mapper = IOSApp.BindingMappers.FirstOrDefault(bm => bm.TargetType == target.GetType() && bm.PropertyPath == targetPropertyPath && bm.SupportedMode.HasFlag(mode));
		if (mapper is null && mode is not (BindingMode.OneWay or BindingMode.OneTime))
			throw new InvalidOperationException($"Could not find BindingMapper for target type '{target.GetType().Name}' with property path '{targetPropertyPath}'. Default bindings only support BindingMode.Oneway or BindingMode.OneTime.");
		
		Binding<TViewModel> binding = new(target, viewModel, targetPropertyPath, sourcePropertyPath, mode, updateSourceTrigger, converter, mapper);
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

	public void Unbind(
		Binding<TViewModel> binding)
	{
		binding.Dispose();
		bindings.Remove(binding);
	}

	public void Clear()
	{
		foreach (Binding<TViewModel> binding in bindings)
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