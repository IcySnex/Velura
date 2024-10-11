namespace Velura.iOS.Views.Elements;

public sealed class SettingsHeaderView : UIView
{
	public SettingsHeaderView(
		string title)
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