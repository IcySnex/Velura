using System.Globalization;
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
}