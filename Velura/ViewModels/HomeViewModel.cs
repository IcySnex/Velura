using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Models;
using Velura.Services;

namespace Velura.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	readonly ILogger<HomeViewModel> logger;
	readonly Database database;

	public HomeViewModel(
		ILogger<HomeViewModel> logger,
		Database database)
	{
		this.logger = logger;
		this.database = database;

		_ = ReloadDataAsync();
		
		logger.LogInformation("[HomeViewModel-.ctor] HomeViewModel has been initialized.");
	}


	[ObservableProperty]
	Movie[]? movies = null;

	[ObservableProperty]
	Show[]? shows = null;

	public async Task ReloadDataAsync()
	{
		logger.LogInformation("[HomeViewModel-ReloadDataAsync] Reloading data from database...");
		
		Movies = await database.GetAsync<Movie>();
		Shows = await database.GetAsync<Show>();
	}


	[RelayCommand]
	void ShowMediaSection(
		string name)
	{
		logger.LogInformation("[HomeViewModel-ShowMediaSection] Showing media section: {name}", name);
	}
}