using Velura.Enums;
using Velura.Models.Abstract;
using Velura.Models.Attributes;
using Velura.Services.Abstract;

namespace Velura.Models;

[Details("App", "Includes all configurable settings of this app.")]
[Image("gear", "#8e8e8e", "#ffffff")]
public sealed class Config(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Config")
{
	public ConfigGeneral General { get; } = new(simpleStorage);
	
	public ConfigAppearance Appearance { get; } = new(simpleStorage);
	
	public ConfigAdvanced Advanced { get; } = new(simpleStorage);
	
	[Details("Debug", "Enables debug options for the app. May decrease performance and introduces new bugs.")]
	public bool IsDebugEnabled
	{
		get => GetValue(nameof(IsDebugEnabled), false);
		set => SetValue(nameof(IsDebugEnabled), value);
	}
}


[Details("General", "This contains general stuff like user or syncing settings.")]
[Image("house", "#0b84ff", "#ffffff")]
public sealed class ConfigGeneral(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "General")
{
	public ConfigUser User { get; } = new(simpleStorage);

	[Details("Sync", "U wanna sync everything?")]
	public bool IsSyncEnabled
	{
		get => GetValue(nameof(IsSyncEnabled), false);
		set => SetValue(nameof(IsSyncEnabled), value);
	}
	
	[Details("Backup", "Automatically backp settings and so lol.")]
	public bool IsBackupEnabled
	{
		get => GetValue(nameof(IsBackupEnabled), false);
		set => SetValue(nameof(IsBackupEnabled), value);
	}
}

[Details("Appearance", "Customize the look and feel of the app.")]
[Image("paintpalette.fill", "#ff9501", "#ffffff")]
public sealed class ConfigAppearance(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Appearance")
{
	[Details("Theme", "Choose the app's theme. Light, Dark, or switch automatically based on your system preferences.")]
	public ThemeMode Theme
	{
		get => GetValue(nameof(ThemeMode), ThemeMode.Auto);
		set => SetValue(nameof(ThemeMode), value);
	}
}

[Details("User", "This contains user specific settings.")]
[Image("person.fill", "#30b556", "#ffffff")]
public sealed class ConfigUser(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "General.User")
{
	[Details("Username", "This is the username LOL.")]
	public string Username
	{
		get => GetValue(nameof(Username), "Bob");
		set => SetValue(nameof(Username), value);
	}
	
	[Details("Age", "How old are youuu?")]
	public int Age
	{
		get => GetValue(nameof(Age), 0);
		set => SetValue(nameof(Age), value);
	}
}

[Details("Advanced", "This contains advanced settings, I wouldnt touch those.")]
[Image("exclamationmark.triangle.fill", "#ff6479", "#ffffff")]
public sealed class ConfigAdvanced(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Advanced")
{
	[Details("Some Key", "This is just some random ahh key.")]
	public string SomeKey
	{
		get => GetValue(nameof(SomeKey), "Default Key Value");
		set => SetValue(nameof(SomeKey), value);
	}
	
	[Details("Some Switch", "Oh shii, this is some random ahh SWITCH.")]
	public bool SomeSwitch
	{
		get => GetValue(nameof(SomeSwitch), true);
		set => SetValue(nameof(SomeSwitch), value);
	}
}