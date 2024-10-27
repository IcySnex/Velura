using Velura.Models.Attributes;

namespace Velura.Enums;

public enum ThemeMode
{
	[L10NName("enum_theme_auto")]
	Auto,
	
	[L10NName("enum_theme_light")]
	Light,
	
	[L10NName("enum_theme_dark")]
	Dark
}