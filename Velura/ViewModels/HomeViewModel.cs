using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Services;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	readonly ILogger<HomeViewModel> logger;
	readonly Database database;
	readonly INavigation navigation;
	
	public ImageCache ImageCache { get; }

	public HomeViewModel(
		ILogger<HomeViewModel> logger,
		Database database,
		ImageCache imageCache,
		INavigation navigation)
	{
		this.logger = logger;
		this.navigation = navigation;
		this.ImageCache = imageCache;
		this.database = database;

		_ = ReloadDataAsync();
		
		logger.LogInformation("[HomeViewModel-.ctor] HomeViewModel has been initialized.");
	}


	[ObservableProperty]
	IReadOnlyList<IMediaContainer>? movies = null;

	[ObservableProperty]
	IReadOnlyList<IMediaContainer>? shows = null;

	public async Task ReloadDataAsync()
	{
		logger.LogInformation("[HomeViewModel-ReloadDataAsync] Reloading data from database...");
		
		Movies = await database.GetAsync<Movie>();
		Shows = await database.GetAsync<Show>();
	}


	[RelayCommand]
	void ShowMediaSection(
		string name)
	{
		ILogger<MediaSectionViewModel> viewModelLogger = App.Provider.GetRequiredService<ILogger<MediaSectionViewModel>>();
		MediaSectionViewModel viewModel = name switch
		{
			nameof(Movies) => new(viewModelLogger, ImageCache, "media_movies".L10N(), Movies!),
			nameof(Shows) => new(viewModelLogger, ImageCache, "media_shows".L10N(), Shows!),
			_ => new(viewModelLogger, ImageCache, name, [])
		};
		navigation.Push(viewModel);
	}
}