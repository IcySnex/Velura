namespace Velura.Services.Abstract;

public interface ISimpleStorage
{
	T GetValue<T>(
		string key,
		T defaultValue = default!);
	
	void SetValue<T>(
		string key,
		T value);
	
	void RemoveValue(
		string key);
}