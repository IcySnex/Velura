using Velura.iOS.Binding.Abstract;

namespace Velura.iOS.Binding.Mappers;

public sealed class UITextFieldTextMapper : PropertyBindingMapper
{
	public override Type TargetType => typeof(UITextField);

	public override string PropertyPath => nameof(UITextField.Text);
	
	public override BindingMode SupportedMode => BindingMode.OneWay | BindingMode.OneWayToSource | BindingMode.TwoWay | BindingMode.OneTime;

	
	public override void Subscribe(
		UIView target,
		UpdateSourceTrigger updateSourceTrigger)
	{
		switch (updateSourceTrigger)
		{
			case UpdateSourceTrigger.PropertyChanged:
				((UITextField)target).EditingChanged += OnValueChanged;
				break;
			case UpdateSourceTrigger.LostFocus:
				((UITextField)target).EditingDidEnd += OnValueChanged;
				break;
		}
	}

	public override void Unsubscribe(
		UIView target)
	{
		((UITextField)target).EditingChanged -= OnValueChanged;
		((UITextField)target).EditingDidEnd -= OnValueChanged;
	}
}