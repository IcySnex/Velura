using System.Reflection;
using Cirrious.FluentLayouts.Touch;
using ObjCRuntime;
using Velura.Helpers;
using Velura.iOS.Binding;
using Velura.iOS.Binding.Converters;
using Velura.iOS.Helpers;
using Velura.iOS.UI;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Models.Attributes;

namespace Velura.iOS.Views.Elements;

public sealed class SettingsItemViewCell : UITableViewCell
{
	readonly UILabel textLabel;
	readonly UILabel secondaryTextLabel;
	readonly UIImageView imageView;
	readonly UIView controlView;

	readonly FluentLayout[] noImageConstraints;
	readonly FluentLayout[] withImageConstraints;

	BindingSet<ConfigGroup>? controlBindingSet = null;
	PropertyBinding<ConfigGroup>? controlBinding = null;
	readonly EnumL10NNameBindingConverter enumStringConverter = new();
	
	public SettingsItemViewCell(
		NativeHandle handle) : base(handle)
	{
		// UI
		textLabel = new()
		{
			Font = UIFont.PreferredBody,
			TextColor = UIColor.Label,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation
		};
		secondaryTextLabel = new()
		{
			Font = UIFont.PreferredCaption1,
			TextColor = UIColor.SecondaryLabel,
			Lines = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? 2 : 4,
			LineBreakMode = UILineBreakMode.TailTruncation
		};
		imageView = new()
		{
			ClipsToBounds = true,
			Layer =
			{
				CornerRadius = 6,
				MasksToBounds = true
			}
		};
		controlView = new();
		
		ContentView.AddSubviews(textLabel, secondaryTextLabel, imageView, controlView);
		
		// Layout
		noImageConstraints =
		[
			textLabel.AtLeftOf(ContentView, 20),
			secondaryTextLabel.AtLeftOf(ContentView, 20),
		];
		withImageConstraints =
		[
			textLabel.ToRightOf(imageView, 15),
			secondaryTextLabel.ToRightOf(imageView, 15),
		];
		
		ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		ContentView.AddConstraints(
			textLabel.AtTopOf(ContentView, 15),
			textLabel.ToLeftOf(controlView, 20),

			secondaryTextLabel.Below(textLabel, 4),
			secondaryTextLabel.AtBottomOf(ContentView, 15),
			secondaryTextLabel.ToLeftOf(controlView, 20),

			imageView.Width().EqualTo(30),
			imageView.Height().EqualTo(30),
			imageView.WithSameCenterY(ContentView),
			imageView.AtLeftOf(ContentView, 20),

			controlView.Width().LessThanOrEqualTo().WidthOf(ContentView).WithMultiplier(0.5f),
			controlView.WithSameCenterY(ContentView),
			controlView.AtRightOf(ContentView, 20)
		);

		// Initialize
		SetIsClickable(false);
		SetText("placeholder".L10N(), "placeholder".L10N());
		SetImage(null);
		SetControl(null);
	}


	void SetIsClickable(
		bool value)
	{
		SelectionStyle = value ? UITableViewCellSelectionStyle.Default : UITableViewCellSelectionStyle.None;
		Accessory = value ? UITableViewCellAccessory.DisclosureIndicator : UITableViewCellAccessory.None;
	}

	void SetText(
		string text,
		string secondaryText)
	{
		textLabel.Text = text;
		secondaryTextLabel.Text = secondaryText;
	}

	void SetImage(
		UIImage? image,
		UIColor? backgroundColor = null,
		UIColor? tintColor = null)
	{
		if (image is null)
		{
			SeparatorInset = new(0, 20, 0, 0);
			
			imageView.Image = null;
			ContentView.RemoveConstraints(withImageConstraints);
			ContentView.AddConstraints(noImageConstraints);
			return;
		}
		
		SeparatorInset = new(0, 65, 0, 0);

		imageView.Image = image.Apply(new(30, 30), new(22, 22), backgroundColor, tintColor);
		ContentView.RemoveConstraints(noImageConstraints);
		ContentView.AddConstraints(withImageConstraints);
	}

	void SetControl(
		UIView? view)
	{
		if (view is null)
		{
			UIView? existingView = controlView.Subviews.FirstOrDefault();
			
			existingView?.RemoveFromSuperview();
			existingView?.Dispose();
			return;
		}

		view.SetContentHuggingPriority((float)UILayoutPriority.DefaultHigh, UILayoutConstraintAxis.Horizontal);
		controlView.AddSubview(view);
		
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		controlView.AddConstraints(
			view.AtTopOf(controlView),
			view.AtBottomOf(controlView),
			view.AtLeftOf(controlView),
			view.AtRightOf(controlView)
		);
	}


	public void UpdateCell(
		SettingsGroup group)
	{
		SetIsClickable(true);
		SetText(group.Details.Name, group.Details.Description);
		SetImage(UIImage.GetSystemImage(group.Image.ResourceName), group.Image.BackgroundColor.ToUIColor(), group.Image.TintColor.ToUIColor());
	}
	
	public void UpdateCell(
		SettingsProperty property,
		BindingSet<ConfigGroup> bindingSet)
	{
		controlBindingSet = bindingSet;
		
		if (controlBinding is not null)
			controlBindingSet.Unbind(controlBinding);
		
		UIView view;
		if (property.Type.IsEnum)
		{
			view = new UISelectionButton()
			{
				Items = Enum.GetValues(property.Type).Cast<Enum>().Select(e =>
				{
					string enumName = e.ToString();
					return property.Type.GetField(enumName)?.GetCustomAttribute<NameAttribute>()?.Name ?? enumName;
				})
			};
			controlBinding = bindingSet.Bind(view, nameof(UISelectionButton.SelectedItem), property.Path, BindingMode.TwoWay, converter: enumStringConverter);
		}
		else if (property.Type == typeof(bool))
		{
			view = new UISwitch();
			controlBinding = bindingSet.Bind(view, nameof(UISwitch.On), property.Path, BindingMode.TwoWay);
		}
		else if (property.Type == typeof(string))
		{
			view = new UITextField()
			{
				Font = UIFont.PreferredBody,
				Placeholder = "edit".L10N() + "...",
				TextAlignment = UITextAlignment.Right,
				AutocorrectionType = UITextAutocorrectionType.No,
				AutocapitalizationType = UITextAutocapitalizationType.None
			};
			controlBinding = bindingSet.Bind(view, nameof(UITextField.Text), property.Path, BindingMode.TwoWay, UpdateSourceTrigger.LostFocus);
		}
		else if (property.Type == typeof(int))
		{
			view = new UINumberField()
			{
				Font = UIFont.PreferredBody,
				Placeholder =  "edit".L10N() + "...",
				TextAlignment = UITextAlignment.Right
			};
			controlBinding = bindingSet.Bind(view, nameof(UINumberField.Number), property.Path, BindingMode.TwoWay, UpdateSourceTrigger.LostFocus);
		}
		else
			throw new NotSupportedException($"Unexpected setting type was passed. '{property.Type}' is not supported.");
		
		SetIsClickable(false);
		SetText(property.Details.Name, property.Details.Description);
		SetControl(view);
	}
}