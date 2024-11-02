using Microsoft.Extensions.Logging;
using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public class DialogHandler : IDialogHandler
{
	static UIViewController GetHostingController() => IOSApp.MainViewController.PresentedViewController ?? IOSApp.MainViewController;


	readonly ILogger<DialogHandler> logger;

	public DialogHandler(
		ILogger<DialogHandler> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[DialogHandler-.ctor] DialogHandler has been initialized.");
	}
	
	
	public void ShowAlert(
		string title,
		string message,
		string confirmButtonText,
		Action? onConfirm = null)
	{
		logger.LogInformation("[DialogHandler-ShowAlertAsync] Showing alert: {title}...", title);
		UIAlertController alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

		alertController.AddAction(UIAlertAction.Create(confirmButtonText, UIAlertActionStyle.Cancel, onConfirm is null ? null : _ => onConfirm.Invoke()));
		GetHostingController().PresentViewController(alertController, true, null);
	}

	public Task ShowAlertAsync(
		string title,
		string message,
		string confirmButtonText)
	{
		TaskCompletionSource<bool> tcs = new();
		ShowAlert(title, message, confirmButtonText, () => tcs.TrySetResult(true));
		
		return tcs.Task; 
	}

	
	public Task<bool> ShowQuestionAsync(
		string title,
		string message,
		string confirmButtonText,
		string cancelButtonText)
	{
		TaskCompletionSource<bool> tcs = new();
		ShowQuestion(title, message, confirmButtonText, cancelButtonText, () => tcs.TrySetResult(true), () => tcs.TrySetResult(false));

		return tcs.Task;
	}
	
	public void ShowQuestion(
		string title,
		string message,
		string confirmButtonText,
		string cancelButtonText,
		Action? onConfirm = null,
		Action? onCancel = null)
	{
		logger.LogInformation("[DialogHandler-ShowQuestionAsync] Showing question: {title}...", title);
		UIAlertController alertController = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
		
		alertController.AddAction(UIAlertAction.Create(confirmButtonText, UIAlertActionStyle.Default, onConfirm is null ? null : _ => onConfirm.Invoke()));
		alertController.AddAction(UIAlertAction.Create(cancelButtonText, UIAlertActionStyle.Cancel, onCancel is null ? null : _ => onCancel.Invoke()));

		GetHostingController().PresentViewController(alertController, true, null);
	}
}