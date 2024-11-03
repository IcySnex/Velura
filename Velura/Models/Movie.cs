using SQLite;
using Velura.Models.Abstract;

namespace Velura.Models;

public class Movie : IMedia
{
	// Media
	[PrimaryKey, AutoIncrement]
	public int Id { get; init; }

	[NotNull]
	public string FilePath { get; init; } = default!;

	public string? PosterPath { get; init; } = null;

	[NotNull, Indexed]
	public string Title { get; init; } = default!;

	public string? Description { get; init; } = null;
	
	public TimeSpan Duration { get; init; } = TimeSpan.Zero;

	// Movie
	public string? Genre { get; init; } = null;

	public DateTime? ReleaseDate { get; init; } = null;
}