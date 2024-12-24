using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Services;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class MediaSectionViewModel<TMediaContainer> : ObservableObject where TMediaContainer : IMediaContainer, new()
{
	readonly ILogger<MediaSectionViewModel<TMediaContainer>> logger;
	readonly INavigation navigation;
	
	public Config Config { get; }
	public ImageCache ImageCache { get; }
	public MediaLibrary MediaLibrary { get; }
	public string SectionName { get; }
	public ObservableRangeCollection<TMediaContainer> MediaContainers { get; }

	public MediaSectionViewModel(
		ILogger<MediaSectionViewModel<TMediaContainer>> logger,
		Config config,
		ImageCache imageCache,
		INavigation navigation,
		MediaLibrary mediaLibrary,
		string sectionName,
		ObservableRangeCollection<TMediaContainer> mediaContainers)
	{
		this.logger = logger;
		this.Config = config;
		this.ImageCache = imageCache;
		this.navigation = navigation;
		this.MediaLibrary = mediaLibrary;
		this.SectionName = sectionName;
		this.MediaContainers = mediaContainers;

		logger.LogInformation("[MediaSectionViewModel-.ctor] MediaSectionViewModel has been initialized.");
	}
	
	
	[RelayCommand]
	async Task RemoveMediaContainerAsync(
		TMediaContainer mediaContainer)
	{
		void ReturnToPreviousPage()
		{
			logger.LogInformation("[MediaSectionViewModel-RemoveMediaContainerAsync] No media containers left: Returning to previous page.");
			navigation.Pop();
		}
		
		switch (mediaContainer)
		{
			case Movie movie:
				await MediaLibrary.RemoveMovieAsync(movie);
				if (MediaLibrary.Movies.Count < 1)
					ReturnToPreviousPage();
				break;
			case Show show:
				await MediaLibrary.RemoveShowAsync(show);
				if (MediaLibrary.Shows.Count < 1)
					ReturnToPreviousPage();
				break;
			default:
				throw new("This media container type is not supported.");
		}
	}
}