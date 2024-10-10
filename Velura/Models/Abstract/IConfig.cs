using System.ComponentModel;
using System.Reflection;

namespace Velura.Models.Abstract;

public interface IConfig : INotifyPropertyChanged
{
	public static readonly Dictionary<string, Dictionary<string, ConfigItem>> Groups = GetItems();

	public static readonly Dictionary<string, ConfigItem> Items = Groups.SelectMany(group => group.Value).ToDictionary(group => group.Key, group => group.Value);

	static Dictionary<string, Dictionary<string, ConfigItem>> GetItems()
	{
		PropertyInfo[] properties = typeof(IConfig).GetProperties();
		Dictionary<string, Dictionary<string, ConfigItem>> items = new();
		
		foreach (PropertyInfo property in properties)
		{
			ConfigItem item = property.GetCustomAttribute<ConfigItem>() ?? throw new NullReferenceException($"The property '{property.Name} is not a ConfigItem.");
			
			if (!items.ContainsKey(item.Group))
				items.Add(item.Group, new());
			items[item.Group][item.Name] = item;
		}

		return items;
	}

	
	void Reset(string propertyName);
	
	void Reset();
	
	
	[ConfigItem("Some Key", "This is some random ahh example key", "General", "123")]
	string SomeKey { get; set; }

	[ConfigItem("Some Switch", "This is some random ahh toggle woggle switchyy", "Advanced", true)]
	bool SomeSwitch { get; set; }
}