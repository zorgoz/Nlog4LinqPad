using LINQPad;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Credits to JoeAlbahari <a href="http://forum.linqpad.net/discussion/1088/dumping-in-different-panel#Comment_2463">on LinqPad forum</a>
/// </summary>

namespace zorgoz.Nlog4LinqPad
{
	public static class LinqPadExtensions
	{
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
				formatter = Util.CreateXhtmlWriter(true);
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

			formatter.WriteLine(toDump);

			if (first || browser.ReadyState == WebBrowserReadyState.Complete)
				browser.DocumentText = formatter.ToString();

			return toDump;
		}
	}
}
