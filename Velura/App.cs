using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Velura.ViewModels;

namespace Velura;

public class App : MvxApplication
{
	public override void Initialize()
	{
		CreatableTypes()
			.EndingWith("Service")
			.AsInterfaces()
			.RegisterAsLazySingleton();

		RegisterAppStart<MainViewModel>();
	}
}