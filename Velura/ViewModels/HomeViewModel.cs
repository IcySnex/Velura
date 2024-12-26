using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Services;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class HomeViewModel : ObservableObject
{
	readonly ILogger<HomeViewModel> logger;
	readonly INavigation navigation;
	
	public Config Config { get; }
	public MediaLibrary MediaLibrary { get; }

	public HomeViewModel(
		ILogger<HomeViewModel> logger,
		Config config,
		INavigation navigation,
		MediaLibrary mediaLibrary)
	{
		this.logger = logger;
		this.Config = config;
		this.navigation = navigation;
		this.MediaLibrary = mediaLibrary;
		
		logger.LogInformation("[HomeViewModel-.ctor] HomeViewModel has been initialized.");
	}


	[RelayCommand]
	void ShowMovieInfo(
		Movie movie)
	{
		logger.LogInformation("[HomeViewmodel-ShowMovieInfo] Creating new movie info ViewModel");
		ILogger<MovieInfoViewModel> viewModelLogger = App.Provider.GetRequiredService<ILogger<MovieInfoViewModel>>();
		MovieInfoViewModel viewModel = new(viewModelLogger, movie);
			
		navigation.Push(viewModel, false);
	}

	[RelayCommand]
	void ShowShowInfo(
		Show show)
	{
		logger.LogInformation("[HomeViewmodel-ShowShowInfo] Creating new show info ViewModel");
	}


	[RelayCommand]
	Task RemoveMovieAsync(
		Movie movie) =>
		MediaLibrary.RemoveMovieAsync(movie);
	
	[RelayCommand]
	Task RemoveShowAsync(
		Show show) =>
		MediaLibrary.RemoveShowAsync(show);

	
	[RelayCommand]
	void ShowMediaSection(
		string name)
	{
		void PushMediaSection<TMediaContainer>(
			string sectionName,
			ObservableRangeCollection<TMediaContainer> mediaContainers) where TMediaContainer : IMediaContainer, new()
		{
			logger.LogInformation("[HomeViewmodel-ShowMediaSection] Creating new media section ViewModel");
			ILogger<MediaSectionViewModel<TMediaContainer>> viewModelLogger = App.Provider.GetRequiredService<ILogger<MediaSectionViewModel<TMediaContainer>>>();
			MediaSectionViewModel<TMediaContainer> viewModel = new(viewModelLogger, Config, navigation, MediaLibrary, sectionName, mediaContainers);
			
			navigation.Push(viewModel);
		}
		
		switch (name)
		{
			case nameof(Services.MediaLibrary.Movies):
				PushMediaSection("media_movies".L10N(), MediaLibrary.Movies);
				break;
			case nameof(Services.MediaLibrary.Shows):
				PushMediaSection("media_shows".L10N(), MediaLibrary.Shows);
				break;
			default:
				throw new("This type of media container is not supported.");
		}
	}
}