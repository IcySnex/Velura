using Velura.iOS.Binding.Targets.Abstract;

namespace Velura.iOS.Binding.Targets;

public class UITextFieldTextMapper : BindingMapper
{
	public new static Type TargetType => typeof(UITextField);

	public new static string PropertyPath => nameof(UITextField.Text);
	
	public new static BindingMode SupportedMode => BindingMode.OneWay | BindingMode.OneWayToSource | BindingMode.TwoWay | BindingMode.OneTime;

	
	public override void Subscribe(
		UIView target,
		UpdateSourceTrigger updateSourceTrigger)
	{
		switch (updateSourceTrigger)
		{
			case UpdateSourceTrigger.PropertyChanged:
				((UITextField)target).EditingChanged += RaiseValueChanged;
				break;
			case UpdateSourceTrigger.LostFocus:
				((UITextField)target).EditingDidEnd += RaiseValueChanged;
				break;
		}
	}

	public override void Unsubscribe(
		UIView target)
	{
		((UITextField)target).EditingChanged -= RaiseValueChanged;
		((UITextField)target).EditingDidEnd -= RaiseValueChanged;
	}
}