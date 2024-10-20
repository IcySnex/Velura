<<<<<<< HEAD
// using MvvmCross.Binding.BindingContext;
=======
using MvvmCross.Binding.BindingContext;
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
using Velura.iOS.Helpers;
using Velura.iOS.Views.Elements;
using Velura.Models;

namespace Velura.iOS.Views;

public class SettingsGroupViewController : UITableViewController
{
<<<<<<< HEAD
	// readonly IMvxBindingContextOwner bindingContextOwner;
=======
	readonly IMvxBindingContextOwner bindingContextOwner;
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
	readonly SettingsGroup group;
	
	readonly NSString groupCellIdentifier;
	readonly NSString propertyCellIdentifier;

	readonly int groupSectionIndex;
	readonly int propertySectionIndex;
	
	public SettingsGroupViewController(
<<<<<<< HEAD
		// IMvxBindingContextOwner bindingContextOwner,
		SettingsGroup group) : base(UITableViewStyle.InsetGrouped)
	{
		// this.bindingContextOwner = bindingContextOwner;
=======
		IMvxBindingContextOwner bindingContextOwner,
		SettingsGroup group) : base(UITableViewStyle.InsetGrouped)
	{
		this.bindingContextOwner = bindingContextOwner;
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
		this.group = group;
		
		// Properties
		Title = group.Details.Name;
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;

		// Cells
		groupCellIdentifier = new($"SettingsGroup[{group.Details.Name}]");
		propertyCellIdentifier = new($"SettingsProperty[{group.Details.Name}]");

		TableView.RowHeight = UITableView.AutomaticDimension;
		TableView.EstimatedRowHeight = 60;
		TableView.RegisterClassForCellReuse(typeof(SettingsGroupViewCell), groupCellIdentifier);
		TableView.RegisterClassForCellReuse(typeof(SettingsPropertyViewCell), propertyCellIdentifier);
		
		groupSectionIndex = group.Groups.Count > 0 ? 0 : -1;
		propertySectionIndex = group.Properties.Count > 0 ? groupSectionIndex + 1 : -1;

		// Dismiss Keyboard
		TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
		TableView.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true))
		{
			CancelsTouchesInView = false,
		});
	}
	

	public override nint NumberOfSections(
		UITableView tableView) =>
		group.Groups.Count > 0 ? group.Properties.Count > 0 ? 2 : 1 : group.Properties.Count > 0 ? 1 : 0;

	public override nint RowsInSection(
		UITableView tableView,
		nint section)
	{
		if (section == groupSectionIndex)
			return group.Groups.Count;
		if (section == propertySectionIndex)
			return group.Properties.Count;

		throw new IndexOutOfRangeException($"Invalid cell index path section: {section}. Supported are 0 = Group, 1 = Property.");
	}
	
	public override UITableViewCell GetCell(
		UITableView tableView,
		NSIndexPath indexPath)
	{
		if (indexPath.Section == groupSectionIndex)
		{
			SettingsGroupViewCell cell = (SettingsGroupViewCell)tableView.DequeueReusableCell(groupCellIdentifier, indexPath);
			SettingsGroup subGroup = group.Groups[indexPath.Row];
				
			cell.UpdateCell(
				subGroup.Details.Name,
				UIImage.GetSystemImage(subGroup.Image.ResourceName)!,
				subGroup.Image.BackgroundColor.ToUIColor(),
				subGroup.Image.TintColor.ToUIColor());
			return cell;
		}
		if (indexPath.Section == propertySectionIndex)
		{
			SettingsPropertyViewCell cell = (SettingsPropertyViewCell)tableView.DequeueReusableCell(propertyCellIdentifier, indexPath);
			SettingsProperty property = group.Properties[indexPath.Row];
<<<<<<< HEAD

			cell.UpdateCell(property);//, bindingContextOwner);
=======
				
			cell.UpdateCell(property, bindingContextOwner);
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
			return cell;
		}
		
		throw new IndexOutOfRangeException($"Invalid cell index path section: {indexPath.Section}. Supported are 0 = Group, 1 = Property.");
	}

	public override void RowSelected(
		UITableView tableView,
		NSIndexPath indexPath)
	{
		TableView.DeselectRow(indexPath, true);
		
		if (indexPath.Section != groupSectionIndex)
			return;
		
		SettingsGroup selectedGroup = group.Groups[indexPath.Row];
<<<<<<< HEAD
		NavigationController?.PushViewController(new SettingsGroupViewController(selectedGroup), true);//bindingContextOwner, selectedGroup), true);
=======
		NavigationController?.PushViewController(new SettingsGroupViewController(bindingContextOwner, selectedGroup), true);
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
	}
}