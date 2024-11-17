using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public class NSUserDefaultsStorage : ISimpleStorage
{
	readonly ILogger<NSUserDefaultsStorage> logger;
	
	readonly NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;
	readonly ConcurrentDictionary<string, object?> cache = new();

	public NSUserDefaultsStorage(
		ILogger<NSUserDefaultsStorage> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[NSUserDefaultsStorage-.ctor] NSUserDefaultsStorage has been initialized.");
	}


	public T GetValue<T>(
		string key,
		T defaultValue = default!)
	{
		if (cache.TryGetValue(key, out object? cachedValue))
			return cachedValue is null ? defaultValue : (T)cachedValue;
		
		logger.LogInformation("[NSUserDefaultsStorage-GetValue] Getting value with key '{key}'...", key);
		NSObject value = userDefaults.ValueForKey(new(key));

		if (value is null)
		{
			cache[key] = null;
			return defaultValue;
		}

		T result = value switch
		{
			NSString s => (T)(object)s.ToString(),
			NSNumber n when typeof(T).IsEnum => (T)Enum.ToObject(typeof(T), n.Int32Value),
			NSNumber n when typeof(T) == typeof(bool) => (T)(object)n.BoolValue,
			NSNumber n when typeof(T) == typeof(int) => (T)(object)n.Int32Value,
			NSNumber n when typeof(T) == typeof(float) => (T)(object)n.FloatValue,
			NSNumber n when typeof(T) == typeof(double) => (T)(object)n.DoubleValue,
			_ => defaultValue
		};
		
		cache[key] = result!;
		return result;
	}

	public void SetValue<T>(
		string key,
		T value)
	{
		logger.LogInformation("[NSUserDefaultsStorage-SetValue] Setting value with key '{key}'...", key);
		userDefaults.SetValueForKey(NSObject.FromObject(value), new(key));
		
		cache[key] = value!;
	}

	public void RemoveValue(
		string key)
	{
		logger.LogInformation("[NSUserDefaultsStorage-RemoveValue] Removing value with key '{key}'...", key);
		userDefaults.RemoveObject(key);
		
		cache.TryRemove(key, out _);
	}
}