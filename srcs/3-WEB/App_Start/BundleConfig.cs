using System.Web;
using System.Web.Optimization;

namespace Web
{
   public class BundleConfig
   {
      // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
      public static void RegisterBundles(BundleCollection bundles)
      {

         // JQUERY
         bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery-{version}.js",
                     "~/Scripts/friendship.helpers.js"));
         bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                     "~/Scripts/jquery-ui-{version}.js",
                     "~/Scripts/globalize/globalize.js",
                     "~/Scripts/jquery.jquery.validate.js"));
         bundles.Add(new StyleBundle("~/Content/jQueryCss/files").Include(
                     "~/Content/jQueryCss/jquery.ui.css",
                     "~/Content/jQueryCss/jquery.ui.core.css",
                     "~/Content/jQueryCss/jquery.ui.resizable.css",
                     "~/Content/jQueryCss/jquery.ui.selectable.css",
                     "~/Content/jQueryCss/jquery.ui.accordion.css",
                     "~/Content/jQueryCss/jquery.ui.autocomplete.css",
                     "~/Content/jQueryCss/jquery.ui.button.css",
                     "~/Content/jQueryCss/jquery.ui.dialog.css",
                     "~/Content/jQueryCss/jquery.ui.slider.css",
                     "~/Content/jQueryCss/jquery.ui.tabs.css",
                     "~/Content/jQueryCss/jquery.ui.datepicker.css",
                     "~/Content/jQueryCss/jquery.ui.progressbar.css",
                     "~/Content/jQueryCss/jquery.ui.theme.css"));

         // JQUERY MASK
         bundles.Add(new ScriptBundle("~/bundles/jquerymask").Include(
                     "~/Scripts/jquery.mask.js"
                     //"~/Scripts/jquery.inputmask/jquery.inputmask-{version}.js",
                     //"~/Scripts/jquery.inputmask/jquery.inputmask.extensions-{version}.js",
                     //"~/Scripts/jquery.inputmask/jquery.inputmask.date.extensions-{version}.js",
                     //"~/Scripts/jquery.inputmask/jquery.inputmask.numeric.extensions-{version}.js"
                     ));


         // BOOTSTRAP
         bundles.Add(new StyleBundle("~/Content/bootstrap/css").Include("~/Content/bootstrap/bootstrap.css"));
         bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Content/bootstrap/js/transition.js",
                     "~/Content/bootstrap/js/alert.js",
                     "~/Content/bootstrap/js/button.js",
                     "~/Content/bootstrap/js/carousel.js",
                     "~/Content/bootstrap/js/collapse.js",
                     "~/Content/bootstrap/js/dropdown.js",
                     "~/Content/bootstrap/js/modal.js",
                     "~/Content/bootstrap/js/tooltip.js",
                     "~/Content/bootstrap/js/popover.js",
                     "~/Content/bootstrap/js/scrollspy.js",
                     "~/Content/bootstrap/js/tab.js",
                     "~/Content/bootstrap/js/typeahead.js",
                     "~/Content/bootstrap/js/affix.js"));

         // THEME
         bundles.Add(new StyleBundle("~/Content/themeCss/files").Include(
                     "~/Content/themeCss/theme.css"));

         // OPTIMIZATIONS
         bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                     "~/Scripts/modernizr-*"));
         BundleTable.EnableOptimizations = true;

      }
   }
}
