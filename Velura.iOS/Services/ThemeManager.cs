using Microsoft.Extensions.Logging;
using Velura.Enums;
using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public class ThemeManager : IThemeManager
{
	readonly ILogger<ThemeManager> logger;
	
	readonly UIWindow window = UIApplication.SharedApplication.Delegate.GetWindow();
	
	public ThemeManager(
		ILogger<ThemeManager> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[ThemeManager-.ctor] ThemeManager has been initialized.");
	}
	
	
	public void Set(
		ThemeMode mode)
	{
		logger.LogInformation("[ThemeManager-Set] Setting theme to '{mode}'...", mode);
		window.OverrideUserInterfaceStyle = mode switch
		{
			ThemeMode.Light => UIUserInterfaceStyle.Light,
			ThemeMode.Dark => UIUserInterfaceStyle.Dark,
			_ => UIUserInterfaceStyle.Unspecified
		};
	}

	public ThemeMode Get()
	{
		logger.LogInformation("[ThemeManager-Get] Getting theme....");
		return window.OverrideUserInterfaceStyle switch
		{
			UIUserInterfaceStyle.Light => ThemeMode.Light,
			UIUserInterfaceStyle.Dark => ThemeMode.Dark,
			_ => ThemeMode.Auto
		};
	}
}