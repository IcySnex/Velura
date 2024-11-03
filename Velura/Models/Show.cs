using SQLite;
using Velura.Models.Abstract;

namespace Velura.Models;

public class Show
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; init; }

	public string? PosterPath { get; init; } = null;

	[NotNull, Indexed]
	public string Title { get; init; } = default!;

	public string? Description { get; init; } = null;
	
	public string? Genre { get; init; } = null;

	public DateTime? ReleaseDate { get; init; } = null;
}