using Velura.iOS.Binding.Abstract;

namespace Velura.iOS.Binding.Mappers;

public sealed class UIButtonTitleMapper : BindingMapper
{
	public override Type TargetType => typeof(UIButton);

	public override string PropertyPath => "Title";

	public override BindingMode SupportedMode => BindingMode.OneWay | BindingMode.OneTime;


	public override object GetValue(
		UIView target) =>
		((UIButton)target).Title(UIControlState.Normal);

	public override void SetValue(
		UIView target,
		object? value) =>
		((UIButton)target).SetTitle((string?)value, UIControlState.Normal);
}