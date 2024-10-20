using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using ObjCRuntime;
using Velura.iOS.Delegates;
using Velura.Models;

namespace Velura.iOS.Views.Elements;

public sealed class SettingsPropertyViewCell : UITableViewCell
{
	readonly IUITextFieldDelegate numbersOnlyDelegate;
	
	readonly UIView backgroundView;
	readonly UILabel nameLabel;
	readonly UILabel descriptionLabel;
	UIView? settingView;
	
	public SettingsPropertyViewCell(
		NativeHandle handle) : base(handle)
	{
		numbersOnlyDelegate = new NumbersOnlyTextFieldDelegate();
		
		// Properties
		SelectionStyle = UITableViewCellSelectionStyle.None;
		BackgroundColor = UIColor.Clear;
		
		// UI
		backgroundView = new()
		{
			BackgroundColor = UIColor.SecondarySystemGroupedBackground,
			Layer =
			{
				CornerRadius = 8,
				MasksToBounds = true
			}
		};
		ContentView.AddSubview(backgroundView);
		
		nameLabel = new()
		{
			Font = UIFont.PreferredBody
		};
		ContentView.AddSubview(nameLabel);

		descriptionLabel = new()
		{
			Font = UIFont.PreferredFootnote,
			TextColor = UIColor.SecondaryLabel,
			Lines = 0,
			LineBreakMode = UILineBreakMode.WordWrap
		};
		ContentView.AddSubview(descriptionLabel);
		
		ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		ContentView.AddConstraints(
			backgroundView.AtTopOf(ContentView),
			backgroundView.AtLeftOf(ContentView),
			backgroundView.AtRightOf(ContentView),
			backgroundView.Height().EqualTo().HeightOf(nameLabel).Plus(22),

			nameLabel.AtLeftOf(ContentView, 15),
			nameLabel.WithSameCenterY(backgroundView),
			
			descriptionLabel.AtLeftOf(ContentView, 15),
			descriptionLabel.AtRightOf(ContentView, 15),
			descriptionLabel.Below(backgroundView, 8),
			descriptionLabel.AtBottomOf(ContentView, 20)
		);
	}

	
	public void UpdateCell(
<<<<<<< HEAD
		SettingsProperty property)
		// IMvxBindingContextOwner bindingContextOwner)
=======
		SettingsProperty property,
		IMvxBindingContextOwner bindingContextOwner)
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
	{
		nameLabel.Text = property.Details.Name;
		descriptionLabel.Text = property.Details.Description;

		if (settingView is not null)
		{
<<<<<<< HEAD
			// bindingContextOwner.ClearBindings(property.Path);
=======
			bindingContextOwner.ClearBindings(property.Path);
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
			settingView?.RemoveFromSuperview();
		}
		
		if (property.Type.IsEnum)
		{
			Enum[] enumValues = Enum.GetValues(property.Type).Cast<Enum>().ToArray();
			string selectedValue = enumValues[0].ToString();

			UIButtonConfiguration buttonConfiguration = UIButtonConfiguration.PlainButtonConfiguration;
			buttonConfiguration.Title = selectedValue;
			buttonConfiguration.Indicator = UIButtonConfigurationIndicator.Popup;
			
			settingView = new UIButton
			{
				Configuration = buttonConfiguration,
				ChangesSelectionAsPrimaryAction = true,
				ShowsMenuAsPrimaryAction = true,
				Menu = UIMenu.Create("", enumValues.Select(enumValue => UIAction.Create(enumValue.ToString(), null, null, _ =>
				{
					buttonConfiguration.Title = enumValue.ToString();
				})).ToArray<UIMenuElement>())
			};
			settingView.SetContentHuggingPriority((float)UILayoutPriority.DefaultHigh, UILayoutConstraintAxis.Horizontal);
		}
		else if (property.Type == typeof(bool))
		{
			settingView = new UISwitch();

<<<<<<< HEAD
			// bindingContextOwner.AddBinding(settingView, new MvxBindingDescription(
			// 	"On",
			// 	$"Config.{property.Path}",
			// 	null, null, null,
			// 	MvxBindingMode.TwoWay
			// ), property.Path);
=======
			bindingContextOwner.AddBinding(settingView, new MvxBindingDescription(
				"On",
				$"Config.{property.Path}",
				null, null, null,
				MvxBindingMode.TwoWay
			), property.Path);
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
		}
		else if (property.Type == typeof(string))
		{
			settingView = new UITextField
			{
				Placeholder = "Change Value...",
				TextAlignment = UITextAlignment.Right,
				AutocorrectionType = UITextAutocorrectionType.No,
				AutocapitalizationType = UITextAutocapitalizationType.None,
				SpellCheckingType = UITextSpellCheckingType.No
			};
			
<<<<<<< HEAD
			// bindingContextOwner.AddBinding(settingView, new MvxBindingDescription(
			// 	"Text",
			// 	$"Config.{property.Path}",
			// 	null, null, null,
			// 	MvxBindingMode.TwoWay
			// ), property.Path);
=======
			bindingContextOwner.AddBinding(settingView, new MvxBindingDescription(
				"Text",
				$"Config.{property.Path}",
				null, null, null,
				MvxBindingMode.TwoWay
			), property.Path);
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
		}
		else if (property.Type == typeof(int))
		{
			settingView = new UITextField
			{
				KeyboardType = UIKeyboardType.NumberPad,
				Delegate = numbersOnlyDelegate,
				Placeholder = "Change Value...",
				TextAlignment = UITextAlignment.Right,
				AutocorrectionType = UITextAutocorrectionType.No,
				AutocapitalizationType = UITextAutocapitalizationType.None,
				SpellCheckingType = UITextSpellCheckingType.No
			};
			
<<<<<<< HEAD
			// bindingContextOwner.AddBinding(settingView, new MvxBindingDescription(
			// 	"Text",
			// 	$"Config.{property.Path}",
			// 	null, null, null,
			// 	MvxBindingMode.TwoWay
			// ), property.Path);
=======
			bindingContextOwner.AddBinding(settingView, new MvxBindingDescription(
				"Text",
				$"Config.{property.Path}",
				null, null, null,
				MvxBindingMode.TwoWay
			), property.Path);
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
		}
		else
		{
			throw new NotSupportedException($"Unexpected setting type was passed. '{property.Type}' is not supported.");
		}
		ContentView.AddSubview(settingView);

		settingView.TranslatesAutoresizingMaskIntoConstraints = false;
		ContentView.AddConstraints(
			settingView.ToRightOf(nameLabel, 15),
            settingView.AtRightOf(ContentView, 15),
			settingView.WithSameCenterY(backgroundView)
		);
	}
}