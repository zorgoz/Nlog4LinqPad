using NLog;
using NLog.Config;
using NLog.Targets;

namespace zorgoz.Nlog4LinqPad
{
	public static class Nlog4LinqPad
	{
		private static LoggingConfiguration config = LogManager.Configuration ?? new LoggingConfiguration();

		public static void LogToConsoleResults(string layout = "${date:format=HH\\:mm\\:ss}\t${level:uppercase=true}\t${message}\t${exception:format=tostring}")
		{
			var consoleTarget = new ConsoleTarget { Layout = layout };

			config.AddTarget("console", consoleTarget);
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, consoleTarget));

			LogManager.Configuration = config;
		}

		public static void LogToHtmlResults(string layout = "${date:format=HH\\:mm\\:ss}${level}${message}${exception:format=tostring}", TargetStyling styling = null)
		{
			var target = new LinqPadHtmlTarget { Layout = layout, Styling = styling ?? TargetStyling.Default };

			config.AddTarget("resultwindow", target);
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));

			LogManager.Configuration = config;
		}
	}
}
