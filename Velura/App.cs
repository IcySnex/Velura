using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Velura.Models;
using Velura.ViewModels;

namespace Velura;

public abstract class App
{
	public static IServiceProvider Provider { get; private set; } = default!;

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
		RegisterPlatformServices(services);
		RegisterServices(services);
		RegisterViewModels(services);
		
		Provider = services.BuildServiceProvider();

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
	}

	static void RegisterViewModels(
		IServiceCollection services)
	{
		services.AddSingleton<HomeViewModel>();
		services.AddSingleton<SearchViewModel>();
		services.AddSingleton<SettingsViewModel>();
	}
}