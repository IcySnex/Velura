using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Velura.iOS.Binding.Abstract;
using Velura.iOS.Binding.Mappers;
using Velura.iOS.Services;
using Velura.iOS.Views;
using Velura.Services.Abstract;

namespace Velura.iOS;

public sealed class IOSApp : App
{
	public static IReadOnlyList<BindingMapper> BindingMappers { get; private set; } = default!;
	
	IOSApp() 
	{ }

	public static IOSApp Initialize() =>
		new();


	protected override LoggerConfiguration ConfigureLogging(
		LoggerConfiguration configuration,
		string preferredTemplate) =>
		base.ConfigureLogging(configuration, preferredTemplate)
			.WriteTo.NSLog(
				outputTemplate: preferredTemplate)
			.WriteTo.File(
				path: Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Logs", "Log-.log"),
				rollingInterval: RollingInterval.Day,
				retainedFileCountLimit: 10,
				outputTemplate: preferredTemplate);

	
	protected override void RegisterPlatformServices(
		IServiceCollection services)
	{
		services.AddSingleton<MainViewController>();
		services.AddSingleton<INavigation, Navigation>();
		services.AddSingleton<ISimpleStorage, NSUserDefaultsStorage>();
	}


	protected override void FinishAppInitialization()
	{
		BindingMappers =
		[
			new UISwitchOnMapper(),
			new UITextFieldTextMapper(),
			new UIButtonTitleMapper()
		];
	}
}