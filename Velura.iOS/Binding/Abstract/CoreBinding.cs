namespace Velura.iOS.Binding.Abstract;

public abstract class CoreBinding(
	UIView target) : IDisposable
{
	public UIView Target { get; protected set; } = target;
	
	
	protected bool IsDisposed = false;

	protected virtual void Dispose(
		bool disposing) =>
		IsDisposed = true;

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
	
	~CoreBinding() =>
		Dispose(false);
}