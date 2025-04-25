using System.Globalization;
using DustyPig.REST;
using DustyPig.TMDB.Models.Common;
using DustyPig.TMDB.Models.Genres;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Client = DustyPig.TMDB.Client;

namespace Velura.Services;

public sealed class MediaInfoProvider
{
	public static string? GetImageUrl(
		string? path,
		string size) =>
		path is null ? null : $"https://image.tmdb.org/t/p/{size}{path}";
	
	
	readonly ILogger<MediaInfoProvider> logger;

	readonly Client client = new(Client.AuthTypes.BearerToken, SecretKeys.TMDBAuthKey);

	public MediaInfoProvider(
		ILogger<MediaInfoProvider> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[MediaInfoProvider-.ctor] MediaInfoProvider has been initialized.");
	}

	
	readonly Dictionary<int, string> genreCache = new();

	public async Task<string> GetGenreNameAsync(
		int id,
		CancellationToken cancellationToken = default)
	{
		if (genreCache.TryGetValue(id, out string? value))
			return value;
		
		logger.LogInformation("[MediaInfoProvider-GetGenreNameAsync] Getting genre name by id: {id}...", id);

		Response<GenreList> movieGenres = await client.Endpoints.Genres.GetMoviesAsync(CultureInfo.CurrentCulture.Name, cancellationToken);
		movieGenres.ThrowIfError();
		foreach (CommonName genre in movieGenres.Data.Genres)
			genreCache[genre.Id] = genre.Name;
		
		Response<GenreList> tvGenres = await client.Endpoints.Genres.GetTvSeriesAsync(CultureInfo.CurrentCulture.Name, cancellationToken);
		tvGenres.ThrowIfError();
		foreach (CommonName genre in tvGenres.Data.Genres)
			genreCache[genre.Id] = genre.Name;

		return genreCache.GetValueOrDefault(id, "Unknown");
	}
	
	
	public async Task<CommonMovie> SearchMovieAsync(
		string query,
		CancellationToken cancellationToken = default)
	{
		logger.LogInformation("[MediaInfoProvider-SearchMovieAsync] Searching for movies: {query}...", query);
		
		Response<PagedResult<CommonMovie>> results = await client.Endpoints.Search.MoviesAsync(
			query,
			language: CultureInfo.CurrentCulture.Name,
			cancellationToken: cancellationToken);
		
		results.ThrowIfError();
		results.Data.Results.ThrowIfEmpty($"No movies found for query: {query}.", logger, "MediaInfoProvider-SearchMovieAsync");
		
		return results.Data.Results[0];
	}
}