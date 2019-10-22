using System.Web;
using System.Web.Optimization;

namespace EC
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

         
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bundle").Include(
                      "~/Scripts/bundle.js"));

           

            bundles.Add(new ScriptBundle("~/Scripts/multiselect").Include(
                     "~/Scripts/bootstrap-select-dropdown.js"));

           

        bundles.Add(new ScriptBundle("~/Scripts/tagsinput").Include(
                 "~/Scripts/jquery.tagsinput.js"));

            bundles.Add(new ScriptBundle("~/Scripts/multiselect-min").Include(
                 "~/Scripts/bootstrap-select-dropdown.min.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/carousel").Include(
                      "~/Content/carousel.css"));

            bundles.Add(new StyleBundle("~/Content/dashboard").Include(
                      "~/Content/dashboard.css"));

            bundles.Add(new StyleBundle("~/Content/navbar").Include(
                     "~/Content/navbar.css"));

            bundles.Add(new StyleBundle("~/Content/grid").Include(
                    "~/Content/grid.css"));

            bundles.Add(new StyleBundle("~/Content/PagedList").Include(
                   "~/Content/PagedList.css"));

            bundles.Add(new StyleBundle("~/Content/tagsinput").Include(
                   "~/Content/jquery.tagsinput.css"));


        }
    }
}
