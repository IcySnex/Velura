using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Velura.iOS.Binding.Abstract;
using Velura.iOS.Binding.Mappers;
using Velura.iOS.Helpers;
using Velura.iOS.Services;
using Velura.iOS.Views;
using Velura.Services;
using Velura.Services.Abstract;

namespace Velura.iOS;

public sealed class IOSApp : App
{
	public static UIWindow MainWindow { get; } = new(UIScreen.MainScreen.Bounds);
	public static bool IsIPad { get; } = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;

	public static IReadOnlyList<PropertyBindingMapper> PropertyBindingMappers { get; private set; } = default!;
	public static MainViewController MainViewController { get; private set; } = default!;
	public static Images Images { get; private set; } = default!;
	
	
	readonly PathResolver pathResolver = new();
	

	protected override LoggerConfiguration ConfigureLogging(
		LoggerConfiguration configuration,
		string preferredTemplate) =>
		base.ConfigureLogging(configuration, preferredTemplate)
			.WriteTo.NSLog(
				outputTemplate: preferredTemplate)
			.WriteTo.File(
				path: pathResolver.CurrentLogFile,
				rollingInterval: RollingInterval.Day,
				retainedFileCountLimit: 10,
				outputTemplate: preferredTemplate);

	
	protected override void RegisterPlatformServices(
		IServiceCollection services)
	{
		services.AddSingleton<MainViewController>();
		
		services.AddSingleton<IPathResolver>(pathResolver);
		services.AddSingleton<INavigation, Navigation>();
		services.AddSingleton<ISimpleStorage, NSUserDefaultsStorage>();
		services.AddSingleton<IThemeManager, ThemeManager>();
		services.AddSingleton<ISystemInfo, SystemInfo>();
		services.AddSingleton<IDialogHandler, DialogHandler>();
		services.AddSingleton<ILauncher, Launcher>();
	}


	protected override void FinishAppInitialization()
	{
		base.FinishAppInitialization();
		
		PropertyBindingMappers =
		[
			new UISwitchOnMapper(),
			new UITextFieldTextMapper(),
			new UINumberFieldNumberMapper(),
			new UISelectionButtonSelectedItemMapper()
		];
		MainViewController = Provider.GetRequiredService<MainViewController>();
		Images = new(Provider.GetRequiredService<ImageCache>());
	}
}