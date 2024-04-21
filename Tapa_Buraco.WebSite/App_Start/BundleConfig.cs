using System.Web;
using System.Web.Optimization;

namespace Tapa_Buraco.WebSite
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use a versão em desenvolvimento do Modernizr para desenvolver e aprender com ela. Após isso, quando você estiver
            // pronto para a produção, utilize a ferramenta de build em https://modernizr.com para escolher somente os testes que precisa.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/project.core").Include(
                        "~/Scripts/site/core.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                            "~/Scripts/jquery-1.10.2.js",
                            "~/Scripts/jquery-migrate-1.2.1.js",
                            "~/Scripts/jquery-ui-1.10.4.js",
                            "~/Scripts/jquery.validationEngine-pt_BR.js",
                            "~/Scripts/jquery.validationEngine.js",
                            "~/Scripts/jtable/jquery.jtable.js",
                            "~/Scripts/jquery.maskedinput-1.3.1.js",
                            "~/Scripts/jquery.smartmenus.js",
                            "~/Scripts/noty/jquery.noty.js",
                            "~/Scripts/noty/layouts/center.js",
                            "~/Scripts/noty/layouts/topCenter.js",
                            "~/Scripts/noty/themes/default.js",
                            "~/Scripts/jquery.datetime.js",
                            //"~/Scripts/moment.js",
                            "~/Scripts/jquery.validate.js",
                            "~/Scripts/master.framework.js",
                            "~/Scripts/jquery.cascadingdropdown.js",
                            "~/Scripts/jquery.tablednd.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                "~/Scripts/jquery.form.js",
                "~/Scripts/master.framework.upload.js"
            ));


            bundles.Add(new ScriptBundle("~/bundles/ajax").Include(
                "~/Scripts/jquery.ajaxmanager.js",
                "~/Scripts/master.framework.ajax.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                //"~/Content/themes/metroblue/jquery-ui.css",
                "~/Scripts/jtable/themes/metro/darkgray/jtable.css",
                "~/Scripts/jtable/themes/jqueryui/jtable_jqueryui.css",
                "~/Content/default.css",
                "~/Content/loading.css",
                "~/Content/sm-core-css.css",
                "~/Content/themes/sm-clean/sm-clean.css",
                "~/Content/validationEngine.jquery.css",
                "~/Content/themes/overcast/jquery-ui.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-route.js",
                        "~/Scripts/angular-ui/ui-bootstrap.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.js"));
        }
    }
}
