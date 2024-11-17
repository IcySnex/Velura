using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Services;

namespace Velura.ViewModels;

public class MediaSectionViewModel : ObservableObject
{
	readonly ILogger<MediaSectionViewModel> logger;
	
	public Config Config { get; }
	public ImageCache ImageCache { get; }

	public MediaSectionViewModel(
		ILogger<MediaSectionViewModel> logger,
		Config config,
		ImageCache imageCache,
		string sectionName,
		IReadOnlyList<IMediaContainer> mediaContainers)
	{
		this.logger = logger;
		this.Config = config;
		this.ImageCache = imageCache;
		
		SectionName = sectionName;
		MediaContainers = mediaContainers;

		logger.LogInformation("[MediaSectionViewModel-.ctor] MediaSectionViewModel has been initialized.");
	}


	public string SectionName { get; }
	
	public IReadOnlyList<IMediaContainer> MediaContainers { get; }
}