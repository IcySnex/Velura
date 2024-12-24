using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Velura.Helpers;
using Velura.Models;
using Velura.Models.Abstract;
using Velura.Services;
using Velura.Services.Abstract;

namespace Velura.ViewModels;

public partial class MediaSectionViewModel<TMediaContainer> : ObservableObject where TMediaContainer : IMediaContainer, new()
{
	readonly ILogger<MediaSectionViewModel<TMediaContainer>> logger;
	readonly Database database;
	readonly INavigation navigation;
	readonly IDialogHandler dialogHandler;
	
	public Config Config { get; }
	public ImageCache ImageCache { get; }
	public string SectionName { get; }

	public MediaSectionViewModel(
		ILogger<MediaSectionViewModel<TMediaContainer>> logger,
		Config config,
		Database database,
		ImageCache imageCache,
		INavigation navigation,
		IDialogHandler dialogHandler,
		string sectionName)
	{
		this.logger = logger;
		this.Config = config;
		this.database = database;
		this.ImageCache = imageCache;
		this.navigation = navigation;
		this.dialogHandler = dialogHandler;
		
		SectionName = sectionName;
		
		logger.LogInformation("[MediaSectionViewModel-.ctor] MediaSectionViewModel has been initialized.");
	}
	
	
	[ObservableProperty]
	IReadOnlyList<TMediaContainer>? mediaContainers;


	[RelayCommand]
	async Task ReloadMediaContainersAsync()
	{
		logger.LogInformation("[HomeViewModel-ReloadMediaContainersAsync] Reloading media containers from database...");
		TMediaContainer[] refreshedMediaContainers = await database.GetAsync<TMediaContainer>();

		if (MediaContainers is null || refreshedMediaContainers.Length != MediaContainers.Count)
		{
			MediaContainers = refreshedMediaContainers;
			return;
		}

		for (int i = 0; i < refreshedMediaContainers.Length; i++)
			if (refreshedMediaContainers[i].Id != MediaContainers[i].Id)
			{
				MediaContainers = refreshedMediaContainers;
				return;
			}
	}

	
	[RelayCommand]
	async Task RemoveMediaContainerAsync(
		TMediaContainer mediaContainer)
	{
		if (!await dialogHandler.ShowQuestionAsync("alert_question".L10N(), $"warning_remove_{mediaContainer switch{Movie => "movie", Show => "show", _ => throw new("This type of media container is not supported.") }}".L10N(), "alert_confirm".L10N(), "alert_cancel".L10N()))
		{
			logger.LogInformation("[HomeViewModel-RemoveMediaContainerAsync] Removing media container from database cancelled.");
			return;
		}
		
		logger.LogInformation("[HomeViewModel-RemoveMediaContainerAsync] Removing media container from database...");
		await database.DeleteAsync<TMediaContainer>(mediaContainer.Id);
		
		await ReloadMediaContainersAsync();

		if (MediaContainers!.Count < 1)
		{
			logger.LogInformation("[HomeViewModel-RemoveMediaContainerAsync] No media containers left.");
			navigation.Pop();
		}
	}
}