using System.Reflection;
using System.Windows.Input;
using Velura.iOS.Binding.Abstract;
using Velura.iOS.Helpers;

namespace Velura.iOS.Binding;

public sealed class EventBinding : CoreBinding
{
	readonly EventInfo eventInfo;
	
	public EventBinding(
		UIView target,
		string targetEventPath,
		ICommand action) : base(target)
	{
		TargetEventPath = targetEventPath;
		Action = action;
		
		eventInfo = target.GetEvent(targetEventPath);
		eventInfo.AddEventHandler(target, OnTargetEventOccured);
	}
	
	public string TargetEventPath { get; }
	public ICommand Action { get; }


	void OnTargetEventOccured(object? sender, EventArgs e)
	{
		if (!Action.CanExecute(null))
			return;
		
		Action.Execute(null);
	}


	protected override void Dispose(
		bool disposing)
	{
		if (IsDisposed)
			return;
		
		if (disposing)
		{
			// Dispose managed state
			eventInfo.RemoveEventHandler(Target, OnTargetEventOccured);
		}
		
		// Free unmanaged resources/Set large fields to null
		Target = default!;
		
		base.Dispose(disposing);
	}
}