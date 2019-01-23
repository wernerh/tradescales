(function (app) {
    'use strict';

    app.controller('indexCtrl', indexCtrl);

    indexCtrl.$inject = ['$scope', 'apiService', 'notificationService'];

    function indexCtrl($scope, apiService, notificationService) {
        $scope.pageClass = 'page-home';
        $scope.loadingTickets = true;
        $scope.loadingUpdates = true;
        $scope.hasStatusMessages = false;
        $scope.isReadOnly = true;

        $scope.latestTickets = [];
        $scope.statusMessages = [];
        $scope.loadData = loadData;
        $scope.clearSearch = clearSearch;
        $scope.generatePdf = generatePdf;
        $scope.getStatusColor = getStatusColor;
        $scope.getButtonDescription = getButtonDescription;
            
        function loadData() {
            apiService.get('/api/statusmessages', null, statusMessagesLoadCompleted, loadFailed);
            apiService.get('/api/tickets', null, ticketsLoadCompleted, loadFailed);
        }

        function ticketsLoadCompleted(response) {
            $scope.latestTickets = response.data;
            $scope.loadingTickets = false;
        }

        function statusMessagesLoadCompleted(response) {
            $scope.statusMessages = response.data;        
            $scope.hasStatusMessages = true;         
        }

        function loadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterTickets = '';
        }

        function generatePdf(ticketId) {
            apiService.post('/api/tickets/generatePdf/' + ticketId, null, generatePdfSucceeded, generatePdfFailed);
        }

        function generatePdfSucceeded(response) {
            downloadPdf(response.data);
            notificationService.displaySuccess('Ticket downloaded successfully');
            loadData();
        }

        function generatePdfFailed(response) {
            console.log(response.data);
            notificationService.displayError(response.data);
        }

        function getStatusColor(status) {
            if (status === 'New Ticket') {
                return 'red';
            }
            if (status === 'Modified') {
                return 'yellow';
            }
            if (status === 'Complete') {
                return 'green';
            }
            return 'red';
        }

        function getButtonDescription(status) {
            if (status === 'Complete') {
                return 'View';
            }
            return 'Generate';
        }

        function downloadPdf(downloadPath) {
            window.open(downloadPath, '_blank', '');
        }

        loadData();
    }

})(angular.module('tradeScales'));