using Cirrious.FluentLayouts.Touch;
using ObjCRuntime;

namespace Velura.iOS.Views.Elements;

public sealed class SettingsGroupViewCell : UITableViewCell
{
	readonly UILabel nameLabel;
	readonly UIImageView imageView;
	
	public SettingsGroupViewCell(
		NativeHandle handle) : base(handle)
	{
		// Properties
		Accessory = UITableViewCellAccessory.DisclosureIndicator;

		// UI
		nameLabel = new();
		ContentView.AddSubview(nameLabel);
		
		imageView = new()
		{
			ContentMode = UIViewContentMode.Center,
			ClipsToBounds = true,
			Layer =
			{
				CornerRadius = 8,
				MasksToBounds = true
			},
		};
		ContentView.AddSubview(imageView);

		ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		ContentView.AddConstraints(
			imageView.Width().EqualTo(30),
			imageView.Height().EqualTo(30),
			imageView.AtLeftOf(ContentView, 15),
			imageView.WithSameCenterY(ContentView),

			nameLabel.ToRightOf(imageView, 15),
			nameLabel.AtRightOf(ContentView, 15),
			nameLabel.WithSameCenterY(ContentView)
		);
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