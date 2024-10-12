using Velura.Models.Abstract;
using Velura.Models.Attributes;
using Velura.Services.Abstract;

namespace Velura.Models;

[Details("App", "Includes all configurable settings of this app.")]
[Image("gear", "#8e8e8e", "#ffffff")]
public class Config(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Config")
{
	public ConfigGeneral General { get; } = new(simpleStorage);
	
	public ConfigAdvanced Advanced { get; } = new(simpleStorage);
}


[Details("General", "This contains general stuff like user or syncing settings.")]
[Image("house", "#0b84ff", "#ffffff")]
public class ConfigGeneral(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "General")
{
	public ConfigUser User { get; } = new(simpleStorage);

	[Details("Sync", "U wanna sync everything?")]
	public bool Sync
	{
		get => GetValue(nameof(Sync), false);
		set => SetValue(nameof(Sync), value);
	}
}

[Details("User", "This contains user specific settings.")]
[Image("person.fill", "#30b556", "#ffffff")]
public class ConfigUser(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "General.User")
{
	[Details("Username", "This is the username LOL.")]
	public string Userrname
	{
		get => GetValue(nameof(Userrname), "Bob");
		set => SetValue(nameof(Userrname), value);
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
public class ConfigAdvanced(
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