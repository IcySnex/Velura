namespace Velura.Models.Abstract;

public interface IMediaContainer
{
	public int Id { get; }
	
	public string Title { get; }
	
	public string? Description { get; }
	
	public string? PosterPath { get; }

	DateTime? ReleaseDate { get; }

	string? Genre { get; }
}