using Velura.iOS.Helpers;
using Velura.iOS.Views.Abstract;
using Velura.iOS.Views.Elements;
using Velura.Models;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public sealed class SettingsViewController() : TabbedTableViewController<SettingsViewModel>("Settings", "gearshape", "gearshape.fill", UITableViewStyle.InsetGrouped)
{
    readonly NSString cellIdentifier = new(nameof(SettingsGroupViewCell));

    
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        
        TableView.RegisterClassForCellReuse(typeof(SettingsGroupViewCell), cellIdentifier);
    }
    
    
    public override UIView GetViewForHeader(UITableView tableView, nint section)
    {
        UIView headerView = new() { BackgroundColor = UIColor.Clear };
        
        SettingsHeaderView settingsHeader = new("Velura");
        headerView.AddSubview(settingsHeader);
        
        NSLayoutConstraint.ActivateConstraints([
            settingsHeader.LeadingAnchor.ConstraintEqualTo(headerView.LeadingAnchor),
            settingsHeader.TrailingAnchor.ConstraintEqualTo(headerView.TrailingAnchor),
            settingsHeader.TopAnchor.ConstraintEqualTo(headerView.TopAnchor),
            settingsHeader.BottomAnchor.ConstraintEqualTo(headerView.BottomAnchor, -30)
        ]);

        return headerView;
    }


    public override nint RowsInSection(
        UITableView tableview,
        nint section) =>
        ViewModel!.Groups.Count;
    
    public override UITableViewCell GetCell(
        UITableView tableView,
        NSIndexPath indexPath)
    {
        SettingsGroupViewCell cell = (SettingsGroupViewCell)tableView.DequeueReusableCell(cellIdentifier, indexPath);
        
        (DetailsAttribute Details, ImageAttribute Image, IReadOnlyList<DetailsAttribute> Properties) group = ViewModel!.Groups[indexPath.Row];
        cell.UpdateCell(group.Details.Name, UIImage.GetSystemImage(group.Image.ResourceName), group.Image.BackgroundColor.ToUIColor(), group.Image.TintColor?.ToUIColor());
        
        return cell;
    }

    public override void RowSelected(
        UITableView tableView,
        NSIndexPath indexPath)
    {
        tableView.DeselectRow(indexPath, true);
    }
}