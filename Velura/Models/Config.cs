using Velura.Enums;
using Velura.Models.Abstract;
using Velura.Models.Attributes;
using Velura.Services.Abstract;

namespace Velura.Models;

[L10NDetails("config", "config_app_description")]
[Image("gear", "#8e8e8e", "#ffffff")]
public sealed class Config(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Config")
{
	public ConfigGeneral General { get; } = new(simpleStorage);
	
	public ConfigAppearance Appearance { get; } = new(simpleStorage);
	
	public ConfigAdvanced Advanced { get; } = new(simpleStorage);
}


[L10NDetails("config_general", "config_general_description")]
[Image("house", "#0b84ff", "#ffffff")]
public sealed class ConfigGeneral(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Config.General")
{ }

[L10NDetails("config_appearance", "config_appearance_description")]
[Image("paintpalette.fill", "#ff9501", "#ffffff")]
public sealed class ConfigAppearance(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Config.Appearance")
{
	[L10NDetails("config_appearance_theme", "config_appearance_theme_description")]
	public ThemeMode Theme
	{
		get => GetValue(nameof(ThemeMode), ThemeMode.Auto);
		set => SetValue(nameof(ThemeMode), value);
	}
}

[L10NDetails("config_advanced", "config_advanced_description")]
[Image("exclamationmark.triangle.fill", "#ff6479", "#ffffff")]
public sealed class ConfigAdvanced(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Config.Advanced")
{ }