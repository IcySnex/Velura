using CommunityToolkit.Mvvm.ComponentModel;
using MvvmCross.ViewModels;

namespace Velura.ViewModels.Abstract;

public abstract partial class ObservableMvxViewModel : ObservableObject, IMvxViewModel
{
	public virtual void ViewCreated()
	{ }

	public virtual void ViewAppearing()
	{ }

	public virtual void ViewAppeared()
	{ }

	public virtual void ViewDisappearing()
	{ }

	public virtual void ViewDisappeared()
	{ }

	public virtual void ViewDestroy(
		bool viewFinishing = true)
	{ }

	public void Init(
		IMvxBundle parameters) =>
		InitFromBundle(parameters);

	public void ReloadState(
		IMvxBundle state) =>
		ReloadFromBundle(state);

	public virtual void Start()
	{ }

	public void SaveState(
		IMvxBundle state) =>
		SaveStateToBundle(state);

	protected virtual void InitFromBundle(
		IMvxBundle parameters)
	{ }

	protected virtual void ReloadFromBundle(
		IMvxBundle state)
	{ }

	protected virtual void SaveStateToBundle(
		IMvxBundle bundle)
	{ }

	public virtual void Prepare()
	{ }

	public virtual Task Initialize() =>
		Task.FromResult(true);

	[ObservableProperty]
	MvxNotifyTask? initializeTask = null;
}

public abstract class ObservableMvxViewModel<TParameter> : ObservableMvxViewModel, IMvxViewModel<TParameter>
{
	public abstract void Prepare(TParameter parameter);
}