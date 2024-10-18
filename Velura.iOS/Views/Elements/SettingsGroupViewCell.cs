using Cirrious.FluentLayouts.Touch;
using ObjCRuntime;
using Velura.iOS.Helpers;

namespace Velura.iOS.Views.Elements;

public sealed class SettingsGroupViewCell : UITableViewCell
{
	readonly UILabel nameLabel;
	readonly UIView imageContainer;
	readonly UIImageView imageView;
	
	public SettingsGroupViewCell(
		NativeHandle handle) : base(handle)
	{
		// Properties
		Accessory = UITableViewCellAccessory.DisclosureIndicator;

		// UI
		nameLabel = new()
		{
			Font = UIFont.PreferredBody
		};
		ContentView.AddSubview(nameLabel);

		imageContainer = new()
		{
			ClipsToBounds = true,
			Layer =
			{
				CornerRadius = 8,
				MasksToBounds = true
			},
		};
		ContentView.AddSubview(imageContainer);
		
		imageView = new()
		{
			ContentMode = UIViewContentMode.Center,
		};
		ContentView.AddSubview(imageView);

		ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		ContentView.AddConstraints(
			imageContainer.Height().EqualTo(30),
			imageContainer.Width().EqualTo(30),
			imageContainer.AtLeftOf(ContentView, 15),
			imageContainer.WithSameCenterY(ContentView),
			
			imageView.Height().EqualTo(30),
			imageView.Width().EqualTo(30),
			imageView.WithSameCenterY(imageContainer),
			imageView.WithSameCenterX(imageContainer),

			nameLabel.ToRightOf(imageView, 15),
			nameLabel.AtRightOf(ContentView, 15),
			nameLabel.WithSameCenterY(ContentView)
		);
	}

	
	public void UpdateCell(
		string name,
		UIImage image,
		UIColor imageBackgroundColor,
		UIColor imageTintColor)
	{
		nameLabel.Text = name;
		imageView.Image = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
		imageContainer.BackgroundColor = imageBackgroundColor;
		imageView.TintColor = imageTintColor;
	}
}