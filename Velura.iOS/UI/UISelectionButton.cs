namespace Velura.iOS.UI;

public class UISelectionButton : UIButton
{
	public UISelectionButton()
	{
		UIButtonConfiguration buttonConfiguration = UIButtonConfiguration.PlainButtonConfiguration;
		buttonConfiguration.Indicator = UIButtonConfigurationIndicator.Popup;

		Configuration = buttonConfiguration;
		ChangesSelectionAsPrimaryAction = true;
		ShowsMenuAsPrimaryAction = true;
	}


	public event EventHandler? SelectedItemChanged;

	protected void RaiseSelectedItemChanged(
		UIAction action) =>
		SelectedItemChanged?.Invoke(this, EventArgs.Empty);


	string? selectedItem;
	public string? SelectedItem
	{
		get => selectedItem;
		set
		{
			selectedItem = value;

			if (Menu is null)
				return;
			
			if (value is null)
			{
				foreach (UIMenuElement element in Menu.Children)
					if (element is UIAction action1)
						action1.State = UIMenuElementState.Off;
			}
			else if (Menu.Children.FirstOrDefault(menuElement => menuElement.Title == value) is UIAction action)
				action.State = UIMenuElementState.On;
		}
	}

	IEnumerable<string>? items;
	public IEnumerable<string>? Items
	{
		get => items;
		set
		{
			items = value;
			if (value is null)
			{
				Menu = null;
				return;
			}
			
			Menu = UIMenu.Create(value.Select(item =>
			{
				UIAction action = UIAction.Create(item, null, null, RaiseSelectedItemChanged);
				action.State = item == SelectedItem ? UIMenuElementState.On : UIMenuElementState.Off;

				return action;
			}).ToArray<UIMenuElement>());
		}
	}
}