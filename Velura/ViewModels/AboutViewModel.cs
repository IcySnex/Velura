using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Enums;
using Velura.Helpers;
using Velura.Models;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class AboutViewModel : ObservableObject
{
	public const string ContactEmail = "my@email.com";
	
	
	readonly IPathResolver pathResolver;
	readonly INavigation navigation;
	readonly ISystemInfo systemInfo;
	readonly ILauncher launcher;

	public AboutViewModel(
		ILogger<AboutViewModel> logger,
		IPathResolver pathResolver,
		INavigation navigation,
		ISystemInfo systemInfo,
		ILauncher launcher)
	{
		this.pathResolver = pathResolver;
		this.navigation = navigation;
		this.systemInfo = systemInfo;
		this.launcher = launcher;

		Version? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
		Version = assemblyVersion?.ToString(3) ?? "1.0.0";

		Dependencies =
		[
			new(".NET & C#", "Microsoft", "8.0.0", "https://github.com/dotnet"),
			new(".NET for iOS", "Microsoft", "8.0.0", "https://github.com/xamarin/xamarin-macios"),
			new("CommunityToolkit.Mvvm", "Microsoft", "8.3.2", "https://github.com/CommunityToolkit/dotnet"),
			new("Cirrious.FluentLayout", "Greg Shackles", "3.0.0", "https://github.com/FluentLayout/Cirrious.FluentLayout"),
			new("Serilog", "Serilog", "4.0.2", "https://github.com/serilog/serilog"),
			new("Serilog.Extensions.Hosting", "Serilog", "8.0.0", "https://github.com/serilog/serilog-extensions-hosting"),
			new("Serilog.Sinks.Xamarin", "Serilog", "1.0.0", "https://github.com/serilog/serilog-sinks-xamarin"),
			new("Serilog.Sinks.File", "Serilog", "6.0.0", "https://github.com/serilog/serilog-sinks-file"),
			new("Serilog.Sinks.Debug", "Serilog", "3.0.0", "https://github.com/serilog/serilog-sinks-debug"),
		];

		Terms =
		[
			new("about_terms_date".L10N(), FormattedTextType.Note),
			new("about_terms_introduction".L10N(), FormattedTextType.Note),
			
			new("about_terms_age".L10N(), FormattedTextType.Header),
			new("about_terms_age_content".L10N()),
			new("about_terms_permissions".L10N(), FormattedTextType.Header),
			new("about_terms_permissions_content".L10N()),
			
			new("about_terms_usercontent".L10N(), FormattedTextType.Header),
			new("about_terms_usercontent_content".L10N()),
			
			new("about_terms_datastorage".L10N(), FormattedTextType.Header),
			new("about_terms_datastorage_content".L10N()),
			
			new("about_terms_purchases".L10N(), FormattedTextType.Header),
			new("about_terms_purchases_content".L10N()),
			
			new("about_terms_userconduct".L10N(), FormattedTextType.Header),
			new("about_terms_userconduct_content".L10N()),
			
			new("about_terms_liability".L10N(), FormattedTextType.Header),
			new("about_terms_liability_content".L10N()),
			
			new("about_terms_termsmodifications".L10N(), FormattedTextType.Header),
			new("about_terms_termsmodifications_content".L10N()),
			
			new("about_terms_governinglaw".L10N(), FormattedTextType.Header),
			new("about_terms_governinglaw_content".L10N()),
			
			new("about_terms_contact".L10N(), FormattedTextType.Header),
			new("about_terms_contact_content".L10N(ContactEmail)),
			
			new("about_terms_contact_thanks".L10N(), FormattedTextType.Note),
		];
		
		logger.LogInformation("[HomeViewModel-.ctor] AboutViewModel has been initialized.");
	}
	
	
	public string Version { get; }
	
	public Dependency[] Dependencies { get; }
	
	public FormattedText[] Terms { get; }


	[RelayCommand]
	void CloseAboutInfo() =>
		navigation.Dismiss();

	
	[RelayCommand]
	void ShowDependencySource(
		string url) =>
		launcher.ShowWebpage(url);

	[RelayCommand]
	void ShowContactEmailComposer()
	{
		string body = $"""
		              {"about_contact_type".L10N()}:
		              [{"about_contact_type_description".L10N()}...]
		              
		              {"about_contact_description".L10N()}:
		              [{"about_contact_description_description".L10N()}...]
		              
		              
		              {"about_contact_moreinfo".L10N()}:
		              - {"about_contact_moreinfo_app".L10N(Version)}
		              - {"about_contact_moreinfo_device".L10N(systemInfo.GetDeviceModel())}
		              - {"about_contact_moreinfo_os".L10N(systemInfo.GetOS())}
		              - {"about_contact_moreinfo_battery".L10N(systemInfo.GetBatteryLevel())}
		              """;
		launcher.ShowEmailComposer(ContactEmail, "about_contact_subject".L10N(), body, (pathResolver.CurrentLogFile.Replace("Log-.log", $"Log-{DateTime.Now:yyyyMMdd}.log"), "text/plain"));
	}
}