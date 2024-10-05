using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Velura.ViewModels;

public partial class MainViewModel : CustomViewModel
{
	readonly ILogger<MainViewModel> logger;
	readonly IMvxNavigationService navigationService;
	
	public MainViewModel(
		ILogger<MainViewModel> logger,
		IMvxNavigationService navigationService)
	{
		this.logger = logger;
		this.navigationService = navigationService;
	}
	
	
	[ObservableProperty]
	string hello = "Hello from MvvmCross";
	
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(ClickText))]
	int clicks = 0;

	public string ClickText => $"Button Clicked {Clicks} Times";
	
	
	[RelayCommand]
	Task Navigate()
	{
		logger.LogInformation("[MainViewModel-Navigate] Navigating to Details");
		
		// return _navigationService.Navigate<DetailViewModel, DetailParameters>(new DetailParameters(Clicks));
		return Task.CompletedTask;
	}

	[RelayCommand]
	void Click()
	{
		logger.LogInformation("[MainViewModel-Click] Clicked {clicks}", Clicks);
		
		Clicks++;
	}
}


public abstract partial class CustomViewModel : ObservableObject, IMvxViewModel
{
	protected CustomViewModel()
	{
	}

	public virtual void ViewCreated()
	{
	}

	public virtual void ViewAppearing()
	{
	}

	public virtual void ViewAppeared()
	{
	}

	public virtual void ViewDisappearing()
	{
	}

	public virtual void ViewDisappeared()
	{
	}

	public virtual void ViewDestroy(bool viewFinishing = true)
	{
	}

	public void Init(IMvxBundle parameters)
	{
		InitFromBundle(parameters);
	}

	public void ReloadState(IMvxBundle state)
	{
		ReloadFromBundle(state);
	}

	public virtual void Start()
	{
	}

	public void SaveState(IMvxBundle state)
	{
		SaveStateToBundle(state);
	}

	protected virtual void InitFromBundle(IMvxBundle parameters)
	{
	}

	protected virtual void ReloadFromBundle(IMvxBundle state)
	{
	}

	protected virtual void SaveStateToBundle(IMvxBundle bundle)
	{
	}

	public virtual void Prepare()
	{
	}

	public virtual Task Initialize()
	{
		return Task.FromResult(true);
	}

	[ObservableProperty]
	MvxNotifyTask? initializeTask = null;
}

public abstract class CustomViewModel<TParameter> : CustomViewModel, IMvxViewModel<TParameter>
{
	public abstract void Prepare(TParameter parameter);
}
