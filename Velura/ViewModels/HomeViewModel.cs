using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Models;
using Velura.Services;

namespace Velura.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	readonly ILogger<HomeViewModel> logger;
	readonly Database database;

	public HomeViewModel(
		ILogger<HomeViewModel> logger,
		Database database)
	{
		this.logger = logger;
		this.database = database;
		
		logger.LogInformation("[HomeViewModel-.ctor] HomeViewModel has been initialized.");
	}
	
	
	[RelayCommand]
	async Task InsertMovieAsync()
	{
		Movie movie = new()
		{
			FilePath = "some/path/to/movie.mp4",
			PosterPath = null,
			Title = "Suzume",
			Description = "A modern action adventure road story where a 17-year-old girl named Suzume helps a mysterious young man close doors from the other side that are releasing disasters all over in Japan.",
			Duration = TimeSpan.FromMinutes(122),
			Genre = "Anime",
			ReleaseDate = new(2022, 11, 11),
		};
		await database.InsertAsync(movie);
	}
	
	[RelayCommand]
	async Task GetMoviesAsync()
	{
		Movie[] movies = await database.GetAsync<Movie>();
		foreach (Movie movie in movies)
		{
			logger.LogInformation("Movie Title: {title}", movie.Title);
		}
	}
}