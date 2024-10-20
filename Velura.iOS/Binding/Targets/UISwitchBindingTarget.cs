using System.ComponentModel;
using Velura.iOS.Binding.Targets.Abstract;

namespace Velura.iOS.Binding.Targets;

public class UISwitchBindingTarget : IBindingTarget
{
	EventHandler? onValueChanged = null;

	public void Subscribe<TViewModel>(
		Binding<TViewModel> binding) where TViewModel : INotifyPropertyChanged
	{
		if (binding.TargetPropertyName != nameof(UISwitch.On))
			return;

		onValueChanged = (_, _) => binding.UpdateSource();
		((UISwitch)binding.Target).ValueChanged += onValueChanged;
	}

	public void Unsubscribe<TViewModel>(
		Binding<TViewModel> binding) where TViewModel : INotifyPropertyChanged =>
		((UISwitch)binding.Target).ValueChanged -= onValueChanged;

}