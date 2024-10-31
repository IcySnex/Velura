using Velura.iOS.Binding.Abstract;

namespace Velura.iOS.Binding.Mappers;

public sealed class UISwitchOnMapper : PropertyBindingMapper
{
	public override Type TargetType => typeof(UISwitch);

	public override string PropertyPath => nameof(UISwitch.On);

	public override BindingMode SupportedMode => BindingMode.OneWay | BindingMode.OneWayToSource | BindingMode.TwoWay | BindingMode.OneTime;

	
	public override void Subscribe(
		UIView target,
		UpdateSourceTrigger updateSourceTrigger) =>
		((UISwitch)target).ValueChanged += OnValueChanged;

	public override void Unsubscribe(
		UIView target) =>
		((UISwitch)target).ValueChanged -= OnValueChanged;
}