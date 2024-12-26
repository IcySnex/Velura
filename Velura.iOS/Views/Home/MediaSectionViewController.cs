using System.Collections.Specialized;
using System.ComponentModel;
using Velura.Helpers;
using Velura.iOS.Helpers;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.ViewModels;

namespace Velura.iOS.Views.Home;

public class MediaSectionViewController<TMediaContainer> : UICollectionViewController where TMediaContainer : IMediaContainer, new()
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
	
	
	readonly MediaSectionViewModel<TMediaContainer> viewModel;
	
	public MediaSectionViewController(
		MediaSectionViewModel<TMediaContainer> viewModel) : base(CreateLayout())
	{
		this.viewModel = viewModel;
		
		// Properties
		Title = viewModel.SectionName;
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		NavigationItem.LargeTitleDisplayMode = UINavigationItemLargeTitleDisplayMode.Never;
		
		// Collection View
		CollectionView.RegisterClassForCell(typeof(MediaContainerViewCell), nameof(MediaContainerViewCell));

		CollectionView.LayoutMargins = UIEdgeInsets.Zero;
		
		viewModel.MediaContainers.CollectionChanged += OnMediaContainersChanged;
		viewModel.Config.Home.PropertyChanged += OnConfigHomePropertyChanged;
	}

	
	void OnMediaContainersChanged(
		object? sender,
		NotifyCollectionChangedEventArgs e) =>
		CollectionView.ReloadData(0, e);

	
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
		cell.UpdateCell(viewModel.MediaContainers[indexPath.Row], viewModel.Config, indexPath.Row);
		return cell;
	}
	
	public override void ItemSelected(
		UICollectionView collectionView,
		NSIndexPath indexPath) =>
		viewModel.ShowMediaContainerInfoCommand.Execute(viewModel.MediaContainers[indexPath.Row]);

	
	public override UIContextMenuConfiguration GetContextMenuConfiguration(
		UICollectionView collectionView,
		NSIndexPath indexPath,
		CGPoint point) =>
		UIContextMenuConfiguration.Create(indexPath, null, _ =>
		{
			UIMenu topActions = UIMenu.Create("", null, UIMenuIdentifier.None, UIMenuOptions.DisplayInline,
			[
				UIAction.Create("media_play".L10N(), UIImage.GetSystemImage("play"), null, _ => { }),
				UIAction.Create("media_mark_as_watched".L10N(), UIImage.GetSystemImage("flag"), null, _ => { }),
			]);
			
			UIAction deleteAction = UIAction.Create("media_remove".L10N(), UIImage.GetSystemImage("trash"), null, viewModel.RemoveMediaContainerCommand.ToUIActionHandler(viewModel.MediaContainers[indexPath.Row]));
			deleteAction.Attributes = UIMenuElementAttributes.Destructive;

			return UIMenu.Create([topActions, deleteAction]);
		});

	public override UITargetedPreview? GetPreviewForHighlightingContextMenu(
		UICollectionView collectionView,
		UIContextMenuConfiguration configuration) =>
		collectionView.CellForItem((NSIndexPath)configuration.Identifier)?.ContentView.CreateTargetedPreview(6, 8);
	public override UITargetedPreview? GetPreviewForDismissingContextMenu(
		UICollectionView collectionView,
		UIContextMenuConfiguration configuration) =>
		collectionView.CellForItem((NSIndexPath)configuration.Identifier)?.ContentView.CreateTargetedPreview(6, 8);
}