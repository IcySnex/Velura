using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TMDbLib.Objects.Search;
using Velura.Helpers;
using Velura.Models;
using Velura.Services;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	readonly ILogger<HomeViewModel> logger;
	readonly Database database;
	readonly INavigation navigation;
	readonly MediaInfoProvider mediaInfoProvider;
	
	public ImageCache ImageCache { get; }

	public HomeViewModel(
		ILogger<HomeViewModel> logger,
		Database database,
		ImageCache imageCache,
		INavigation navigation,
		MediaInfoProvider mediaInfoProvider)
	{
		this.logger = logger;
		this.navigation = navigation;
		this.ImageCache = imageCache;
		this.database = database;
		this.mediaInfoProvider = mediaInfoProvider;

		async void LoadData() =>
			await ReloadDataAsync();
		LoadData();
		
		logger.LogInformation("[HomeViewModel-.ctor] HomeViewModel has been initialized.");
	}


	[ObservableProperty]
	IReadOnlyList<Movie>? movies = null;

	[ObservableProperty]
	IReadOnlyList<Show>? shows = null;

	public async Task ReloadDataAsync()
	{
		logger.LogInformation("[HomeViewModel-ReloadDataAsync] Reloading data from database...");

		// await AddMovieAsync("Soul");
		// await AddMovieAsync("Suzume");
		// await AddMovieAsync("Forest Gump");
		// await AddMovieAsync("A Goofy Movie");
		// await AddMovieAsync("Fatherhood");
		// await AddMovieAsync("Jennifer's Body");
		// await AddMovieAsync("Space Jam");
		// await AddMovieAsync("Joker");
		// await AddMovieAsync("Your Name");
		
		Movies = await database.GetAsync<Movie>();
		Shows = await database.GetAsync<Show>();
	}


	async Task AddMovieAsync(
		string query)
	{
		SearchMovie searchResult = await mediaInfoProvider.SearchMovieAsync(query);

		// Add movie
		Movie movie = new()
		{
			Title = searchResult.Title,
			FilePath = "/placeholder/path",
			Description = searchResult.Overview,
			PosterPath = MediaInfoProvider.GetImageUrl(searchResult.PosterPath, "w200"),
			ReleaseDate = searchResult.ReleaseDate,
			Duration = TimeSpan.Zero
		};
		await database.InsertAsync(movie);
		
		// Add genres
		foreach (int genreId in searchResult.GenreIds)
		{
			GenreMap genreMap = new()
			{
				GenreId = genreId,
				MediaContainerId = movie.Id
			};
			await database.InsertAsync(genreMap);
		}
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