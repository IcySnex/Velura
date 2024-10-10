using ObjCRuntime;

namespace Velura.iOS.Views.Cells;

public sealed class SettingViewCell : UITableViewCell
{
	public SettingViewCell(
		NativeHandle handle) : base(handle) =>
		Initialize();

	public SettingViewCell(
		NSString cellId) : base(UITableViewCellStyle.Default, cellId) =>
		Initialize();


	UILabel titleLabel = default!;
	UIImageView imageView = default!;
	
	void Initialize()
	{
		Accessory = UITableViewCellAccessory.DisclosureIndicator;
		
		
		titleLabel = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		
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
		
		ContentView.AddSubview(titleLabel);
		ContentView.AddSubview(imageView);

		NSLayoutConstraint.ActivateConstraints(new[]
		{
			imageView.LeadingAnchor.ConstraintEqualTo(ContentView.LeadingAnchor, 15),
			imageView.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor),
			imageView.WidthAnchor.ConstraintEqualTo(30),
			imageView.HeightAnchor.ConstraintEqualTo(30),
			titleLabel.LeadingAnchor.ConstraintEqualTo(imageView.TrailingAnchor, 15),
			titleLabel.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor),
			titleLabel.TrailingAnchor.ConstraintEqualTo(ContentView.TrailingAnchor, 15)

		});
	}

	public void UpdateCell(
		string title,
		UIImage? image,
		UIColor imageBackgroundColor,
		UIColor imageForegroundColor)
	{
		titleLabel.Text = title;
		imageView.Image = image?.Scale(new (20, 20)).ApplyTintColor(imageForegroundColor, UIImageRenderingMode.Automatic);
		imageView.BackgroundColor = imageBackgroundColor;
	}
}