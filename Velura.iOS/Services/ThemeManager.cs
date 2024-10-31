using Microsoft.Extensions.Logging;
using Velura.Enums;
using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public class ThemeManager : IThemeManager
{
	readonly ILogger<ThemeManager> logger;
	
	public ThemeManager(
		ILogger<ThemeManager> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[ThemeManager-.ctor] ThemeManager has been initialized.");
	}
	
	
	public void SetMode(
		ThemeMode mode)
	{
		logger.LogInformation("[ThemeManager-Set] Setting theme to '{mode}'...", mode);
		IOSApp.MainWindow.OverrideUserInterfaceStyle = mode switch
		{
			ThemeMode.Light => UIUserInterfaceStyle.Light,
			ThemeMode.Dark => UIUserInterfaceStyle.Dark,
			_ => UIUserInterfaceStyle.Unspecified
		};
	}
	

	public void SetPreferLargeTitles(
		bool enable)
	{
		foreach (UIViewController viewController in IOSApp.MainViewController.ViewControllers!)
		{
			if (viewController is not UINavigationController navigationController)
				return;
			
			navigationController.NavigationBar.PrefersLargeTitles = enable;
		}
	}
}