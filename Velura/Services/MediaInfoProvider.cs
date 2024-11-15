using System.Globalization;
using Microsoft.Extensions.Logging;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using Velura.Helpers;

namespace Velura.Services;

public sealed class MediaInfoProvider
{
	public static string? GetImageUrl(
		string? path,
		string size) =>
		path is null ? null : $"https://image.tmdb.org/t/p/{size}{path}";
	
	
	readonly ILogger<MediaInfoProvider> logger;

	readonly TMDbClient client = new("d8321e5f175262c6a054a024df87c763");

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
		
		foreach (Genre genre in await client.GetMovieGenresAsync(CultureInfo.CurrentCulture.Name, cancellationToken))
			genreCache[genre.Id] = genre.Name;
		foreach (Genre genre in await client.GetTvGenresAsync(CultureInfo.CurrentCulture.Name, cancellationToken))
			genreCache[genre.Id] = genre.Name;

		return genreCache.GetValueOrDefault(id, "Unknown");
	}
	
	
	public async Task<SearchMovie> SearchMovieAsync(
		string query,
		CancellationToken cancellationToken = default)
	{
		logger.LogInformation("[MediaInfoProvider-SearchMovieAsync] Searching for movies: {query}...", query);
		
		SearchContainer<SearchMovie> results = await client.SearchMovieAsync(query, CultureInfo.CurrentCulture.Name, cancellationToken: cancellationToken);
		results.Results.ThrowIfEmpty($"No movies found for query: {query}.", logger, "MediaInfoProvider-SearchMovieAsync");
		
		return results.Results[0];
	}
}