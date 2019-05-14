(function (app) {
    'use strict';

    app.controller('ticketEditCtrl', ticketEditCtrl);

    ticketEditCtrl.$inject = ['$scope', '$location', '$routeParams', '$rootScope', 'apiService', 'notificationService'];

    function ticketEditCtrl($scope, $location, $routeParams, $rootScope, apiService, notificationService) {

        $scope.loadingTicket = true;
        
        $scope.hauliers = [];
        $scope.customers = [];
        $scope.destinations = [];
        $scope.products = [];
        $scope.drivers = [];

        $scope.ticket = {};
        
        $scope.updateTicket = updateTicket;
        $scope.updateNettWeight = updateNettWeight;

        // Load Ticket
        function loadTicket() {
            $scope.loadingTicket = true;
            apiService.get('/api/tickets/details/' + $routeParams.id, null, ticketLoadCompleted, loadFailed);
        }
        function ticketLoadCompleted(response) {
            $scope.ticket = response.data;         
            $scope.loadingTicket = false;
        }
               
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

        // Update Ticket
        function updateTicket() {   
            $scope.ticket.LastModified = new Date();
            $scope.ticket.LastModifiedBy = $rootScope.repository.loggedUser.username;
            $scope.ticket.Status = 'Modified';            
            apiService.post('/api/tickets/update', $scope.ticket, updateTicketSucceeded, updateTicketFailed);
        }
        function updateTicketSucceeded(response) {
            notificationService.displaySuccess('Ticket updated successfully');
            $scope.ticket = {};
            if ($rootScope.previousState)
                $location.path($rootScope.previousState);
            else
                $location.path('/');
        }
        function updateTicketFailed(response) {
            notificationService.displayError(response.data);
        }

        // Update Nett Weight
        function updateNettWeight() {
            $scope.ticket.NettWeight = $scope.ticket.GrossWeight - $scope.ticket.TareWeight;
        }

        loadTicket();     
        loadHauliers();
        loadCustomers();
        loadDestinations();   
        loadProducts();
        loadDrivers();     
    }

})(angular.module('tradeScales'));