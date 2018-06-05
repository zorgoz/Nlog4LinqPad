using LINQPad;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace zorgoz.Nlog4LinqPad
{
	[Target("LinqPadHtml")]
	public sealed class LinqPadHtmlTarget : TargetWithLayout
	{
		private const string prefix = "lqph";
		private static readonly Regex extractRenderer = new Regex(@"\$\{(.+?)(:.+?)?}", RegexOptions.Singleline | RegexOptions.Compiled);

		public LinqPadHtmlTarget()
		{
			Layout = "${date:format=HH\\:mm\\:ss}${level}${message}";
		}

		public TargetStyling Styling { get; set; } = TargetStyling.Default;

		public string Prepend { get; set; }
		public string Append { get; set; }
		public string ItemNode { get; set; } = "span";
		public string RowNode { get; set; } = "div";

		private IDictionary<string, SimpleLayout> layouts;
		private string layout;

		public override Layout Layout
		{
			get => layout;
			set
			{
				layout = value.ToString();
				layouts = extractRenderer
					.Matches(layout)
					.Cast<Match>()
					.ToDictionary(x => x.Groups[1].Value, x => new SimpleLayout(x.Value));
			}
		}

		protected override void InitializeTarget()
		{
			Util.RawHtml(GetHtmlStyleTag()).Dump();
		}

		private bool HasRowStyle => !string.IsNullOrWhiteSpace(Styling.Row);
		private bool HasItemStyle => !string.IsNullOrWhiteSpace(Styling.Item);

		private string GetHtmlStyleTag()
		{
			var styleBuilder = new StringBuilder();

			styleBuilder.AppendLine("<style type='text/css'>");

			if (HasRowStyle) styleBuilder.AppendLine($".{prefix}-row {{{Styling.Row}}}");
			if (HasItemStyle) styleBuilder.AppendLine($".{prefix}-item {{{Styling.Item}}}");

			foreach (var c in Styling.styles)
			{
				styleBuilder.AppendLine($".{prefix}-{c.Key.Name} {{{c.Value}}}");
			}
			foreach (var c in Styling.Classes)
			{
				styleBuilder.AppendLine($".{prefix}-{c.Key} {{{c.Value}}}");
			}

			styleBuilder.AppendLine("</style>");

			return styleBuilder.ToString();
		}

		protected override void Write(LogEventInfo logEvent)
		{
			Util.RawHtml(GetLogEventTag(logEvent)).Dump();
		}

		private string GetLogEventTag(LogEventInfo logEvent)
		{
			var tagBuilder = new StringBuilder();
			var classes = new CssClassBuilder();

			if (HasRowStyle) classes.Add($"{prefix}-row");
			classes.Add($"{prefix}-{logEvent.Level.Name}");

			tagBuilder.Append($"<{RowNode} {classes.AsAttribute}>");

			foreach (var l in layouts)
			{
				classes.Clear();
				if (HasItemStyle) classes.Add($"{prefix}-item");
				if (Styling.GetRendererStyle(l.Key, out var _)) classes.Add($"{prefix}-{l.Key}");

				tagBuilder.Append($"<{ItemNode} {classes.AsAttribute}>");
				tagBuilder.Append(l.Value.Render(logEvent));
				tagBuilder.Append($"</{ItemNode}>");
			}

			tagBuilder.AppendLine($"</{RowNode}>");

			return tagBuilder.ToString();
		}
	}

	internal class CssClassBuilder : List<string>
	{
		public string AsAttribute => this.Count > 0 ? $"class='{string.Join(" ", this)}'" : string.Empty;
	}
}
