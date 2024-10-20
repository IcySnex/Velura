using Microsoft.Extensions.Logging;
using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public class NSUserDefaultsStorage : ISimpleStorage
{
	readonly ILogger<NSUserDefaultsStorage> logger;
	
	readonly NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

	public NSUserDefaultsStorage(
		ILogger<NSUserDefaultsStorage> logger)
	{
		this.logger = logger;
		
<<<<<<< HEAD
		logger.LogInformation("[NSUserDefaultsStorage-.ctor] NSUserDefaultsStorage has been initialized.");
=======
		logger.LogInformation("[NSUserDefaultsStorage-.ctor] UserDefaultsStorage has been initialized.");
>>>>>>> db4a7f244f0f55aadc41c2ebbc6a519c78d776ce
	}


	public T? GetValue<T>(
		string key)
	{
		logger.LogInformation("[NSUserDefaultsStorage-GetValue] Getting value with key '{key}'...", key);
		NSObject value = userDefaults.ValueForKey(new(key));
		
		if (value is null)
			return default;

		return value switch
		{
			NSString s => (T)(object)s.ToString(),
			NSNumber n when typeof(T) == typeof(bool) => (T)(object)n.BoolValue,
			NSNumber n when typeof(T) == typeof(int) => (T)(object)n.Int32Value,
			NSNumber n when typeof(T) == typeof(float) => (T)(object)n.FloatValue,
			NSNumber n when typeof(T) == typeof(double) => (T)(object)n.DoubleValue,
			_ => default
		};
	}

	public void SetValue<T>(
		string key,
		T value)
	{
		logger.LogInformation("[NSUserDefaultsStorage-SetValue] Setting value with key '{key}'...", key);
		userDefaults.SetValueForKey(NSObject.FromObject(value), new(key));
	}

	public void RemoveValue(
		string key)
	{
		logger.LogInformation("[NSUserDefaultsStorage-RemoveValue] Removing value with key '{key}'...", key);
		userDefaults.RemoveObject(key);
	}
}