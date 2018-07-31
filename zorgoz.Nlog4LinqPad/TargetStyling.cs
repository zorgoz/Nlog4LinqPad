using LINQPad;
using NLog;
using System.Collections.Generic;

namespace zorgoz.Nlog4LinqPad
{
	/// <summary>
	/// Class holds styling information for the WebBrowser control target
	/// </summary>
	public class TargetStyling
	{
		/// <summary>
		/// Default styles both for dark and light theme
		/// </summary>
		public static TargetStyling Default =
			Util.IsDarkThemeEnabled ?
			new TargetStyling
			{
				Row = "font-family: monospace; font-size: 10pt; padding: 2px 0;",
				Item = "padding: 0 10px; display: inline-block;",
				Fatal = "background-color: #33001a; color: white;",
				Error = "background-color: #4d0000; color: white;",
				Warn = "background-color: #662900; color: white;",
				Info = "background-color: lightgray; color: black;",
				Debug = "color: lightgray;",
				Trace = "color: darkgray;",

				Classes = {
				{"date", "width: 100px;"},
				{"level", "width: 70px; text-transform: uppercase; font-weight: bold;"},
				{"exception:empty", "display:none;"},
				{"exception", "display:block; padding: 10px;"}
				}
			}
			: new TargetStyling
			{
				Row = "font-family: monospace; font-size: 10pt; padding: 2px 0;",
				Item = "padding: 0 10px; display: inline-block;",
				Fatal = "background-color: #e6b3e6; color: black;",
				Error = "background-color: #ff9999; color: black;",
				Warn = "background-color: #ffdd99; color: black;",
				Info = "background-color: lightgray; color: black;",
				Debug = "color: black;",
				Trace = "color: darkgray;",

				Classes = {
				{"date", "width: 100px;"},
				{"level", "width: 70px; text-transform: uppercase; font-weight: bold;"},
				{"exception:empty", "display:none;"},
				{"exception", "display:block; padding: 10px;"}
				}
			};

		internal IDictionary<LogLevel, string> styles = new Dictionary<LogLevel, string>();

		/// <summary>
		/// Contains extra classes that can be used in compbination with the log levels
		/// </summary>
		public readonly IDictionary<string, string> Classes = new Dictionary<string, string>();

		/// <summary>
		/// Row style
		/// </summary>
		public string Row { get; set; }

		/// <summary>
		/// Item base style in a row
		/// </summary>
		public string Item { get; set; }

		/// <summary>
		/// Style for fatal error rows
		/// </summary>
		public string Fatal { get => GetStyle(LogLevel.Fatal); set => styles[LogLevel.Fatal] = value; }

		/// <summary>
		/// Style for error rows
		/// </summary>
		public string Error { get => GetStyle(LogLevel.Error); set => styles[LogLevel.Error] = value; }

		/// <summary>
		/// Style for warning rows
		/// </summary>
		public string Warn { get => GetStyle(LogLevel.Warn); set => styles[LogLevel.Warn] = value; }

		/// <summary>
		/// Style for information rows
		/// </summary>
		public string Info { get => GetStyle(LogLevel.Info); set => styles[LogLevel.Info] = value; }

		/// <summary>
		/// Style for debug rows
		/// </summary>
		public string Debug { get => GetStyle(LogLevel.Debug); set => styles[LogLevel.Debug] = value; }

		/// <summary>
		/// Style for trace rows
		/// </summary>
		public string Trace { get => GetStyle(LogLevel.Trace); set => styles[LogLevel.Trace] = value; }

		internal string GetStyle(LogLevel level)
		{
			styles.TryGetValue(level, out var result);
			return result;
		}

		internal bool GetRendererStyle(string renderer, out string result)
		{
			return Classes.TryGetValue(renderer, out result);
		}
	}
}
