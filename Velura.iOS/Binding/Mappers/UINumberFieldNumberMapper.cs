using Velura.iOS.Binding.Abstract;
using Velura.iOS.UI;

namespace Velura.iOS.Binding.Mappers;

public sealed class UINumberFieldNumberMapper : BindingMapper
{
	public override Type TargetType => typeof(UINumberField);

	public override string PropertyPath => nameof(UINumberField.Number);
	
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