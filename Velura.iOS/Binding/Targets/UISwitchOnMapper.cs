using Velura.iOS.Binding.Targets.Abstract;

namespace Velura.iOS.Binding.Targets;

public class UISwitchOnMapper : BindingMapper
{
	public new static Type TargetType => typeof(UISwitch);

	public new static string PropertyPath => nameof(UISwitch.On);

	public new static BindingMode SupportedMode => BindingMode.OneWay | BindingMode.OneWayToSource | BindingMode.TwoWay | BindingMode.OneTime;

	
	public override void Subscribe(
		UIView target,
		UpdateSourceTrigger updateSourceTrigger) =>
		((UISwitch)target).ValueChanged += RaiseValueChanged;

	public override void Unsubscribe(
		UIView target) =>
		((UISwitch)target).ValueChanged -= RaiseValueChanged;
}