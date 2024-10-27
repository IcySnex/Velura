namespace Velura.iOS.Views.UI;


public class UINumberField : UITextField, IUITextFieldDelegate
{
	public new bool ShouldChangeCharacters(
		UITextField textField,
		NSRange range,
		string replacementString) =>
		replacementString.All(char.IsDigit);

	
	public UINumberField()
	{
		KeyboardType = UIKeyboardType.NumberPad;
		AutocorrectionType = UITextAutocorrectionType.No;
		AutocapitalizationType = UITextAutocapitalizationType.None;
		SpellCheckingType = UITextSpellCheckingType.No;
		Delegate = this;
	}

	
	[Obsolete("This property is only valid for 'UITextField'. To change the value of a 'UINumberField', use the 'Number' property.", true)]
	public new string? Text
	{
		get => throw new InvalidOperationException();
		set => throw new InvalidOperationException();
	}

	public int Number
	{
		get => int.TryParse(base.Text, out int number) ? number : 0;
		set => base.Text = value.ToString();
	}
}