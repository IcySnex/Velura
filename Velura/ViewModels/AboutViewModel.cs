using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
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
		
		logger.LogInformation("[HomeViewModel-.ctor] AboutViewModel has been initialized.");
	}
	
	
	public string Version { get; }
	
	
	[RelayCommand]
	void CloseAboutInfo() =>
		navigation.Dismiss();
}