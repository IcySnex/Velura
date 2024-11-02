using System.Windows.Input;
using Cirrious.FluentLayouts.Touch;
using Velura.Helpers;
using Velura.iOS.Helpers;
using Velura.iOS.UI;
using Velura.Models;

namespace Velura.iOS.Views.About;

public class DependenciesViewController(
	Dependency[] dependencies,
	ICommand showWebpageCommand) : UIViewController
{
	readonly Dependency[] dependencies = dependencies;
	readonly ICommand showWebpageCommand = showWebpageCommand;

	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		// Properties
		Title = "about_dependencies".L10N();
		View!.BackgroundColor = UIColor.SystemGroupedBackground;

		// UI
		UIScrollView scrollView = new()
		{
			TranslatesAutoresizingMaskIntoConstraints = false
		};
		UIStackView stackView = new()
		{
			Axis = UILayoutConstraintAxis.Vertical,
			Spacing = 10,
			Distribution = UIStackViewDistribution.Fill,
			Alignment = UIStackViewAlignment.Fill,
			TranslatesAutoresizingMaskIntoConstraints = false,
		};
		UIPaddedView descriptionView = new()
		{
			ChildView = new UILabel()
			{
				Text = "about_dependencies_description".L10N(),
				Font = UIFont.PreferredBody,
				AdjustsFontForContentSizeCategory = true,
				Lines = 0,
				LineBreakMode = UILineBreakMode.WordWrap,
			},
			Padding = new(0, 0, 16, 0)
		};
		
		View.AddSubview(scrollView);
		scrollView.AddSubview(stackView);
		stackView.AddArrangedSubview(descriptionView);
		
		foreach (Dependency dependency in dependencies)
		{
			UIButton button = UIButtonConfiguration.TintedButtonConfiguration.CreateButton(
				title: dependency.Name,
				subTitle: $"v{dependency.Version}, {dependency.Author}",
				onPress: showWebpageCommand.ToUIAction(dependency.Url));
			
			stackView.AddArrangedSubview(button);
		}
		
		// Layout
		View.AddConstraints(
			scrollView.AtLeftOf(View),
			scrollView.AtRightOf(View),
			scrollView.AtTopOfSafeArea(View),
			scrollView.AtBottomOf(View),

			stackView.AtLeftOf(View, 16),
			stackView.AtRightOf(View, 16),
			stackView.AtTopOf(scrollView),
			stackView.AtBottomOf(scrollView)
		);
	}
}