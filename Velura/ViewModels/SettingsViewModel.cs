using Microsoft.Extensions.Logging;
using Velura.Models.Abstract;
using Velura.ViewModels.Abstract;

namespace Velura.ViewModels;

public sealed class SettingsViewModel : ObservableMvxViewModel
{
	readonly ILogger<SettingsViewModel> logger;
	
	 public IConfig Config { get; }

	public SettingsViewModel(
		ILogger<SettingsViewModel> logger,
		IConfig config)
	{
		this.logger = logger;
		this.Config = config;
		
		logger.LogInformation("[SettingsViewModel-.ctor] SettingsViewModel has been initialized.");
	}
}