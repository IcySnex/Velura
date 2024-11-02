namespace Velura.Services.Abstract;

public interface ILauncher
{
	void OpenUrl(
		string url);
	
	
	void ShowWebpage(
		string url);
	
	void ShowEmailComposer(
		string address,
		string subject,
		string body,
		(string filePath, string mimeType)? attachment = null);
}