using Velura.iOS.Views.Abstract;
using Velura.iOS.Views.Cells;
using Velura.Models.Abstract;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public sealed class SettingsViewController() : TabbedTableViewController<SettingsViewModel>("Settings", "gearshape", "gearshape.fill", UITableViewStyle.InsetGrouped)
{
    readonly NSString cellIdentifier = new(nameof(SettingViewCell));

    
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        
        TableView.RegisterClassForCellReuse(typeof(SettingViewCell), cellIdentifier);
    }
    
    
    public override UIView GetViewForHeader(UITableView tableView, nint section)
    {
        // Create a header view with 30px spacing at the top
        UIView headerView = new UIView { BackgroundColor = UIColor.Clear };
        SettingsHeaderView settingsHeader = new SettingsHeaderView("Apple Account");

        // Add the header to the view
        headerView.AddSubview(settingsHeader);

        // Set constraints for the header view to include 30px spacing
        settingsHeader.TranslatesAutoresizingMaskIntoConstraints = false;
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            settingsHeader.LeadingAnchor.ConstraintEqualTo(headerView.LeadingAnchor),
            settingsHeader.TrailingAnchor.ConstraintEqualTo(headerView.TrailingAnchor),
            settingsHeader.TopAnchor.ConstraintEqualTo(headerView.TopAnchor),
            settingsHeader.BottomAnchor.ConstraintEqualTo(headerView.BottomAnchor, -30)
        });

        return headerView;
    }

    
    public override nint RowsInSection(
        UITableView tableview,
        nint section) =>
        IConfig.Groups.Count;
    
    public override UITableViewCell GetCell(
        UITableView tableView,
        NSIndexPath indexPath)
    {
        SettingViewCell cell = (SettingViewCell)tableView.DequeueReusableCell(cellIdentifier, indexPath);

        cell.UpdateCell(IConfig.Groups.Keys.ElementAt(indexPath.Row), UIImage.GetSystemImage("gear"), UIColor.Gray, UIColor.White);
        return cell;
    }

    public override void RowSelected(
        UITableView tableView,
        NSIndexPath indexPath)
    {
        tableView.DeselectRow(indexPath, true);
    }
}


public sealed class SettingsHeaderView : UIView
{
    public SettingsHeaderView(string title)
    {
        BackgroundColor = UIColor.SecondarySystemBackground;
        Layer.CornerRadius = 8;

        
        UILabel titleLabel = new()
        {
            Text = title,
            Font = UIFont.BoldSystemFontOfSize(20),
            TranslatesAutoresizingMaskIntoConstraints = false
        };

        AddSubview(titleLabel);

        NSLayoutConstraint.ActivateConstraints([
            titleLabel.LeadingAnchor.ConstraintEqualTo(LeadingAnchor, 15),
            titleLabel.TrailingAnchor.ConstraintEqualTo(TrailingAnchor, -15),
            titleLabel.TopAnchor.ConstraintEqualTo(TopAnchor, 10),
            titleLabel.BottomAnchor.ConstraintEqualTo(BottomAnchor, -10)
        ]);
    }
}

