using System.Web.Optimization;

namespace WBS.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/Vendors/modernizr.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
                "~/Scripts/Vendors/jquery.js",
                "~/Scripts/Vendors/bootstrap.js",
                "~/Scripts/Vendors/toastr.js",
                "~/Scripts/Vendors/jquery.raty.js",
                "~/Scripts/Vendors/respond.src.js",
                "~/Scripts/Vendors/angular.js",
                "~/Scripts/Vendors/angular-route.js",
                "~/Scripts/Vendors/angular-cookies.js",
                "~/Scripts/Vendors/angular-validator.js",
                "~/Scripts/Vendors/angular-base64.js",
                "~/Scripts/Vendors/angular-file-upload.js",
                "~/Scripts/Vendors/angucomplete-alt.min.js",
                "~/Scripts/Vendors/ui-bootstrap-tpls-0.13.1.js",
                "~/Scripts/Vendors/underscore.js",
                "~/Scripts/Vendors/raphael.js",
                "~/Scripts/Vendors/morris.js",
                "~/Scripts/Vendors/jquery.fancybox.js",
                "~/Scripts/Vendors/jquery.fancybox-media.js",
                "~/Scripts/Vendors/loading-bar.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/spa").Include(
                "~/Scripts/spa/modules/common.core.js",
                "~/Scripts/spa/modules/common.ui.js",
                "~/Scripts/spa/app.js",
                "~/Scripts/spa/services/apiService.js",
                "~/Scripts/spa/services/notificationService.js",
                "~/Scripts/spa/services/membershipService.js",
                "~/Scripts/spa/services/fileUploadService.js",
                "~/Scripts/spa/layout/topBar.directive.js",
                "~/Scripts/spa/layout/sideBar.directive.js",
                "~/Scripts/spa/layout/customPager.directive.js",    
                "~/Scripts/spa/account/loginCtrl.js",
                "~/Scripts/spa/account/registerCtrl.js",

                //Home
                "~/Scripts/spa/home/rootCtrl.js",
                "~/Scripts/spa/home/indexCtrl.js",

                //Tickets
                "~/Scripts/spa/tickets/ticketNewCtrl.js",
                "~/Scripts/spa/tickets/ticketEditCtrl.js",

                //Users
                "~/Scripts/spa/users/usersCtrl.js",
                "~/Scripts/spa/users/userAddCtrl.js",
                "~/Scripts/spa/users/userEditCtrl.js",

                //Companies
                "~/Scripts/spa/customers/customersCtrl.js",
                "~/Scripts/spa/customers/customerAddCtrl.js",
                "~/Scripts/spa/customers/customerEditCtrl.js",

                //Hauliers
                "~/Scripts/spa/hauliers/hauliersCtrl.js",
                "~/Scripts/spa/hauliers/haulierAddCtrl.js",
                "~/Scripts/spa/hauliers/haulierEditCtrl.js",

                //Drivers
                "~/Scripts/spa/drivers/driversCtrl.js",
                "~/Scripts/spa/drivers/driverAddCtrl.js",
                "~/Scripts/spa/drivers/driverEditCtrl.js",

                //Destinations
                "~/Scripts/spa/destinations/destinationsCtrl.js",
                "~/Scripts/spa/destinations/destinationAddCtrl.js",
                "~/Scripts/spa/destinations/destinationEditCtrl.js",

                //Products
                "~/Scripts/spa/products/productsCtrl.js",
                "~/Scripts/spa/products/productAddCtrl.js",
                "~/Scripts/spa/products/productEditCtrl.js",

                //Reports
                "~/Scripts/spa/reports/reportsCtrl.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/content/css/site.css",
                "~/content/css/bootstrap.css",
                "~/content/css/bootstrap-theme.css",
                "~/content/css/font-awesome.css",
                "~/content/css/morris.css",
                "~/content/css/toastr.css",
                "~/content/css/jquery.fancybox.css",
                "~/content/css/loading-bar.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}