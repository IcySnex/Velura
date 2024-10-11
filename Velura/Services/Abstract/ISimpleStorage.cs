namespace Velura.Services.Abstract;

public interface ISimpleStorage
{
	T? GetValue<T>(
		string key);
	
	void SetValue<T>(
		string key,
		T value);
	
	void RemoveValue(
		string key);
}