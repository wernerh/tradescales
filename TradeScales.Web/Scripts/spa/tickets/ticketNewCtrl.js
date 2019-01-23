(function (app) {
    'use strict';

    app.controller('ticketNewCtrl', ticketNewCtrl);

    ticketNewCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService', 'notificationService'];

    function ticketNewCtrl($scope, $location, $rootScope, apiService, notificationService) {
           
        $scope.hauliers = [];
        $scope.customers = [];
        $scope.destinations = [];
        $scope.products = [];
        $scope.drivers = [];  
        
        $scope.CreateTicket = createTicket;
        $scope.UpdateNettWeight = updateNettWeight;
     
        $scope.ticket = {
            Status: 'New Ticket',         
            VehicleId: 1,
            HaulierId: 1,
            CustomerId: 1,
            DestinationId: 1,
            ProductId: 1,
            DriverId: 1,          
            SealFrom: 'Seal From #1',
            SealTo: 'Seal To #1',
            GrossWeight: 0,
            TareWeight: 0
        };

        // Load Ticket Number
        function loadNextTicketNumber() {
            apiService.get('/api/tickets/loadnextticketnumber', null, loadNextTicketNumberCompleted, loadFailed);
        }
        function loadNextTicketNumberCompleted(response) {
            $scope.ticket.TicketNumber = '#' + response.data;
            $scope.ticket.OrderNumber = response.data;
            $scope.ticket.DeliveryNumber = response.data;
        }

        // Load TimeIn
        function loadTimeIn() {
            $scope.ticket.TimeIn = new Date();
            $scope.ticket.TimeOut = new Date();
            $scope.ticket.LastModified = new Date();
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

        // Create Ticket
        function createTicket() {         
            $scope.ticket.LastModifiedBy = $rootScope.repository.loggedUser.username;
            apiService.post('/api/tickets/add', $scope.ticket, createTicketSucceeded, createTicketFailed);
        }
        function createTicketSucceeded(response) {           
            notificationService.displaySuccess('Ticket (' + $scope.ticket.TicketNumber + ') created successfully');
            $scope.ticket = {};
            if ($rootScope.previousState)
                $location.path($rootScope.previousState);
            else
                $location.path('/');
        }
        function createTicketFailed(response) {
            notificationService.displayError(response.data);
        }

        // Update Nett Weight
        function updateNettWeight() {
            $scope.ticket.NettWeight = $scope.ticket.GrossWeight - $scope.ticket.TareWeight;
        }

        loadNextTicketNumber();
        loadTimeIn();
        loadHauliers();
        loadCustomers();
        loadDestinations();   
        loadProducts();
        loadDrivers();
        updateNettWeight();
    }

})(angular.module('tradeScales'));