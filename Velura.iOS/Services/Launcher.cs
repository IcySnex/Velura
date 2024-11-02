using MessageUI;
using Microsoft.Extensions.Logging;
using SafariServices;
using Velura.Helpers;
using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public class Launcher : ILauncher
{
	static UIViewController GetHostingController() => IOSApp.MainViewController.PresentedViewController ?? IOSApp.MainViewController;
	
	
	readonly ILogger<Launcher> logger;
	readonly IDialogHandler dialogHandler;

	public Launcher(
		ILogger<Launcher> logger,
		IDialogHandler dialogHandler)
	{
		this.logger = logger;
		this.dialogHandler = dialogHandler;
		
		logger.LogInformation("[Launcher-.ctor] Launcher has been initialized.");
	}

	
	public void OpenUrl(
		string url)
	{
		logger.LogInformation("[Launcher-OpenUrl] Opening Url: {url}...", url);
		UIApplication.SharedApplication.OpenUrl(new(url), [], null);
	}


	public void ShowWebpage(
		string url)
	{
		logger.LogInformation("[Launcher-ShowWebpage] Showing webpage : {url}...", url);
		GetHostingController().PresentViewController(new SFSafariViewController(new NSUrl(url))
		{
			ModalPresentationStyle = UIModalPresentationStyle.PageSheet,
			DismissButtonStyle = SFSafariViewControllerDismissButtonStyle.Close,
			PreferredControlTintColor = UIColor.FromName("AccentColor")
		}, true, null);
	}
	
	public void ShowEmailComposer(
		string address,
		string subject,
		string body,
		(string filePath, string mimeType)? attachment = null)
	{
		logger.LogInformation("[Launcher-ShowEmailComposer] Showing email composer: {address}...", address);

		if (!MFMailComposeViewController.CanSendMail)
		{
			logger.LogWarning("[Launcher-ShowEmailComposer] Mail services are not available.");
			dialogHandler.ShowAlert("alert_error".L10N(), "error_nomailservices".L10N(), "alert_confirm".L10N());
			return;
		}

		UIViewController hostingController = GetHostingController();
		
		MFMailComposeViewController mailController = new();
		mailController.SetToRecipients([address]);
		mailController.SetSubject(subject);
		mailController.SetMessageBody(body, false);
		
		if (attachment.HasValue && File.Exists(attachment.Value.filePath))
			mailController.AddAttachmentData(NSData.FromFile(attachment.Value.filePath), attachment.Value.mimeType, Path.GetFileName(attachment.Value.filePath));
		
		mailController.Finished += (_, _) =>
			hostingController.DismissViewController(true, null);
		
		hostingController.PresentViewController(mailController, true, null);
	}
}

