using Microsoft.Extensions.Logging;
using Velura.ViewModels.Abstract;

namespace Velura.ViewModels;

public sealed class SearchViewModel : ObservableMvxViewModel
{
	readonly ILogger<SearchViewModel> logger;

	public SearchViewModel(
		ILogger<SearchViewModel> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[SearchViewModel-.ctor] SearchViewModel has been initialized.");
	}
}