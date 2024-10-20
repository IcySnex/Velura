using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Models.Attributes;

namespace Velura.ViewModels;

public sealed class SettingsViewModel : ObservableObject
{
	readonly ILogger<SettingsViewModel> logger;

	public Config Config { get; }
	
	public SettingsViewModel(
		ILogger<SettingsViewModel> logger,
		Config config)
	{
		this.logger = logger;
		this.Config = config;
		
		Group = CreateSettingsGroup(typeof(Config));

		logger.LogInformation("[SettingsViewModel-.ctor] SettingsViewModel has been initialized.");
	}
	
	public readonly SettingsGroup Group;

	SettingsGroup CreateSettingsGroup(
		Type configGroup,
		string? relativePath = null)
	{
		logger.LogInformation("[SettingsViewModel-CreateSettingsGroup] Creating settings group from config group...");

		DetailsAttribute? details = configGroup.GetCustomAttribute<DetailsAttribute>();
		ImageAttribute? image = configGroup.GetCustomAttribute<ImageAttribute>();
		if (details is null || image is null)
		{
			Exception ex = new NullReferenceException("The config group doesn't have a details or image attribute.");
			logger.LogError("[SettingsViewModel-CreateSettingsGroup] Failed to create settings group from config group.");
			throw ex;
		}
		
		List<SettingsGroup> subGroupes = [];
		List<SettingsProperty> properties = [];
		foreach (PropertyInfo propertyInfo in configGroup.GetProperties())
		{
			string propertyRelativePath = relativePath is null ? propertyInfo.Name : $"{relativePath}.{propertyInfo.Name}";
			
			if (typeof(ConfigGroup).IsAssignableFrom(propertyInfo.PropertyType))
			{
				subGroupes.Add(CreateSettingsGroup(propertyInfo.PropertyType, propertyRelativePath));
				continue;
			}
			
			DetailsAttribute? propertyDetails = propertyInfo.GetCustomAttribute<DetailsAttribute>();
			if (propertyDetails is null)
			{
				logger.LogWarning("[SettingsViewModel-CreateSettingsGroup] The config group property '{propertyName}' doesn't have a details attribute.", propertyInfo.Name);
				continue;
			}

			properties.Add(new(propertyDetails, propertyRelativePath, propertyInfo.PropertyType));
		}
		
		return new(details, image, subGroupes, properties);
	}
}