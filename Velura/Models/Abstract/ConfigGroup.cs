using System.ComponentModel;
using Velura.Services.Abstract;

namespace Velura.Models.Abstract;

public abstract class ConfigGroup(
	ISimpleStorage simpleStorage,
	string relativePath) : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected T GetValue<T>(
		string key,
		T defaultValue = default!) =>
		simpleStorage.GetValue<T>($"{relativePath}.{key}") ?? defaultValue;

	protected void SetValue<T>(
		string key,
		T value)
	{
		simpleStorage.SetValue($"{relativePath}.{key}", value);
		PropertyChanged?.Invoke(this, new(key));
	}
}