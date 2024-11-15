using SQLite;
using Velura.Models.Abstract;

namespace Velura.Models;

public class Show : IMediaContainer
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; init; }

	[NotNull, Indexed]
	public string Title { get; init; } = default!;

	public string? Description { get; init; } = null;
	
	public string? PosterUrl { get; init; } = null;

	public DateTime? ReleaseDate { get; init; } = null;
}