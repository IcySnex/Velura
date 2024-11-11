using Velura.Models.Abstract;

namespace Velura.iOS.Views.Home;

public class MediaSectionViewController : UICollectionViewController
{
	readonly IMediaContainer[] collection;
	
	public MediaSectionViewController(
		string name,
		IMediaContainer[] collection)
	{
		this.collection = collection;
		
		// Properties
		Title = name;
		View!.BackgroundColor = UIColor.SystemGroupedBackground;
		
		// Collection View
		CollectionView.RegisterClassForCell(typeof(MediaContainerViewCell), nameof(MediaContainerViewCell));

		CollectionView.LayoutMargins = UIEdgeInsets.Zero;
	}
}