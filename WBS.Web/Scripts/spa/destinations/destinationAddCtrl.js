(function (app) {
    'use strict';

    app.controller('destinationAddCtrl', destinationAddCtrl);

    destinationAddCtrl.$inject = ['$scope', 'apiService'];

    function destinationAddCtrl($scope, apiService) {

        $scope.newDestination = {};
        $scope.addDestination = addDestination;

        $scope.submission = {
            successMessages: ['Successfull submission will appear here.'],
            errorMessages: ['Submition errors will appear here.']
        };

        function addDestination() {
            apiService.post('/api/destinations/add', $scope.newDestination, addDestinationSucceeded, addDestinationFailed);
        }

        function addDestinationSucceeded(response) {
            $scope.submission.errorMessages = ['Submition errors will appear here.'];
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newDestination.Name + ' has been successfully added');
            $scope.newDestination = {};
        }

        function addDestinationFailed(response) {
            $scope.submission.errorMessages = response.data;
        }
    }

})(angular.module('tradeScales'));