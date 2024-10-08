using System.ComponentModel;
using System.Globalization;
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

	
	public void Remove(
		string propertyName) => 
		userDefaults.RemoveObject(propertyName);
	
	
	T GetOrDefault<T>(
		string key)
	{
		NSObject value = userDefaults.ValueForKey(new(key));
		if (value is null)
			return IConfig.GetDefaultValue<T>(key);

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
		string key,
		T value)
	{
		userDefaults.SetValueForKey(NSObject.FromObject(value), new(key));
		OnPropertyChanged(key);
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