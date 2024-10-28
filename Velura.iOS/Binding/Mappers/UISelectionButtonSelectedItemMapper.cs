using Velura.iOS.Binding.Abstract;
using Velura.iOS.UI;

namespace Velura.iOS.Binding.Mappers;

public sealed class UISelectionButtonSelectedItemMapper : BindingMapper
{
	public override Type TargetType => typeof(UISelectionButton);

	public override string PropertyPath => nameof(UISelectionButton.SelectedItem);

	public override BindingMode SupportedMode =>  BindingMode.OneWay | BindingMode.OneWayToSource | BindingMode.TwoWay | BindingMode.OneTime;


	public override void Subscribe(
		UIView target,
		UpdateSourceTrigger updateSourceTrigger) =>
		((UISelectionButton)target).SelectedItemChanged += OnValueChanged;

	public override void Unsubscribe(
		UIView target) =>
		((UISelectionButton)target).SelectedItemChanged -= OnValueChanged;
}