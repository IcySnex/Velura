using System.ComponentModel;
using Velura.iOS.Binding.Targets.Abstract;

namespace Velura.iOS.Binding.Targets;

public class UITextFieldBindingTarget : IBindingTarget
{
	EventHandler? onValueChanged = null;

	public void Subscribe<TViewModel>(
		Binding<TViewModel> binding) where TViewModel : INotifyPropertyChanged
	{
		if (binding.TargetPropertyName != nameof(UITextField.Text))
			return;

		onValueChanged = (_, _) => binding.UpdateSource();
		switch (binding.UpdateSourceTrigger)
		{
			case UpdateSourceTrigger.PropertyChanged:
				((UITextField)binding.Target).EditingChanged += onValueChanged;
				break;
			case UpdateSourceTrigger.LostFocus:
				((UITextField)binding.Target).EditingDidEnd += onValueChanged;
				break;
		}
	}

	public void Unsubscribe<TViewModel>(
		Binding<TViewModel> binding) where TViewModel : INotifyPropertyChanged
	{
		switch (binding.UpdateSourceTrigger)
		{
			case UpdateSourceTrigger.PropertyChanged:
				((UITextField)binding.Target).EditingChanged -= onValueChanged;
				break;
			case UpdateSourceTrigger.LostFocus:
				((UITextField)binding.Target).EditingDidEnd -= onValueChanged;
				break;
		}
	}
}