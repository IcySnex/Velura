using Velura.iOS.Binding;
using Velura.iOS.Helpers;
using Velura.iOS.UI;
using Velura.iOS.Views.Elements;
using Velura.Models;
using Velura.Models.Abstract;

namespace Velura.iOS.Views;

public class SettingsGroupViewController : UITableViewController, IUIScrollViewDelegate
{
	readonly SettingsGroup group;
	readonly BindingSet<ConfigGroup> bindingSet;
	readonly Dictionary<string, SettingsGroupViewController> viewControllersCache;
	
	readonly ConcealingTitleView? titleView;

	readonly int groupSectionIndex;
	readonly int propertySectionIndex;
	
	public SettingsGroupViewController(
		SettingsGroup group,
		BindingSet<ConfigGroup> bindingSet,
		Dictionary<string, SettingsGroupViewController> viewControllersCache,
		bool createHeader = true) : base(UITableViewStyle.InsetGrouped)
	{
		this.group = group;
		this.bindingSet = bindingSet;
		this.viewControllersCache = viewControllersCache;
		
		groupSectionIndex = group.Groups.Count > 0 ? 0 : -1;
		propertySectionIndex = group.Properties.Count > 0 ? groupSectionIndex + 1 : -1;

		// Properties
		Title = group.Details.Name;
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;
		
		// TableView
		TableView.RegisterClassForCellReuse(typeof(SettingsItemViewCell), nameof(SettingsItemViewCell));

		TableView.LayoutMargins = UIEdgeInsets.Zero;

		TableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
		TableView.AddGestureRecognizer(new UITapGestureRecognizer(() => View!.EndEditing(true))
		{
			CancelsTouchesInView = false,
		});
		
		// Title/Header
		if (createHeader)
		{
			titleView = new(group.Details.Name);
			NavigationItem.TitleView = titleView;

			TableView.TableHeaderView = new UIPaddedView()
			{
				ChildView = new SettingsHeaderView(group.Details.Name, group.Details.Description, UIImage.GetSystemImage(group.Image.ResourceName), group.Image.BackgroundColor.ToUIColor(), group.Image.TintColor.ToUIColor()),
				Padding = new(0, 16, 32, 16)
			};
		}
	}


	public void Scrolled(
		UIScrollView scrollView) =>
		titleView?.ScrollViewDidScroll(scrollView.ContentOffset.Y);


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
		SettingsItemViewCell cell = (SettingsItemViewCell)tableView.DequeueReusableCell(nameof(SettingsItemViewCell), indexPath);
		
		if (indexPath.Section == groupSectionIndex)
			cell.UpdateCell(group.Groups[indexPath.Row]);
		if (indexPath.Section == propertySectionIndex)
			cell.UpdateCell(group.Properties[indexPath.Row], bindingSet);

		return cell;
	}

	public override void RowSelected(
		UITableView tableView,
		NSIndexPath indexPath)
	{
		TableView.DeselectRow(indexPath, true);
		
		if (indexPath.Section != groupSectionIndex)
			return;
		
		SettingsGroup selectedGroup = group.Groups[indexPath.Row];
		if (!viewControllersCache.TryGetValue(selectedGroup.Path, out SettingsGroupViewController? viewController))
		{
			viewController = new(selectedGroup, bindingSet.CreateSubSet<ConfigGroup>(selectedGroup.Path), viewControllersCache);
			viewControllersCache[selectedGroup.Path] = viewController;
		}
		NavigationController?.PushViewController(viewController, true);
	}


	protected override void Dispose(
		bool disposing)
	{
		if (disposing)
		{
			// Dispose managed state
			bindingSet.Dispose();
			titleView?.Dispose();
			TableView.TableHeaderView?.Dispose();
		}
		
		base.Dispose(disposing);
	}
}