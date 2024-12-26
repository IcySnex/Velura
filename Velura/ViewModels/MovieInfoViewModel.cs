using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Velura.Models;

namespace Velura.ViewModels;

public partial class MovieInfoViewModel : ObservableObject
{
	readonly ILogger<MovieInfoViewModel> logger;
	
	public Movie Movie { get; }

	public MovieInfoViewModel(
		ILogger<MovieInfoViewModel> logger,
		Movie movie)
	{
		this.logger = logger;
		this.Movie = movie;
		
		logger.LogInformation("[MovieInfoViewModel-.ctor] MovieInfoViewModel has been initialized.");
	}
}