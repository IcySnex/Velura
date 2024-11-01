using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.Models;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class AboutViewModel : ObservableObject
{
	readonly ILogger<AboutViewModel> logger;
	readonly INavigation navigation;

	public AboutViewModel(
		ILogger<AboutViewModel> logger,
		INavigation navigation)
	{
		this.logger = logger;
		this.navigation = navigation;

		Version? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
		Version = assemblyVersion is null ? "app_version".L10N(1, 0, 0) : "app_version".L10N(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build);

		Dependencies =
		[
			new(".NET & C#", "Microsoft", "8.0.0", "https://github.com/dotnet"),
			new(".NET for iOS", "Microsoft", "8.0.0", "https://github.com/xamarin/xamarin-macios"),
			new("CommunityToolkit.Mvvm", "Microsoft", "8.3.2", "https://github.com/CommunityToolkit/dotnet"),
			new("Cirrious.FluentLayout", "Greg Shackles", "3.0.0", "https://github.com/FluentLayout/Cirrious.FluentLayout"),
			new("Serilog", "Serilog Contributors", "4.0.2", "https://github.com/serilog/serilog"),
			new("Serilog.Extensions.Hosting", "Serilog", "8.0.0", "https://github.com/serilog/serilog-extensions-hosting"),
			new("Serilog.Sinks.Xamarin", "Serilog", "1.0.0", "https://github.com/serilog/serilog-sinks-xamarin"),
			new("Serilog.Sinks.File", "Serilog", "6.0.0", "https://github.com/serilog/serilog-sinks-file"),
			new("Serilog.Sinks.Debug", "Serilog", "3.0.0", "https://github.com/serilog/serilog-sinks-debug"),
		];
		
		logger.LogInformation("[HomeViewModel-.ctor] AboutViewModel has been initialized.");
	}
	
	
	public string Version { get; }
	
	public Dependency[] Dependencies { get; }
	
	
	[RelayCommand]
	void CloseAboutInfo() =>
		navigation.Dismiss();
}