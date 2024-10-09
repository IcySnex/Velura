using System.ComponentModel;
using System.Reflection;
using Velura.Models.Attributes;

namespace Velura.Models.Abstract;

public interface IConfig : INotifyPropertyChanged
{
	public static readonly Dictionary<string, object> DefaultItems = typeof(IConfig).GetProperties().ToDictionary(
		property => property.Name,
		property => property.GetCustomAttribute<ConfigItemAttribute>()?.DefaultValue ?? throw new NullReferenceException($"The property '{property.Name} is not a ConfigItem."));


	void Reset(string propertyName);
	
	void Reset();
	
	
	[ConfigItem("Some Key", "General", "This is some random ahh example key", "123")]
	string SomeKey { get; set; }

	[ConfigItem("Some Switch", "Advanced", "This is some random ahh toggle woggle switchyy", true)]
	bool SomeSwitch { get; set; }
}