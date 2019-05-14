(function () {
    'use strict';

    angular.module('tradeScales', ['common.core', 'common.ui'])
        .config(config)
        .run(run);

    config.$inject = ['$routeProvider'];
    function config($routeProvider) {
        $routeProvider
            .when("/", {
                templateUrl: "scripts/spa/home/index.html",
                controller: "indexCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/tickets", {
                templateUrl: "scripts/spa/home/index.html",
                controller: "indexCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/tickets/new", {
                templateUrl: "scripts/spa/tickets/ticketNew.html",
                controller: "ticketNewCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/tickets/:id", {
                templateUrl: "scripts/spa/tickets/ticketEdit.html",
                controller: "ticketEditCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/login", {
                templateUrl: "scripts/spa/account/login.html",
                controller: "loginCtrl"
            })
            .when("/register", {
                templateUrl: "scripts/spa/account/register.html",
                controller: "registerCtrl"
            })
            .when("/users", {
                templateUrl: "scripts/spa/users/users.html",
                controller: "usersCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/users/add", {
                templateUrl: "scripts/spa/users/userAdd.html",
                controller: "userAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/customers", {
                templateUrl: "scripts/spa/customers/customers.html",
                controller: "customersCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/customers/add", {
                templateUrl: "scripts/spa/customers/customerAdd.html",
                controller: "customerAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/hauliers", {
                templateUrl: "scripts/spa/hauliers/hauliers.html",
                controller: "hauliersCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/hauliers/add", {
                templateUrl: "scripts/spa/hauliers/haulierAdd.html",
                controller: "haulierAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/drivers", {
                templateUrl: "scripts/spa/drivers/drivers.html",
                controller: "driversCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/drivers/add", {
                templateUrl: "scripts/spa/drivers/driverAdd.html",
                controller: "driverAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/destinations", {
                templateUrl: "scripts/spa/destinations/destinations.html",
                controller: "destinationsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/destinations/add", {
                templateUrl: "scripts/spa/destinations/destinationAdd.html",
                controller: "destinationAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/products", {
                templateUrl: "scripts/spa/products/products.html",
                controller: "productsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/products/add", {
                templateUrl: "scripts/spa/products/productAdd.html",
                controller: "productAddCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .when("/reports", {
                templateUrl: "scripts/spa/reports/reports.html",
                controller: "reportsCtrl",
                resolve: { isAuthenticated: isAuthenticated }
            })
            .otherwise({ redirectTo: "/" });
    }

    run.$inject = ['$rootScope', '$location', '$cookieStore', '$http'];

    function run($rootScope, $location, $cookieStore, $http) {
        // handle page refreshes
        $rootScope.repository = $cookieStore.get('repository') || {};
        if ($rootScope.repository.loggedUser) {
            $http.defaults.headers.common['Authorization'] = $rootScope.repository.loggedUser.authdata;
        }

        $(document).ready(function () {
            $(".fancybox").fancybox({
                openEffect: 'none',
                closeEffect: 'none'
            });

            $('.fancybox-media').fancybox({
                openEffect: 'none',
                closeEffect: 'none',
                helpers: {
                    media: {}
                }
            });

            $('[data-toggle=offcanvas]').click(function () {
                $('.row-offcanvas').toggleClass('active');
            });
        });
    }

    isAuthenticated.$inject = ['membershipService', '$rootScope', '$location'];

    function isAuthenticated(membershipService, $rootScope, $location) {
        if (!membershipService.isUserLoggedIn()) {
            $rootScope.previousState = $location.path();
            $location.path('/login');
        }
    }
})();