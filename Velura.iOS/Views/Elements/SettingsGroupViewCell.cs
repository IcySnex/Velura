using ObjCRuntime;

namespace Velura.iOS.Views.Elements;

public sealed class SettingsGroupViewCell : UITableViewCell
{
	public SettingsGroupViewCell(
		NativeHandle handle) : base(handle) =>
		Initialize();

	public SettingsGroupViewCell(
		NSString cellId) : base(UITableViewCellStyle.Default, cellId) =>
		Initialize();


	UILabel nameLabel = default!;
	UIImageView imageView = default!;
	
	void Initialize()
	{
		TranslatesAutoresizingMaskIntoConstraints = false;
		Accessory = UITableViewCellAccessory.DisclosureIndicator;
		
		
		nameLabel = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		ContentView.AddSubview(nameLabel);
		
		imageView = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false,
			ContentMode = UIViewContentMode.Center,
			ClipsToBounds = true,
			Layer =
			{
				CornerRadius = 8,
				MasksToBounds = true
			}
		};
		ContentView.AddSubview(imageView);

		NSLayoutConstraint.ActivateConstraints([
			imageView.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, 15),
			imageView.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor),
			imageView.WidthAnchor.ConstraintEqualTo(30),
			imageView.HeightAnchor.ConstraintEqualTo(30),
			nameLabel.LeadingAnchor.ConstraintEqualTo(imageView.TrailingAnchor, 15),
			nameLabel.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor),
			nameLabel.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, 15)

		]);
	}

	public void UpdateCell(
		string name,
		UIImage? image,
		UIColor imageBackgroundColor,
		UIColor? imageTintColor = null)
	{
		nameLabel.Text = name;
		imageView.Image = imageTintColor is null ? image : image?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
		imageView.BackgroundColor = imageBackgroundColor;
		imageView.TintColor = imageTintColor;

	}
}