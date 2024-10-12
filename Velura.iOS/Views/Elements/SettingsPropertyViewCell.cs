using Cirrious.FluentLayouts.Touch;
using ObjCRuntime;
using Velura.iOS.Delegates;

namespace Velura.iOS.Views.Elements;

public sealed class SettingsPropertyViewCell : UITableViewCell
{
	readonly IUITextFieldDelegate numbersOnlyDelegate = new NumbersOnlyTextFieldDelegate();
	
	readonly UILabel nameLabel;
	UIView? settingView;
	
	public SettingsPropertyViewCell(
		NativeHandle handle) : base(handle)
	{
		// Properties
		SelectionStyle = UITableViewCellSelectionStyle.None;
		
		// UI
		nameLabel = new();
		ContentView.AddSubview(nameLabel);
		
		ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		ContentView.AddConstraints(
			nameLabel.AtLeftOf(ContentView, 15),
			nameLabel.WithSameCenterY(ContentView)
		);
	}

	
	public void UpdateCell(
		string name,
		Type type)
	{
		nameLabel.Text = name;

		settingView?.RemoveFromSuperview();
		if (type == typeof(bool))
			settingView = new UISwitch();
		else if (type == typeof(string))
			settingView = new UITextField
			{
				Placeholder = "Change Value...",
				TextAlignment = UITextAlignment.Right,
			};
		else if (type == typeof(int))
			settingView = new UITextField
			{
				Placeholder = "Change Value...",
				TextAlignment = UITextAlignment.Right,
				KeyboardType = UIKeyboardType.NumberPad,
				Delegate = numbersOnlyDelegate,
				AutocorrectionType = UITextAutocorrectionType.No,
				AutocapitalizationType = UITextAutocapitalizationType.None,
				SpellCheckingType = UITextSpellCheckingType.No
			};
		else
			throw new NotSupportedException($"Unexpected setting type was passed. '{type}' is not supported.");
		settingView.TranslatesAutoresizingMaskIntoConstraints = false;
		
		ContentView.AddSubview(settingView);
		ContentView.AddConstraints(
			settingView.AtRightOf(ContentView, 15),
			settingView.WithSameCenterY(ContentView),
			settingView.ToRightOf(nameLabel, 15)
		);
	}
}