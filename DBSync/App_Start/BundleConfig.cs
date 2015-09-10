using System.Web;
using System.Web.Optimization;

namespace DBSync.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                      "~/Content/js/script.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/node_modules/normalize.css",
                      "~/Content/bootstrap/css/bootstrap.css",
                      "~/Content/css/site.css"));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                      "~/Content/css/font-awesome.min.css"));
        }
    }
}