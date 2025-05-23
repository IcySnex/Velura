namespace Velura.Models.Abstract;

public interface IMedia
{
	public int Id { get; }
	
	public string FilePath { get; }
	
	public string Title { get; }
	
	public string? Description { get; }

	DateTime? ReleaseDate { get; }

	public TimeSpan Duration { get; }
}