namespace Velura.Models.Abstract;

public interface IMedia
{
	public int Id { get; }
	
	public string FilePath { get; }
	
	public string? PosterPath { get; }
	
	public string Title { get; }
	
	public string? Description { get; }
	
	public TimeSpan Duration { get; }
}