using Microsoft.Extensions.Logging;
using Velura.ViewModels.Abstract;

namespace Velura.ViewModels;

public sealed class HomeViewModel : ObservableMvxViewModel
{
	readonly ILogger<HomeViewModel> logger;

	public HomeViewModel(
		ILogger<HomeViewModel> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[HomeViewModel-.ctor] HomeViewModel has been initialized.");
	}
}