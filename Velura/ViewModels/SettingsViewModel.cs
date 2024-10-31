using System.ComponentModel;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Models.Attributes;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
	readonly ILogger<SettingsViewModel> logger;
	readonly INavigation navigation;
	readonly IThemeManager themeManager;
	
	public Config Config { get; }
	
	public SettingsViewModel(
		ILogger<SettingsViewModel> logger,
		Config config,
		INavigation navigation,
		IThemeManager themeManager)
	{
		this.logger = logger;
		this.Config = config;
		this.navigation = navigation;
		this.themeManager = themeManager;
		
		Config.Appearance.PropertyChanged += OnConfigAppearancePropertyChanged;
		themeManager.Set(Config.Appearance.Theme);
		
		Group = CreateSettingsGroup(typeof(Config), "Config");

		logger.LogInformation("[SettingsViewModel-.ctor] SettingsViewModel has been initialized.");
	}

	
	void OnConfigAppearancePropertyChanged(
		object? _,
		PropertyChangedEventArgs e)
	{
		if (e.PropertyName != nameof(ConfigAppearance.Theme))
			return;
		
		themeManager.Set(Config.Appearance.Theme);
	}


	public readonly SettingsGroup Group;

	SettingsGroup CreateSettingsGroup(
		Type configGroup,
		string path)
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
		
		List<SettingsGroup> subGroups = [];
		List<SettingsProperty> properties = [];
		foreach (PropertyInfo propertyInfo in configGroup.GetProperties())
		{
			if (typeof(ConfigGroup).IsAssignableFrom(propertyInfo.PropertyType))
			{
				subGroups.Add(CreateSettingsGroup(propertyInfo.PropertyType, propertyInfo.Name));
				continue;
			}
			
			DetailsAttribute? propertyDetails = propertyInfo.GetCustomAttribute<DetailsAttribute>();
			if (propertyDetails is null)
			{
				logger.LogWarning("[SettingsViewModel-CreateSettingsGroup] The config group property '{propertyName}' doesn't have a details attribute.", propertyInfo.Name);
				continue;
			}

			properties.Add(new(propertyDetails, propertyInfo.Name, propertyInfo.PropertyType));
		}
		
		return new(details, image, subGroups, properties, path);
	}


	[RelayCommand]
	void ShowAboutInfo() =>
		navigation.Present<AboutViewModel>();
}