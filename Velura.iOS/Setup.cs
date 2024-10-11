using Microsoft.Extensions.Logging;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using Velura.iOS.Services;
using Velura.Services.Abstract;

namespace Velura.iOS;

public class Setup : MvxIosSetup<App>
{
	protected override void RegisterPlatformProperties(
		IMvxIoCProvider iocProvider)
	{
		base.RegisterPlatformProperties(iocProvider);
		
		iocProvider.RegisterType<ISimpleStorage, NSUserDefaultsStorage>();
	}

	
	static string GetLogFilePath()
	{
		string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		string logFolger = Path.Combine(documentsFolder, "Logs", "Log-.txt");

		return logFolger;
	}


	protected override ILoggerProvider? CreateLogProvider() => new SerilogLoggerProvider();

	protected override ILoggerFactory? CreateLogFactory()
	{
		const string template = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:l}{NewLine:l}{Exception:l}";
		Logger logger = new LoggerConfiguration()
			.MinimumLevel.Information()
			.WriteTo.NSLog(outputTemplate: template)
			.WriteTo.File(GetLogFilePath(), outputTemplate: template, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
			.CreateLogger();

		logger.Information("[Setup-CreateLogFactory] Initilaized Logger Factory.");
		return new SerilogLoggerFactory(logger);
	}
}