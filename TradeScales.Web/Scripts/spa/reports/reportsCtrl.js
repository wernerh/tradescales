(function (app) {
    'use strict';

    app.controller('reportsCtrl', reportsCtrl);

    reportsCtrl.$inject = ['$scope', '$rootScope', 'apiService', 'notificationService'];

    function reportsCtrl($scope, $rootScope, apiService, notificationService) {

        $scope.hauliers = [];
        $scope.customers = [];
        $scope.destinations = [];
        $scope.products = [];
        $scope.drivers = [];

        $scope.generateReport = generateReport;
   
        $scope.filter = {};

        // Load Hauliers
        function loadHauliers() {
            apiService.get('/api/hauliers/', null, loadHauliersCompleted, loadFailed);
        }
        function loadHauliersCompleted(response) {
            $scope.hauliers = response.data;
        }

        // Load Customers
        function loadCustomers() {
            apiService.get('/api/customers/', null, loadCustomersCompleted, loadFailed);
        }
        function loadCustomersCompleted(response) {
            $scope.customers = response.data;
        }

        // Load Destinations
        function loadDestinations() {
            apiService.get('/api/destinations/', null, loadDestinationsCompleted, loadFailed);
        }
        function loadDestinationsCompleted(response) {
            $scope.destinations = response.data;
        }

        // Products
        function loadProducts() {
            apiService.get('/api/products/', null, loadProductsCompleted, loadFailed);
        }
        function loadProductsCompleted(response) {
            $scope.products = response.data;
        }

        // Load Drivers
        function loadDrivers() {
            apiService.get('/api/drivers/', null, loadDriversCompleted, loadFailed);
        }
        function loadDriversCompleted(response) {
            $scope.drivers = response.data;
        }

        // Load Failed
        function loadFailed(response) {
            notificationService.displayError(response.data);
        }

        // Generate Report
        function generateReport() {
            alert('Generate Report');
        }
        function generateReportSucceeded(response) {
            notificationService.displaySuccess('Report generated successfully');
            $scope.ticket = {};
        }
        function generateReportFailed(response) {
            notificationService.displayError(response.data);
        }
         
        loadHauliers();
        loadCustomers();
        loadDestinations();
        loadProducts();
        loadDrivers();      
    }

})(angular.module('tradeScales'));