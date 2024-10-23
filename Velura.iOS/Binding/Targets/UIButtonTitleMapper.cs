using Velura.iOS.Binding.Targets.Abstract;

namespace Velura.iOS.Binding.Targets;

public class UIButtonTitleMapper : BindingMapper
{
	public new static Type TargetType => typeof(UIButton);

	public new static string PropertyPath => "Title";

	public new static BindingMode SupportedMode => BindingMode.OneWay | BindingMode.OneTime;


	public override object GetValue(
		UIView target) =>
		((UIButton)target).Title(UIControlState.Normal);

	public override void SetValue(
		UIView target,
		object? value) =>
		((UIButton)target).SetTitle((string?)value, UIControlState.Normal);
}