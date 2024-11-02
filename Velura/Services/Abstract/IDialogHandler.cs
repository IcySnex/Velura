namespace Velura.Services.Abstract;

public interface IDialogHandler
{
	Task ShowAlertAsync(
		string title,
		string message,
		string confirmButtonText);

	void ShowAlert(
		string title,
		string message,
		string confirmButtonText,
		Action? onConfirm = null);
	
	
	Task<bool>  ShowQuestionAsync(
		string title,
		string message,
		string confirmButtonText,
		string cancelButtonText);
	
	void ShowQuestion(
		string title,
		string message,
		string confirmButtonText,
		string cancelButtonText,
		Action? onConfirm = null,
		Action? onCancel = null);
}