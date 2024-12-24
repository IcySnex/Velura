using System.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Velura.Models;
using Velura.Services;
using Velura.ViewModels;

namespace Velura;

public abstract class App
{
	public static IServiceProvider Provider { get; private set; } = default!;
	
	public static ResourceManager Localization { get; private set; } = default!;

	protected App()
	{
		IServiceCollection services = new ServiceCollection();
		
		services.AddLogging(builder =>
		{
			Log.Logger = ConfigureLogging(new(), "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:l}{NewLine:l}{Exception:l}").CreateLogger();
			
			builder.SetMinimumLevel(LogLevel.Information);
		
			builder.ClearProviders();
			builder.AddSerilog();
		});
		
		Localization = new("Velura.Resources.Localization", typeof(App).Assembly);
		
		RegisterPlatformServices(services);
		RegisterServices(services);
		RegisterViewModels(services);
		Provider = services.BuildServiceProvider();

		FinishAppInitialization();
		
		ILogger<App> logger = Provider.GetRequiredService<ILogger<App>>();
		logger.LogInformation("[App-.ctor] App has been initialized.");
	}


	protected virtual LoggerConfiguration ConfigureLogging(
		LoggerConfiguration configuration,
		string preferredTemplate) =>
		configuration
			.MinimumLevel.Information()
			.WriteTo.Debug(
				outputTemplate: preferredTemplate);
	
	
	protected virtual void RegisterPlatformServices(
		IServiceCollection services)
	{ }

	static void RegisterServices(
		IServiceCollection services)
	{
		services.AddSingleton<Config>();
		services.AddSingleton<HttpClient>();
		services.AddSingleton<ImageCache>();
		services.AddSingleton<Database>();
		services.AddSingleton<MediaLibrary>();
		services.AddSingleton<MediaInfoProvider>();
	}

	static void RegisterViewModels(
		IServiceCollection services)
	{
		services.AddSingleton<HomeViewModel>();
		services.AddSingleton<SearchViewModel>();
		services.AddSingleton<SettingsViewModel>();
		
		services.AddSingleton<AboutViewModel>();
	}


	protected virtual async void FinishAppInitialization()
	{
		MediaLibrary library = Provider.GetRequiredService<MediaLibrary>();
		await library.LoadMoviesAsync();
		await library.LoadShowsAsync();
	}
}