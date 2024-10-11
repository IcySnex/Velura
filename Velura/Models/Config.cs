using Velura.Models.Abstract;
using Velura.Services.Abstract;

namespace Velura.Models;

public class Config(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "Config")
{
	[Details("General", "This contains general stuff like user or syncing settings.")]
	[Image("gear", "#0b84ff", "#ffffff")]
	public ConfigGeneral General { get; } = new(simpleStorage);
	
	[Details("User", "This contains user specific settings.")]
	[Image("person.fill", "#30b556", "#ffffff")]
	public ConfigUser User { get; } = new(simpleStorage);

	[Details("Advanced", "This contains advanced settings, I wouldnt touch those.")]
	[Image("exclamationmark.triangle.fill", "#111111", "#ffffff")]
	public ConfigAdvanced Advanced { get; } = new(simpleStorage);
}


public class ConfigGeneral(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "General")
{
	[Details("Sync", "U wanna sync everything?")]
	public bool Sync
	{
		get => GetValue(nameof(Sync), false);
		set => SetValue(nameof(Sync), value);
	}
}

public class ConfigUser(
	ISimpleStorage simpleStorage) : ConfigGroup(simpleStorage, "User")
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