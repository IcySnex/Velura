using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Velura.Helpers;

public static class Extensions
{
	public static string L10N(
		this string key,
		params object[] formatArgs)
	{
		string value = App.Localization.GetString(key, CultureInfo.CurrentCulture) ?? throw new KeyNotFoundException($"Could not find localized string with key '{key}'.");
		return string.Format(value, formatArgs);
	}
	
	public static string L10N(
		this TimeSpan timespan,
		bool includeSeconds = false)
	{
		StringBuilder builder = new();
		
		int hours = (int)timespan.TotalHours;
		int minutes = timespan.Minutes;
		int seconds = includeSeconds ? timespan.Seconds : 0;

		if (hours > 0)
		{
			if (builder.Length != 0)
				builder.Append(' ');
			builder.Append((hours == 1 ? "time_short_hour" : "time_short_hour_plural").L10N(hours));
		}
		if (minutes > 0)
		{
			if (builder.Length != 0)
				builder.Append(' ');
			builder.Append((minutes == 1 ? "time_short_minute" : "time_short_minute_plural").L10N(minutes));
		}
		if (seconds > 0)
		{
			if (builder.Length != 0)
				builder.Append(' ');
			builder.Append((seconds == 1 ? "time_short_second" : "time_short_second_plural").L10N(seconds));
		}
		
		return builder.Length == 0 ? "not_available".L10N() : builder.ToString();
	}


	public static void ThrowIfNull(
		this object? obj,
		string message,
		ILogger? logger = null,
		string caller = "[-]")
	{
		if (obj is not null)
			return;
		
		NotSupportedException ex = new(message);
		logger?.LogError(ex, "[{caller}] {message}.", caller, message);
		throw ex;
	}

	public static void ThrowIfEmpty<T>(
		this ICollection<T> collection,
		string message,
		ILogger? logger = null,
		string caller = "[-]")
	{
		if (collection.Count > 0)
			return;
		
		NotSupportedException ex = new(message);
		logger?.LogError(ex, "[{caller}] {message}.", caller, message);
		throw ex;
	}
}