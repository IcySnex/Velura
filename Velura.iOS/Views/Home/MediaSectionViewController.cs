using System.ComponentModel;
using Velura.Models;
using Velura.ViewModels;

namespace Velura.iOS.Views.Home;

public class MediaSectionViewController : UICollectionViewController
{
	static UICollectionViewLayout CreateLayout()
	{
		UICollectionViewCompositionalLayoutConfiguration layoutConfig = new();
    
		UICollectionViewCompositionalLayout layout = new((_, layoutEnvironment) =>
		{
			int itemsPerRow;
			nfloat width = layoutEnvironment.Container.EffectiveContentSize.Width;
			if (width > 1000)
				itemsPerRow = 8;
			else if (width > 800)
				itemsPerRow = 6;
			else if (width > 700)
				itemsPerRow = 5;
			else if (width > 500)
				itemsPerRow = 4;
			else
				itemsPerRow = 3;

			NSCollectionLayoutSize itemSize = NSCollectionLayoutSize.Create(
				NSCollectionLayoutDimension.CreateFractionalWidth(1 / itemsPerRow),
				NSCollectionLayoutDimension.CreateEstimated(206)
			);
			NSCollectionLayoutItem item = NSCollectionLayoutItem.Create(itemSize);

			NSCollectionLayoutSize groupSize = NSCollectionLayoutSize.Create(
				NSCollectionLayoutDimension.CreateFractionalWidth(1),
				NSCollectionLayoutDimension.CreateEstimated(206)
			);
			NSCollectionLayoutGroup group = NSCollectionLayoutGroup.CreateHorizontal(groupSize, item, itemsPerRow);
			group.InterItemSpacing = NSCollectionLayoutSpacing.CreateFixed(10);

			NSCollectionLayoutSection section = NSCollectionLayoutSection.Create(group);
			section.ContentInsets = new(4, 16, 4, 16);
			section.InterGroupSpacing = 10;
			section.OrthogonalScrollingBehavior = UICollectionLayoutSectionOrthogonalScrollingBehavior.None;

			return section;
		}, layoutConfig);

		return layout;
	}
	
	
	readonly MediaSectionViewModel viewModel;
	
	public MediaSectionViewController(
		MediaSectionViewModel viewModel) : base(CreateLayout())
	{
		this.viewModel = viewModel;
		
		// Properties
		Title = viewModel.SectionName;
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;
		
		// Collection View
		CollectionView.RegisterClassForCell(typeof(MediaContainerViewCell), nameof(MediaContainerViewCell));

		CollectionView.LayoutMargins = UIEdgeInsets.Zero;
		
		viewModel.Config.Home.PropertyChanged += OnConfigHomePropertyChanged;
	}
	
	
	void OnConfigHomePropertyChanged(
		object? sender,
		PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(ConfigHome.AllowLineWrap):
			case nameof(ConfigHome.MediaContainerDescription):
				CollectionView.ReloadData();
				break;
		}
	}
	
	
	public override nint NumberOfSections(
		UICollectionView collectionView) =>
		1;
	
	public override nint GetItemsCount(
		UICollectionView collectionView,
		nint section) =>
		viewModel.MediaContainers.Count;
	
	
	public override UICollectionViewCell GetCell(
		UICollectionView collectionView,
		NSIndexPath indexPath)
	{
		MediaContainerViewCell cell = (MediaContainerViewCell)collectionView.DequeueReusableCell(nameof(MediaContainerViewCell), indexPath);
		cell.UpdateCell(viewModel.MediaContainers[indexPath.Row], viewModel.Config, viewModel.ImageCache, indexPath.Row);
		return cell;
	}
}