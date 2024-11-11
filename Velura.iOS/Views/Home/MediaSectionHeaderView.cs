using System.Windows.Input;
using Cirrious.FluentLayouts.Touch;
using ObjCRuntime;
using Velura.Helpers;
using Velura.iOS.Helpers;

namespace Velura.iOS.Views.Home;

public class MediaSectionHeaderView : UICollectionReusableView
{
	readonly UILabel textLabel;

	ICommand? showMediaSectionCommand = null;
	string? sectionName = null;
	
	public MediaSectionHeaderView(
		NativeHandle handle) : base(handle)
	{
		// UI
		textLabel = new()
		{
			Font = UIFontMetrics.DefaultMetrics.GetScaledFont(UIFont.BoldSystemFontOfSize(20)),
			AdjustsFontForContentSizeCategory = true,
			TextColor = UIColor.Label,
			Lines = 1,
			LineBreakMode = UILineBreakMode.TailTruncation
		};
		UIButton showAllButton = UIButtonConfiguration.PlainButtonConfiguration.CreateButton(
			title: "home_show_all".L10N(),
			buttonSize: UIButtonConfigurationSize.Small,
			image: UIImage.GetSystemImage("chevron.right.circle"),
			onPress: _ => showMediaSectionCommand?.Execute(sectionName));
		AddSubviews(textLabel, showAllButton);
		
		// Layout
		this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
		this.AddConstraints(
			textLabel.WithSameCenterY(this),
			textLabel.AtLeftOf(this),

			showAllButton.WithSameCenterY(this),
			showAllButton.AtRightOf(this, -10)
		);
	}

	
	void SetText(
		string text)
	{
		textLabel.Text = text;
	}
	
	void SetOnShowAllPressedCommand(
		ICommand showMediaSectionCommand,
		string sectionName)
	{
		this.showMediaSectionCommand = showMediaSectionCommand;
		this.sectionName = sectionName;
	}


	public void UpdateHeader(
		string sectionTitle,
		ICommand showMediaSectionCommand,
		string sectionName)
	{
		SetText(sectionTitle);
		SetOnShowAllPressedCommand(showMediaSectionCommand, sectionName);
	}
}