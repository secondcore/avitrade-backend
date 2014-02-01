using System.Web;
using System.Web.Optimization;

namespace Vimba.AviTrade.Web
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			//bundles.Add(new ScriptBundle("~/bundles/avitrade").Include(
			//            "~/Scripts/avitrade-*", "~/Scripts/Extjs/ext-all.js"));

			bundles.Add(new ScriptBundle("~/bundles/avitrade").Include(
						"~/Scripts/avitrade-*"));

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/Libs/jquery-1.*"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
						"~/Scripts/Libs/jquery-ui*"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/Libs/jquery.unobtrusive*",
						"~/Scripts/Libs/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/Libs/modernizr33-*"));

			bundles.Add(new ScriptBundle("~/bundles/flot").Include(
						"~/Scripts/Libs/jquery.flot.js"));

			bundles.Add(new StyleBundle("~/bundles/ext-css").Include("~/Scripts/extjs/resources/css/ext-all.css", "~/Scripts/extjs/ux/grid/css/GridFilters.css"));
			bundles.Add(new ScriptBundle("~/bundles/ext-js").Include("~/Scripts/extjs/ext-all-debug.js"));
			bundles.Add(new ScriptBundle("~/bundles/trader-orders").Include("~/Scripts/trader-orders.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

			bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
						"~/Content/themes/base/jquery.ui.core.css",
						"~/Content/themes/base/jquery.ui.resizable.css",
						"~/Content/themes/base/jquery.ui.selectable.css",
						"~/Content/themes/base/jquery.ui.accordion.css",
						"~/Content/themes/base/jquery.ui.autocomplete.css",
						"~/Content/themes/base/jquery.ui.button.css",
						"~/Content/themes/base/jquery.ui.dialog.css",
						"~/Content/themes/base/jquery.ui.slider.css",
						"~/Content/themes/base/jquery.ui.tabs.css",
						"~/Content/themes/base/jquery.ui.datepicker.css",
						"~/Content/themes/base/jquery.ui.progressbar.css",
						"~/Content/themes/base/jquery.ui.theme.css"));
		}
	}
}