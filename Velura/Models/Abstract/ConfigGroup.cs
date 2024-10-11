using System.ComponentModel;
using Velura.Services.Abstract;

namespace Velura.Models.Abstract;

public abstract class ConfigGroup(
	ISimpleStorage simpleStorage,
	string relativepath) : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected T GetValue<T>(
		string key,
		T defaultValue = default!) =>
		simpleStorage.GetValue<T>($"{relativepath}.{key}") ?? defaultValue;

	protected void SetValue<T>(
		string key,
		T value)
	{
		simpleStorage.SetValue($"{relativepath}.{key}", value);
		PropertyChanged?.Invoke(this, new(key));
	}
}