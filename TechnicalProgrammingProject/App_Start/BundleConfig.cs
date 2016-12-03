using System.Web.Optimization;

namespace TechnicalProgrammingProject
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUI").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                    "~/Content/asset/js/datepickerReady.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                     "~/Content/asset/js/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/sidebar").Include(
                     "~/Content/asset/js/leftsidebar.js"));

            bundles.Add(new ScriptBundle("~/bundles/sidebar").Include(
                     "~/Content/asset/js/leftsidebar.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                      "~/Content/themes/base/jquery.ui.core.css",
                      "~/Content/themes/base/jquery.ui.datepicker.css",
                      "~/Content/themes/base/jquery-ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/asset/css/login").Include(
                      "~/Content/asset/css/login.css"));

            bundles.Add(new StyleBundle("~/Content/asset/css/leftsidebar").Include(
                      "~/Content/asset/css/leftsidebar.css"));

            bundles.Add(new StyleBundle("~/Content/asset/css/uploadRecipe").Include(
                      "~/Content/asset/css/uploadRecipe.css"));

        }
    }
}