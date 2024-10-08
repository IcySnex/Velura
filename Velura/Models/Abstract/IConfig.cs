using System.ComponentModel;
using System.Reflection;
using Velura.Models.Attributes;

namespace Velura.Models.Abstract;

public interface IConfig : INotifyPropertyChanged
{
	public static T GetDefaultValue<T>(
		string propertyName)
	{
		PropertyInfo? info = typeof(IConfig).GetProperty(propertyName);
		
		ConfigItemAttribute<T> attribute = info?.GetCustomAttribute<ConfigItemAttribute<T>>() ?? throw new NullReferenceException("The property doesn't have a ConfigItemAttribute");
		return attribute.DefaultValue;
	}
	
	
	void Remove(string propertyName);
	
	
	[ConfigItem<string>("Some Key", "This is some random ahh example key", "123")]
	string SomeKey { get; set; }

	[ConfigItem<bool>("Some Switch", "This is some random ahh toggle woggle switchyy", true)]
	bool SomeSwitch { get; set; }
}