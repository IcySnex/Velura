using System.Globalization;

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
}