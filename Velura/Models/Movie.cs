using SQLite;
using Velura.Models.Abstract;

namespace Velura.Models;

public class Movie : IMedia, IMediaContainer
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; init; }

	[NotNull]
	public string FilePath { get; init; } = default!;

	[NotNull, Indexed]
	public string Title { get; init; } = default!;

	public string? Description { get; init; } = null;

	public string? PosterUrl { get; init; } = null;
	
	public string? BackdropUrl { get; init; } = null;

	public DateTime? ReleaseDate { get; init; } = null;
	
	public TimeSpan Duration { get; init; } = TimeSpan.Zero;
}