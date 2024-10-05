using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Ios.Core;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;

namespace Velura.iOS;

public class Setup : MvxIosSetup<App>
{
	static string GetLogFilePath()
	{
		string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		string logFolger = Path.Combine(documentsFolder, "Logs", "Log-.txt");

		return logFolger;
	}


	protected override ILoggerProvider? CreateLogProvider() => new SerilogLoggerProvider();

	protected override ILoggerFactory? CreateLogFactory()
	{
		Logger logger = new LoggerConfiguration()
			.MinimumLevel.Information()
			.WriteTo.NSLog()
			.WriteTo.File(GetLogFilePath(), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
			.CreateLogger();

		logger.Information("[Setup-CreateLogFactory] Initilaized Logger Factory.");
		return new SerilogLoggerFactory(logger);
	}
}