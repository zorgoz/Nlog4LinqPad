using NLog;
using NLog.Config;
using NLog.Targets;

namespace zorgoz.Nlog4LinqPad
{
	/// <summary>
	/// Log levels enumeration as NLog levels are instances of a class
	/// </summary>
	public enum LogLevels
	{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
		Trace, Debug, Info, Warn, Error, Fatal
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
	}

	/// <summary>
	/// LinqPad logger utility class
	/// </summary>
	public static class Nlog4LinqPad
	{
		private static readonly LoggingConfiguration config = LogManager.Configuration ?? new LoggingConfiguration();

		/// <summary>
		/// Initializes console logger
		/// </summary>
		/// <param name="minLogLevel">Minimum log level to display</param>
		/// <param name="layout">Layout according to NLog layout definition</param>
		public static void LogToConsoleResults(LogLevels minLogLevel = LogLevels.Trace, string layout = "${date:format=HH\\:mm\\:ss}\t${level:uppercase=true}\t${message}\t${exception:format=tostring}")
		{
			var consoleTarget = new ConsoleTarget { Layout = layout };

			config.AddTarget("console", consoleTarget);
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.FromString(minLogLevel.ToString()), consoleTarget));

			LogManager.Configuration = config;
		}

		/// <summary>
		/// Initializes html logger
		/// </summary>
		/// <param name="minLogLevel">Minimum log level to display</param>
		/// <param name="layout">Layout to use. Note: anything outside <c>${}</c> layout tags is ignored. Each of such a tag will become an item in the result row, decorated with the tag name as class</param>
		/// <param name="styling">Styling instance to use. Default is used if null.</param>
		/// <param name="ownPanelName">If null, the default panel and its presets are used, this causes mixing of the log with any other text dumped. If name is specified, a separate panel is created, which is optimized for this purpose only, thus having far less nodes in the DOM.</param>
		public static void LogToHtmlResults(LogLevels minLogLevel = LogLevels.Trace, string layout = "${date:format=HH\\:mm\\:ss}${level}${message}${exception:format=tostring}", TargetStyling styling = null, string ownPanelName = "Log window")
		{
			var target = new LinqPadHtmlTarget { Layout = layout, Styling = styling ?? TargetStyling.Default, PanelName = ownPanelName };

			config.AddTarget("resultwindow", target);
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.FromString(minLogLevel.ToString()), target));

			LogManager.Configuration = config;
		}
	}
}
