using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Models;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class MovieInfoViewModel : ObservableObject
{
	readonly ILogger<MovieInfoViewModel> logger;
	readonly INavigation navigation;
	
	public Movie Movie { get; }

	public MovieInfoViewModel(
		ILogger<MovieInfoViewModel> logger,
		INavigation navigation,
		Movie movie)
	{
		this.logger = logger;
		this.navigation = navigation;
		this.Movie = movie;
		
		logger.LogInformation("[MovieInfoViewModel-.ctor] MovieInfoViewModel has been initialized.");
	}


	[RelayCommand]
	void Close() =>
		navigation.Dismiss();
}