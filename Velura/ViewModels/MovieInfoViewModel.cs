using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Models;
using Velura.Services;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class MovieInfoViewModel : ObservableObject
{
	readonly ILogger<MovieInfoViewModel> logger;
	readonly MediaLibrary mediaLibrary;
	readonly MediaInfoProvider mediaInfoProvider;
	readonly INavigation navigation;
	
	public MovieInfoViewModel(
		ILogger<MovieInfoViewModel> logger,
		MediaLibrary mediaLibrary,
		MediaInfoProvider mediaInfoProvider,
		INavigation navigation)
	{
		this.logger = logger;
		this.mediaLibrary = mediaLibrary;
		this.mediaInfoProvider = mediaInfoProvider;
		this.navigation = navigation;

		logger.LogInformation("[MovieInfoViewModel-.ctor] MovieInfoViewModel has been initialized.");
	}


	public Movie Movie { get; private set; } = default!;
	public IReadOnlyList<string> Genres { get; private set; } = default!;

	public async Task InitializeAsync(
		Movie movie)
	{
		logger.LogInformation("[MovieInfoViewModel-InitializeAsync] Initializing viewmodel...");
		
		List<string> result = [];
		foreach (int genreId in await mediaLibrary.GetGenresAsync(movie.Id))
		{
			string genreName = await mediaInfoProvider.GetGenreNameAsync(genreId);
			result.Add(genreName);
		}
		
		Genres = result;
		Movie = movie;
	}


	[RelayCommand]
	void Close() =>
		navigation.Dismiss();
}