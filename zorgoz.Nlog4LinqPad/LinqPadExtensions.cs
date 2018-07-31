using LINQPad;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace zorgoz.Nlog4LinqPad
{
	/// <summary>
	/// Credits to JoeAlbahari <a href="http://forum.linqpad.net/discussion/1088/dumping-in-different-panel#Comment_2463">on LinqPad forum</a>
	/// </summary>
	internal static class LinqPadExtensions
	{
		public class StringBuilderTextWriter : TextWriter
		{
			private readonly StringBuilder stringBuilder = new StringBuilder();

			public override void Write(char value)
			{
				stringBuilder.Append(value);
			}

			public override void Write(string value)
			{
				stringBuilder.AppendLine(value);
			}

			public override void WriteLine(object value)
			{
				stringBuilder.AppendLine(value.ToString());
			}

			public override Encoding Encoding => Encoding.Default;

			public override string ToString() => stringBuilder.ToString();
		}

		public static T DumpToPanel<T>(this T toDump, string panelName)
		{
			if (string.IsNullOrWhiteSpace(panelName)) return toDump.Dump();

			WebBrowser browser;
			TextWriter formatter;

			var panel = PanelManager.GetOutputPanel(panelName);
			bool first = panel == null;
			if (first)
			{
				panel = PanelManager.DisplayControl(browser = new WebBrowser(), panelName);

				formatter = new StringBuilderTextWriter();

				browser.Tag = formatter;
				bool init = true;
				browser.DocumentCompleted += (sender, args) =>
				{
					if (init) browser.DocumentText = formatter.ToString();
					init = false;
				};
			}
			else
			{
				browser = (WebBrowser)(panel.GetControl());
				formatter = (TextWriter)browser.Tag;
			}

			if (toDump is string)
			{
				formatter.WriteLine(toDump);
			}
			else
			{
				var method = toDump.GetType().GetMethod("GetContent");
				formatter.WriteLine(method.Invoke(toDump, null));
			}

			if (first || browser.ReadyState == WebBrowserReadyState.Complete)
				browser.DocumentText = formatter.ToString();

			return toDump;
		}
	}
}
