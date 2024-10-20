using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace Velura.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	readonly ILogger<HomeViewModel> logger;

	public HomeViewModel(
		ILogger<HomeViewModel> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[HomeViewModel-.ctor] HomeViewModel has been initialized.");
	}
	
	
	[ObservableProperty]
	string helloText = "Hello World!";

	[RelayCommand]
	void SayHello()
	{
		logger.LogInformation("[HomeViewModel-SayHello] {text}", HelloText);
	}
}