using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;

namespace Velura.ViewModels;

public partial class SearchViewModel : ObservableObject
{
	readonly ILogger<SearchViewModel> logger;

	public SearchViewModel(
		ILogger<SearchViewModel> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[SearchViewModel-.ctor] SearchViewModel has been initialized.");
	}
	
	
	[ObservableProperty]
	string helloText = "Hello World!";

	[RelayCommand]
	void SayHello(
		string newText)
	{
		HelloText = newText;
		logger.LogInformation("[SearchViewModel-SayHello] {text}", HelloText);
	}
}