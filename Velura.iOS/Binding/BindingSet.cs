using System.ComponentModel;
using Velura.iOS.Binding.Targets.Abstract;

namespace Velura.iOS.Binding;

public class BindingSet<TViewModel> where TViewModel : INotifyPropertyChanged
{
	readonly TViewModel viewModel;
	readonly List<Binding<TViewModel>> bindings = [];

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
		IEnumerable<Binding<TViewModel>> affectedBindings = bindings.Where(b => b.SourcePropertyName == e.PropertyName && b.Mode is BindingMode.OneWay or BindingMode.TwoWay);
		foreach (Binding<TViewModel> binding in affectedBindings)
			if (binding.UpdateSourceTrigger != UpdateSourceTrigger.Explicit)
				binding.UpdateTarget();
	}

	
	public Binding<TViewModel> Bind(
		UIView target,
		string sourcePropertyPath,
		string targetPropertyPath,
		BindingMode mode = BindingMode.OneWay,
		UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged)
	{
		Binding<TViewModel> binding = new(viewModel, target, sourcePropertyPath, targetPropertyPath, mode, updateSourceTrigger);
		bindings.Add(binding);

		if (updateSourceTrigger is UpdateSourceTrigger.Explicit)
			return binding;

		if (mode is BindingMode.TwoWay or BindingMode.OneWayToSource && IBindingTarget.Implementations.TryGetValue(target.GetType(), out IBindingTarget? bindingTarget))
			bindingTarget.Subscribe(binding);
		
		switch (mode)
		{
			case BindingMode.OneWay or BindingMode.TwoWay or BindingMode.OneTime:
				binding.UpdateTarget();
				break;
			case BindingMode.OneWayToSource:
				binding.UpdateSource();
				break;
		}

		return binding;
	}

	public void Unbind(
		Binding<TViewModel> binding)
	{
		bindings.Remove(binding);
		
		if (binding.Mode is BindingMode.TwoWay or BindingMode.OneWayToSource && IBindingTarget.Implementations.TryGetValue(binding.Target.GetType(), out IBindingTarget? bindingTarget))
			bindingTarget.Unsubscribe(binding);
	}


	public void Clear()
	{
		foreach (Binding<TViewModel> binding in bindings)
			Unbind(binding);
	}
}