using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Velura.Helpers;
using Velura.Services;
using Velura.ViewModels;

namespace Velura.iOS.Views.Home;

public class HomeViewController : UICollectionViewController
{
	static UICollectionViewLayout CreateLayout()
	{
		NSCollectionLayoutSize itemSize = NSCollectionLayoutSize.Create(
			NSCollectionLayoutDimension.CreateEstimated(104),
			NSCollectionLayoutDimension.CreateEstimated(206)
		);
		NSCollectionLayoutItem item = NSCollectionLayoutItem.Create(itemSize);
		NSCollectionLayoutGroup itemGroup = NSCollectionLayoutGroup.CreateHorizontal(itemSize, item, 1);

		NSCollectionLayoutSize headerSize = NSCollectionLayoutSize.Create(
			NSCollectionLayoutDimension.CreateFractionalWidth(1),
			NSCollectionLayoutDimension.CreateAbsolute(40)
		);
		NSCollectionLayoutBoundarySupplementaryItem header = NSCollectionLayoutBoundarySupplementaryItem.Create(headerSize, UICollectionElementKindSectionKey.Header, NSRectAlignment.Top);
		
		NSCollectionLayoutSection section = NSCollectionLayoutSection.Create(itemGroup);
		section.ContentInsets = new(4, 16, 16, 16);
		section.InterGroupSpacing = 8;
		section.OrthogonalScrollingBehavior = UICollectionLayoutSectionOrthogonalScrollingBehavior.ContinuousGroupLeadingBoundary;
		section.BoundarySupplementaryItems = [header];
		
		UICollectionViewCompositionalLayout layout = new(section);
		return layout;
	}
	
	
	readonly HomeViewModel viewModel = App.Provider.GetRequiredService<HomeViewModel>();
	readonly ImageCache imageCache = App.Provider.GetRequiredService<ImageCache>();
	
	readonly List<string> sections = [];

	public HomeViewController() : base(CreateLayout())
	{
		Title = "home_title".L10N();
		TabBarItem.Image = UIImage.GetSystemImage("house");
		TabBarItem.SelectedImage = UIImage.GetSystemImage("house.fill");
	}
	
	public override void ViewDidLoad()
	{
		base.ViewDidLoad();
		
		// Properties
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		
		// Collection View
		CollectionView.RegisterClassForCell(typeof(MediaContainerViewCell), nameof(MediaContainerViewCell));
		CollectionView.RegisterClassForSupplementaryView(typeof(MediaSectionHeaderView), UICollectionElementKindSection.Header, nameof(MediaSectionHeaderView));

		CollectionView.LayoutMargins = UIEdgeInsets.Zero;
		
		viewModel.PropertyChanged += OnViewModelPropertyChanged;
	}

	
	void OnViewModelPropertyChanged(
		object? sender,
		PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(HomeViewModel.Movies):
			case nameof(HomeViewModel.Shows):
				UpdateSections();
				CollectionView.ReloadData();
				break;
		}
	}
	
	void UpdateSections()
	{
		sections.Clear();

		if (viewModel.Movies?.Length > 0)
			sections.Add(nameof(HomeViewModel.Movies));
		if (viewModel.Shows?.Length > 0)
			sections.Add(nameof(HomeViewModel.Shows));
	}



	public override nint NumberOfSections(
		UICollectionView collectionView) =>
		sections.Count;

	public override nint GetItemsCount(
		UICollectionView collectionView,
		nint section) =>
		sections[(int)section] switch
		{
			nameof(HomeViewModel.Movies) => viewModel.Movies!.Length,
			nameof(HomeViewModel.Shows) => viewModel.Shows!.Length,
			
			_ => 0
		};
	
	public override UICollectionReusableView GetViewForSupplementaryElement(
		UICollectionView collectionView,
		NSString kind,
		NSIndexPath indexPath)
	{
		if (kind != UICollectionElementKindSectionKey.Header)
			return default!;
		
		MediaSectionHeaderView header = (MediaSectionHeaderView)collectionView.DequeueReusableSupplementaryView(UICollectionElementKindSection.Header, nameof(MediaSectionHeaderView), indexPath);
		switch (sections[indexPath.Section])
		{
			case nameof(HomeViewModel.Movies):
				header.UpdateHeader("media_movies".L10N(), viewModel.ShowMediaSectionCommand, nameof(HomeViewModel.Movies));
				return header;
			case nameof(HomeViewModel.Shows):
				header.UpdateHeader("media_shows".L10N(), viewModel.ShowMediaSectionCommand, nameof(HomeViewModel.Shows));
				return header;
			
			default:
				return default!;
		}
	}

	
	public override UICollectionViewCell GetCell(
		UICollectionView collectionView,
		NSIndexPath indexPath)
	{
		MediaContainerViewCell cell = (MediaContainerViewCell)collectionView.DequeueReusableCell(nameof(MediaContainerViewCell), indexPath);
		switch (sections[indexPath.Section])
		{
			case nameof(HomeViewModel.Movies):
				cell.UpdateCell(viewModel.Movies![indexPath.Row], imageCache, indexPath.Row);
				return cell;
			case nameof(HomeViewModel.Shows):
				cell.UpdateCell(viewModel.Shows![indexPath.Row], imageCache, indexPath.Row);
				return cell;
			
			default:
				return default!;
		}
	}
	
	public override void ItemSelected(
		UICollectionView collectionView,
		NSIndexPath indexPath)
	{
	}
}