using SQLite;
using Velura.Models.Abstract;

namespace Velura.Models;

public class Episode : IMedia
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

	// Episode
	public int ShowId { get; init; } = default!;
	
	public int Season { get; init; } = default!;
	
	public int Number { get; init; } = default!;
}