using Microsoft.Extensions.Logging;
using Velura.ViewModels.Abstract;

namespace Velura.ViewModels;

public sealed class SettingsViewModel : ObservableMvxViewModel
{
	readonly ILogger<SettingsViewModel> logger;

	public SettingsViewModel(
		ILogger<SettingsViewModel> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[SettingsViewModel-.ctor] SettingsViewModel has been initialized.");
	}
}