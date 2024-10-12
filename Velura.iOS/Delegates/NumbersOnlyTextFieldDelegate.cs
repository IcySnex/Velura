namespace Velura.iOS.Delegates;

public class NumbersOnlyTextFieldDelegate : UITextFieldDelegate
{
	[Export("textField:shouldChangeCharactersInRange:replacementString:")]
	public override bool ShouldChangeCharacters(
		UITextField textField,
		NSRange range,
		string replacementString) =>
		replacementString.All(char.IsDigit);
}