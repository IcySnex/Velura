using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Velura.Models;
using Velura.Services;
using Velura.ViewModels;

namespace Velura;

public sealed class App : MvxApplication
{
	public override void Initialize()
	{
		Mvx.IoCProvider.RegisterType<Config>();
		Mvx.IoCProvider.RegisterType<JsonConverter>();

		RegisterAppStart<MainViewModel>();
	}
}