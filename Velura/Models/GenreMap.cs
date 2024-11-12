using SQLite;

namespace Velura.Models;

public sealed class GenreMap
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; init; }

	[NotNull]
	public int GenreId { get; init; } = default!;
	
	[NotNull]
	public int MediaContainerId { get; init; } = default!;
}