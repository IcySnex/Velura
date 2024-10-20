using System.ComponentModel;

namespace Velura.iOS.Binding.Targets.Abstract;

public interface IBindingTarget
{
	public static readonly Dictionary<Type, IBindingTarget> Implementations = new()
	{
		{ typeof(UISwitch), new UISwitchBindingTarget() },
		{ typeof(UITextField), new UITextFieldBindingTarget() }
	};
	
	
	void Subscribe<TViewModel>(
		Binding<TViewModel> binding) where TViewModel : INotifyPropertyChanged;

	void Unsubscribe<TViewModel>(
		Binding<TViewModel> binding) where TViewModel : INotifyPropertyChanged;
}