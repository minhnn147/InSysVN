using System.Web;
using System.Web.Optimization;

namespace WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region App_Assets
            bundles.Add(new StyleBundle("~/Content/css/login").Include(
                    "~/Content/css/bootstrap.min.css",
                    "~/Content/css/fontawesome-all.min.css",
                    "~/Content/css/stylelogin.css"
                ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/fontawesome-all.min.css",
                "~/Content/css/nprogress.css",
                "~/Content/css/jquery.mCustomScrollbar.min.css",
                "~/Content/css/custom.min.css",
                "~/Content/css/cropper.min.css",
                "~/Content/css/style.css",
                "~/Content/css/toastr.min.css",
                "~/Content/css/bootstrap-table.min.css",
                "~/Content/css/bootstrap-editable.css",
                "~/Content/css/select2.min.css"
            ));
            bundles.Add(new ScriptBundle("~/Content/js").Include(
                "~/Content/js/jquery.min.js",
                "~/Content/js/define/Define_ShortcutKeyboard.js",
                "~/Content/js/define/Define_CheckCookie.js",
                "~/Content/js/bootstrap.min.js",
                "~/Content/js/bootstrap-datepicker.min.js",
                "~/Content/js/bootstrap-datepicker.vi.min.js",
                "~/Content/js/fastclick.js",
                "~/Content/js/nprogress.js",
                "~/Content/js/jquery.mCustomScrollbar.concat.min.js",
                "~/Content/js/custom.min.js",
                "~/Content/js/toastr.min.js",
                "~/Content/js/moment.min.js",
                "~/Content/js/moment-with-locales.js",
                "~/Content/js/jquery.cookie.js",
                "~/Content/js/extend/toastr.js",
                "~/Content/js/extend/method.js",
                "~/Assets/vendors/jquery-number/jquery.number.js",
                "~/Assets/common/extendDate.js",
                "~/Assets/common/extendNumber.js",
                "~/Assets/common/extendString.js",
                "~/Assets/common/extendJquery.js",
                "~/Assets/common/extendMethods.js",
                "~/Assets/vendors/jquery-number/jquery.number.js",
                "~/Content/js/bootstrap-table.min.js",
                "~/Content/js/bootstrap-table-vi-VN.min.js",
                "~/Content/js/select2.full.min.js",
                "~/Content/js/app.js"
            ));
            bundles.Add(new StyleBundle("~/App_Assets/css").Include(
                     "~/App_Assets/css/bootstrap.css",
                     "~/App_Assets/fonts/feather/style.min.css",
                     "~/App_Assets/fonts/font-awesome/css/font-awesome.min.css",
                     "~/App_Assets/fonts/flag-icon-css/css/flag-icon.min.css",
                     "~/App_Assets/vendors/css/extensions/pace.css",
                     "~/Assets/js/jquery-ui/jquery-ui.min.css"
                     ,
                     "~/App_Assets/vendors/css/forms/icheck/icheck.css",
                     "~/App_Assets/vendors/css/forms/icheck/custom.css",
                     "~/App_Assets/css/bootstrap-extended.css"
                     ,
                     "~/App_Assets/css/app.css",
                     "~/App_Assets/css/colors.css",
                     "~/App_Assets/css/core/menu/menu-types/vertical-menu.css",
                     "~/App_Assets/css/core/menu/menu-types/vertical-overlay-menu.css",
                     "~/App_Assets/css/core/colors/palette-gradient.css",
                     "~/App_Assets/css/pages/login-register.css",
                     "~/App_Assets/css/style.css"
                     ));
            bundles.Add(new ScriptBundle("~/Script/js").Include(
                     "~/App_Assets/vendors/js/vendors.min.js",
                     "~/App_Assets/vendors/js/forms/validation/jqBootstrapValidation.js",
                     "~/App_Assets/vendors/js/forms/icheck/icheck.min.js",
                     "~/App_Assets/js/core/app-menu.js",
                     "~/App_Assets/js/core/app.js",
                     "~/App_Assets/js/core/app-system.js"
                     ));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            #endregion
            BundleTable.EnableOptimizations = false;
            //var combineStatic = Convert.ToBoolean(ConfigurationManager.AppSettings["CombineStatic"]);
            //BundleTable.EnableOptimizations = combineStatic;
        }
    }
}
