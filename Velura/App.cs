using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Velura.Models.Abstract;
using Velura.Services;
using Velura.Services.Abstract;
using Velura.ViewModels;

namespace Velura;

public sealed class App : MvxApplication
{
	public override void Initialize()
	{
		Mvx.IoCProvider.ConstructAndRegisterSingleton<IConverter, JsonConverter>();

		
		RegisterAppStart<MainViewModel>();
	}
}