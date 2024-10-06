using MvvmCross.ViewModels;
using Velura.ViewModels;

namespace Velura;

public sealed class App : MvxApplication
{
	public override void Initialize()
	{
		RegisterAppStart<MainViewModel>();
	}
}