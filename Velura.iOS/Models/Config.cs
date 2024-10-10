using System.ComponentModel;
using System.Runtime.CompilerServices;
using Velura.Models.Abstract;

namespace Velura.iOS.Models;

public class Config : IConfig
{
	readonly NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

	
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(
		[CallerMemberName] string? propertyName = null) =>
		PropertyChanged?.Invoke(this, new(propertyName));


	public void Reset(
		string propertyName)
	{
		userDefaults.RemoveObject(propertyName);
		OnPropertyChanged(propertyName);
	}

	public void Reset()
	{
		NSUserDefaults.ResetStandardUserDefaults();
		foreach (string propertyName in IConfig.Items.Keys)
			OnPropertyChanged(propertyName);
	}
	
	
	T GetOrDefault<T>(
		string propertyName)
	{
		NSObject value = userDefaults.ValueForKey(new(propertyName));
		if (value is null)
			return (T)IConfig.Items[propertyName].DefaultValue;

		Type type = typeof(T);
		if (type == typeof(string))
			return (T)(object)((NSString)value).ToString();
		if (type == typeof(bool))
			return (T)(object)((NSNumber)value).BoolValue;
		if (type == typeof(int))
			return (T)(object)((NSNumber)value).Int32Value;
		if (type == typeof(float))
			return (T)(object)((NSNumber)value).FloatValue;
		if (type == typeof(double))
			return (T)(object)((NSNumber)value).DoubleValue;
		
		throw new InvalidOperationException($"Unsupported type: {typeof(T)}");
	}

	void Set<T>(
		string propertyName,
		T value)
	{
		userDefaults.SetValueForKey(NSObject.FromObject(value), new(propertyName));
		OnPropertyChanged(propertyName);
	}
	
	
	public string SomeKey
	{
		get => GetOrDefault<string>(nameof(SomeKey));
		set => Set(nameof(SomeKey), value);
	}

	public bool SomeSwitch
	{
		get => GetOrDefault<bool>(nameof(SomeSwitch));
		set => Set(nameof(SomeSwitch), value);
	}
}