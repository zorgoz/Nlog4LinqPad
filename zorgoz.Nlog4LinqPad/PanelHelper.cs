using LINQPad;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms;

namespace zorgoz.Nlog4LinqPad
{
	internal static class PanelHelper
	{
		private static readonly Subject<string> queue = new Subject<string>();
		private static IDisposable observer;

		public static void End() => observer.Dispose();

		public static void InitPanel(this string styles, string panelName)
		{
			WebBrowser browser;

			var panel = PanelManager.DisplayControl(browser = new WebBrowser(), panelName);
			browser.DocumentText =
					@"<!DOCTYPE HTML><html><head><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/><meta http-equiv=""X-UA-Compatible"" content=""IE=edge""/>"
					+ styles
					+ "</head><body>";

			observer = queue
				.Buffer(TimeSpan.FromSeconds(1), 200)
				.ObserveOn(browser)
				.Where(x => x.Count > 0)
				.Subscribe(strings =>
			{
				browser.SuspendLayout();
				foreach (var text in strings)
				{
					var e = browser.Document.CreateElement("div");
					e.InnerHtml = text;
					browser.Document.Body.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeEnd, e);
				}
				browser.ResumeLayout();
				browser.Document.Body.ScrollIntoView(false);
			});
		}

		public static T DumpToPanel<T>(this T toDump, string panelName)
		{
			if (string.IsNullOrWhiteSpace(panelName)) return toDump.Dump();

			var panel = PanelManager.GetOutputPanel(panelName);

			var browser = (WebBrowser)(panel.GetControl());

			var text = (toDump is string)
					? toDump.ToString()
					: toDump.GetType().GetMethod("GetContent")?.Invoke(toDump, null).ToString();

			queue.OnNext(text);

			return toDump;
		}
	}
}
