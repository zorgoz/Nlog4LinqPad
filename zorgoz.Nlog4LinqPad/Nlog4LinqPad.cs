using NLog;
using NLog.Config;
using NLog.Targets;

namespace zorgoz.Nlog4LinqPad
{
	public enum LogLevels
	{
		Trace, Debug, Info, Warn, Error, Fatal
	}

	public static class Nlog4LinqPad
	{
		private static readonly LoggingConfiguration config = LogManager.Configuration ?? new LoggingConfiguration();

		public static void LogToConsoleResults(LogLevels minLogLevel = LogLevels.Trace, string layout = "${date:format=HH\\:mm\\:ss}\t${level:uppercase=true}\t${message}\t${exception:format=tostring}")
		{
			var consoleTarget = new ConsoleTarget { Layout = layout };

			config.AddTarget("console", consoleTarget);
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.FromString(minLogLevel.ToString()), consoleTarget));

			LogManager.Configuration = config;
		}

		public static void LogToHtmlResults(LogLevels minLogLevel = LogLevels.Trace, string layout = "${date:format=HH\\:mm\\:ss}${level}${message}${exception:format=tostring}", TargetStyling styling = null, string ownPanelName = "Log window")
		{
			var target = new LinqPadHtmlTarget { Layout = layout, Styling = styling ?? TargetStyling.Default, PanelName = ownPanelName };

			config.AddTarget("resultwindow", target);
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.FromString(minLogLevel.ToString()), target));

			LogManager.Configuration = config;
		}
	}
}
