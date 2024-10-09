namespace Velura.Services.Abstract;

public interface IConverter
{
	public string ToString(
		object vlaue);

	
	public T ToObject<T>(
		string value);

	public bool TryToObject<T>(
		string value,
		out T? result);
}