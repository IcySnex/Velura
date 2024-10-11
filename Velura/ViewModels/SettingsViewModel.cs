using System.Reflection;
using Microsoft.Extensions.Logging;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.ViewModels.Abstract;

namespace Velura.ViewModels;

public sealed class SettingsViewModel : ObservableMvxViewModel
{
	readonly ILogger<SettingsViewModel> logger;
	
	public Config Config { get; }

	public SettingsViewModel(
		ILogger<SettingsViewModel> logger,
		Config config)
	{
		this.logger = logger;
		this.Config = config;

		Groups = GetConfigGroups();
		
		logger.LogInformation("[SettingsViewModel-.ctor] SettingsViewModel has been initialized.");
	}

	public readonly IReadOnlyList<(DetailsAttribute Details, ImageAttribute Image, IReadOnlyList<DetailsAttribute> Properties)> Groups;

	List<(DetailsAttribute Details, ImageAttribute Image, IReadOnlyList<DetailsAttribute> Properties)> GetConfigGroups()
	{
		logger.LogInformation("[SettingsViewModel-GetGroupDetails] Getting config groups...");
		
		List<(DetailsAttribute Details, ImageAttribute Image, IReadOnlyList<DetailsAttribute> Properties)> groups = [];
		foreach (PropertyInfo propertyInfo in typeof(Config).GetProperties())
		{
			if (!typeof(ConfigGroup).IsAssignableFrom(propertyInfo.PropertyType))
				continue;
			
			DetailsAttribute? details = propertyInfo.GetCustomAttribute<DetailsAttribute>();
			ImageAttribute? image = propertyInfo.GetCustomAttribute<ImageAttribute>();
			if (details is null || image is null)
			{
				logger.LogWarning("[SettingsViewModel-GetConfigGroups] The group property '{propertyName}' doesn't have a details or image attribute.", propertyInfo.Name);
				continue;
			}
				
			List<DetailsAttribute> groupProperties = [];
			foreach (PropertyInfo groupProperty in propertyInfo.PropertyType.GetProperties())
			{
				DetailsAttribute? propertyDetails = groupProperty.GetCustomAttribute<DetailsAttribute>();
				if (propertyDetails is null)
				{
					logger.LogWarning("[SettingsViewModel-GetConfigGroups] The property '{propertyName}' doesn't have a details attribute.", groupProperty.Name);
					continue;
				}
					
				groupProperties.Add(propertyDetails);
			}

			groups.Add((details, image, groupProperties));
		}
		
		return groups;
	}
}