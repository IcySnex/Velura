using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.Models;
using Velura.Services.Abstract;

namespace Velura.Services;

public class MediaLibrary
{
	readonly ILogger<MediaInfoProvider> logger;
	readonly Database database;
	readonly IDialogHandler dialogHandler;
	
	public MediaLibrary(
		ILogger<MediaInfoProvider> logger,
		Database database,
		IDialogHandler dialogHandler)
	{
		this.logger = logger;
		this.database = database;
		this.dialogHandler = dialogHandler;
		
		logger.LogInformation("[MediaLibrary-.ctor] MediaLibrary has been initialized.");
	}


	public ObservableRangeCollection<Movie> Movies { get; } = [];
	
	public async Task LoadMoviesAsync()
	{
		logger.LogInformation("[MediaLibrary-LoadMoviesAsync] Loading movies from database...");
		try
		{
			Movie[] movies = await database.GetAsync<Movie>();

			Movies.Clear();
			Movies.AddRange(movies);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "[MediaLibrary-LoadMoviesAsync] Failed to load movies from database.");
			await dialogHandler.ShowAlertAsync("alert_error".L10N(), "error_failed_loading_movies".L10N(), "alert_confirm".L10N());
		}
	}

	public async Task AddMovieAsync(
		Movie movie)
	{
		logger.LogInformation("[MediaLibrary-AddMovieAsync] Adding movie...");
		try
		{
			if (await database.InsertAsync(movie) < 1)
				throw new("Failed to add movie to database.");

			Movies.Add(movie);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "[MediaLibrary-AddMovieAsync] Failed to add movie.");
			await dialogHandler.ShowAlertAsync("alert_error".L10N(), "error_failed_adding_movie".L10N(), "alert_confirm".L10N());
		}
	}

	public async Task RemoveMovieAsync(
		Movie movie)
	{
		if (!await dialogHandler.ShowQuestionAsync("alert_question".L10N(), "warning_remove_movie".L10N(), "alert_confirm".L10N(), "alert_cancel".L10N()))
		{
			logger.LogInformation("[HomeViewModel-RemoveMovieAsync] Removing movie from database cancelled.");
			return;
		}

		logger.LogInformation("[MediaLibrary-RemoveMovieAsync] Removing movie...");
		try
		{
			if (await database.DeleteAsync<Movie>(movie.Id) < 1)
				throw new("Failed to remove movie from database.");
			
			if (!Movies.Remove(movie))
				throw new("Failed to remove movie from memory.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "[MediaLibrary-RemoveMovieAsync] Failed to remove movie.");
			await dialogHandler.ShowAlertAsync("alert_error".L10N(), "error_failed_removing_movie".L10N(), "alert_confirm".L10N());
		}
	}

	
	public ObservableRangeCollection<Show> Shows { get; } = [];

	public async Task LoadShowsAsync()
	{
		logger.LogInformation("[MediaLibrary-LoadShowsAsync] Loading shows from database...");
		try
		{
			Show[] shows = await database.GetAsync<Show>();

			Shows.Clear();
			Shows.AddRange(shows);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "[MediaLibrary-LoadShowsAsync] Failed to load shows from database.");
			await dialogHandler.ShowAlertAsync("alert_error".L10N(), "error_failed_loading_shows".L10N(), "alert_confirm".L10N());
		}
	}

	public async Task AddShowAsync(
		Show show)
	{
		logger.LogInformation("[MediaLibrary-AddShowAsync] Adding show...");
		try
		{
			if (await database.InsertAsync(show) < 1)
				throw new("Failed to add show to database.");

			Shows.Add(show);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "[MediaLibrary-AddShowAsync] Failed to add show.");
			await dialogHandler.ShowAlertAsync("alert_error".L10N(), "error_failed_adding_show".L10N(), "alert_confirm".L10N());
		}
	}
	
	public async Task RemoveShowAsync(
		Show show)
	{
		if (!await dialogHandler.ShowQuestionAsync("alert_question".L10N(), "warning_remove_show".L10N(), "alert_confirm".L10N(), "alert_cancel".L10N()))
		{
			logger.LogInformation("[HomeViewModel-RemoveShowAsync] Removing show from database cancelled.");
			return;
		}

		logger.LogInformation("[MediaLibrary-RemoveShowAsync] Removing show...");
		try
		{
			if (await database.DeleteAsync<Show>(show.Id) < 1)
				throw new("Failed to remove show from database.");
			
			if (!Shows.Remove(show))
				throw new("Failed to remove show from memory.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "[MediaLibrary-RemoveShowAsync] Failed to remove show.");
			await dialogHandler.ShowAlertAsync("alert_error".L10N(), "error_failed_removing_show".L10N(), "alert_confirm".L10N());
		}
	}
}