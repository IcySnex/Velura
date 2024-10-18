using System.Reflection;
using MvvmCross.Binding.Bindings.Target;

namespace Velura.iOS.Bindings;

public class UITextFieldTextBinding(
	object target,
	PropertyInfo targetPropertyInfo) : MvxPropertyInfoTargetBinding<UITextField>(target, targetPropertyInfo)
{
	bool isSubscribed;

	
	protected override void SetValueImpl(
		object target,
		object value)
	{
		if (target is not UITextField textField)
			return;

		textField.Text = (string)value;
	}

	
	public override void SubscribeToEvents()
	{
		isSubscribed = true;
		View.EditingDidEnd += TextFieldEditingDidEnd;
	}

	void TextFieldEditingDidEnd(
		object? sender,
		EventArgs e) =>
		FireValueChanged(View.Text);
	
	
	protected override void Dispose(
		bool isDisposing)
	{
		base.Dispose(isDisposing);

		if (!isDisposing || !isSubscribed)
			return;

		View.EditingDidEnd -= TextFieldEditingDidEnd;
		isSubscribed = false;
	}
}
