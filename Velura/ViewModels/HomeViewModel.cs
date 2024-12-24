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
	readonly IDialogHandler dialogHandler;
	
	public Config Config { get; }
	public ImageCache ImageCache { get; }

	public HomeViewModel(
		ILogger<HomeViewModel> logger,
		Config config,
		Database database,
		ImageCache imageCache,
		INavigation navigation,
		IDialogHandler dialogHandler)
	{
		this.logger = logger;
		this.Config = config;
		this.navigation = navigation;
		this.dialogHandler = dialogHandler;
		this.ImageCache = imageCache;
		this.database = database;
		
		logger.LogInformation("[HomeViewModel-.ctor] HomeViewModel has been initialized.");
	}


	[ObservableProperty]
	IReadOnlyList<Movie>? movies = null;

	[ObservableProperty]
	IReadOnlyList<Show>? shows = null;

	
	[RelayCommand]
	async Task ReloadMoviesAsync()
	{
		logger.LogInformation("[HomeViewModel-ReloadMoviesAsync] Reloading movies from database...");
		Movie[] refreshedMovies = await database.GetAsync<Movie>();

		if (Movies is null || refreshedMovies.Length != Movies.Count)
		{
			Movies = refreshedMovies;
			return;
		}

		for (int i = 0; i < refreshedMovies.Length; i++)
			if (refreshedMovies[i].Id != Movies[i].Id)
			{
				Movies = refreshedMovies;
				return;
			}
	}
	
	[RelayCommand]
	async Task ReloadShowsAsync()
	{
		logger.LogInformation("[HomeViewModel-ReloadShowsAsync] Reloading shows from database...");
		Show[] refreshedShows = await database.GetAsync<Show>();

		if (Shows is null || refreshedShows.Length != Shows.Count)
		{
			Shows = refreshedShows;
			return;
		}

		for (int i = 0; i < refreshedShows.Length; i++)
			if (refreshedShows[i].Id != Shows[i].Id)
			{
				Shows = refreshedShows;
				return;
			}
	}
	
	
	[RelayCommand]
	async Task RemoveMovieAsync(
		Movie movie)
	{
		if (!await dialogHandler.ShowQuestionAsync("alert_question".L10N(), "warning_remove_movie".L10N(), "alert_confirm".L10N(), "alert_cancel".L10N()))
		{
			logger.LogInformation("[HomeViewModel-RemoveMovieAsync] Removing movie from database cancelled.");
			return;
		}
		
		logger.LogInformation("[HomeViewModel-RemoveMovieAsync] Removing movie from database...");
		await database.DeleteAsync<Movie>(movie.Id);
		
		await ReloadMoviesAsync();
	}
	
	[RelayCommand]
	async Task RemoveShowAsync(
		Show show)
	{
		if (!await dialogHandler.ShowQuestionAsync("alert_question".L10N(), "warning_remove_show".L10N(), "alert_confirm".L10N(), "alert_cancel".L10N()))
		{
			logger.LogInformation("[HomeViewModel-RemoveShowAsync] Removing show from database cancelled.");
			return;
		}
		
		logger.LogInformation("[HomeViewModel-RemoveShowAsync] Removing show from database...");
		await database.DeleteAsync<Show>(show.Id);
		
		await ReloadShowsAsync();
	}


	[RelayCommand]
	void ShowMediaSection(
		string name)
	{
		void PushMediaSection<TMediaContainer>(
			string sectionName) where TMediaContainer : IMediaContainer, new()
		{
			ILogger<MediaSectionViewModel<TMediaContainer>> viewModelLogger = App.Provider.GetRequiredService<ILogger<MediaSectionViewModel<TMediaContainer>>>();
			MediaSectionViewModel<TMediaContainer> viewModel = new(viewModelLogger, Config, database, ImageCache, navigation, dialogHandler, sectionName);
			navigation.Push(viewModel);
		}
		
		switch (name)
		{
			case nameof(Movies):
				PushMediaSection<Movie>("media_movies".L10N());
				break;
			case nameof(Shows):
				PushMediaSection<Show>("media_shows".L10N());
				break;
			default:
				throw new("This type of media container is not supported.");
		}
	}
}