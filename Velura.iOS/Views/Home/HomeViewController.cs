using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Velura.Helpers;
using Velura.iOS.Helpers;
using Velura.Models;
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
		
		NSCollectionLayoutGroup group = NSCollectionLayoutGroup.CreateHorizontal(itemSize, item, 1);

		NSCollectionLayoutSize headerSize = NSCollectionLayoutSize.Create(
			NSCollectionLayoutDimension.CreateFractionalWidth(1),
			NSCollectionLayoutDimension.CreateAbsolute(32)
		);
		NSCollectionLayoutBoundarySupplementaryItem header = NSCollectionLayoutBoundarySupplementaryItem.Create(headerSize, UICollectionElementKindSectionKey.Header, NSRectAlignment.Top);
		
		NSCollectionLayoutSection section = NSCollectionLayoutSection.Create(group);
		section.ContentInsets = new(4, 16, 16, 16);
		section.InterGroupSpacing = 10;
		section.OrthogonalScrollingBehavior = UICollectionLayoutSectionOrthogonalScrollingBehavior.ContinuousGroupLeadingBoundary;
		section.BoundarySupplementaryItems = [header];
		
		UICollectionViewCompositionalLayout layout = new(section);
		return layout;
	}
	
	
	readonly HomeViewModel viewModel = App.Provider.GetRequiredService<HomeViewModel>();
	
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

		viewModel.MediaLibrary.Movies.CollectionChanged += OnMediaLibraryMoviesChanged;
		viewModel.MediaLibrary.Shows.CollectionChanged += OnMediaLibraryShowsChanged;
		viewModel.Config.Home.PropertyChanged += OnConfigHomePropertyChanged;
	}

	
	void OnMediaLibraryMoviesChanged(
		object? sender,
		NotifyCollectionChangedEventArgs e)
	{
		if (UpdateSections())
		{
			CollectionView.ReloadData();
			return;
		}

		int sectionIndex = sections.IndexOf(nameof(MediaLibrary.Movies));
		if (sectionIndex != -1)
			CollectionView.ReloadData(sectionIndex, e);
	}

	void OnMediaLibraryShowsChanged(
		object? sender,
		NotifyCollectionChangedEventArgs e)
	{
		if (UpdateSections())
		{
			CollectionView.ReloadData();
			return;
		}

		int sectionIndex = sections.IndexOf(nameof(MediaLibrary.Shows));
		if (sectionIndex != -1)
			CollectionView.ReloadData(sectionIndex, e);
	}

	
	void OnConfigHomePropertyChanged(
		object? sender,
		PropertyChangedEventArgs e)
	{
		switch (e.PropertyName)
		{
			case nameof(ConfigHome.AllowLineWrap):
			case nameof(ConfigHome.MediaContainerDescription):
				UpdateSections();
				CollectionView.ReloadData();
				break;
		}
	}
	
	
	bool UpdateSections()
	{
		int sectionsCountBefore = sections.Count;
		
		sections.Clear();

		if (viewModel.MediaLibrary.Movies.Count > 0)
			sections.Add(nameof(MediaLibrary.Movies));
		if (viewModel.MediaLibrary.Shows.Count > 0)
			sections.Add(nameof(MediaLibrary.Shows));
		
		return sectionsCountBefore != sections.Count;
	}



	public override nint NumberOfSections(
		UICollectionView collectionView) =>
		sections.Count;

	public override nint GetItemsCount(
		UICollectionView collectionView,
		nint section) =>
		sections[(int)section] switch
		{
			nameof(MediaLibrary.Movies) => viewModel.MediaLibrary.Movies.Count,
			nameof(MediaLibrary.Shows) => viewModel.MediaLibrary.Shows.Count,
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
			case nameof(MediaLibrary.Movies):
				header.UpdateHeader("media_movies".L10N(), viewModel.ShowMediaSectionCommand, nameof(MediaLibrary.Movies));
				return header;
			case nameof(MediaLibrary.Shows):
				header.UpdateHeader("media_shows".L10N(), viewModel.ShowMediaSectionCommand, nameof(MediaLibrary.Shows));
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
			case nameof(MediaLibrary.Movies):
				cell.UpdateCell(viewModel.MediaLibrary.Movies[indexPath.Row], viewModel.Config, viewModel.ImageCache, indexPath.Row);
				return cell;
			case nameof(MediaLibrary.Shows):
				cell.UpdateCell(viewModel.MediaLibrary.Shows[indexPath.Row], viewModel.Config, viewModel.ImageCache, indexPath.Row);
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
			
			UIAction deleteAction = UIAction.Create("media_remove".L10N(), UIImage.GetSystemImage("trash"), null, sections[indexPath.Section] switch
			{
				nameof(MediaLibrary.Movies) => viewModel.RemoveMovieCommand.ToUIActionHandler(viewModel.MediaLibrary.Movies[indexPath.Row]),
				nameof(MediaLibrary.Shows) => viewModel.RemoveShowCommand.ToUIActionHandler(viewModel.MediaLibrary.Shows[indexPath.Row]),
				_ => default!
			});
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