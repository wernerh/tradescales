(function (app) {
    'use strict';

    app.controller('driverAddCtrl', driverAddCtrl);

    driverAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function driverAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newDriver = {};

        $scope.AddDriver = AddDriver;

        $scope.openDatePicker = openDatePicker;
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};

        $scope.submission = {
            successMessages: ['Successfull submission will appear here.'],
            errorMessages: ['Submition errors will appear here.']
        };

        function AddDriver() {
            apiService.post('/api/drivers/add', $scope.newDriver,
                addDriverSucceded,
                addDriverFailed);
        }

        function addDriverSucceded(response) {
            $scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            var driverAdded = response.data;
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newDriver.LastName + ' has been successfully registered');
            $scope.submission.successMessages.push('Check ' + driverAdded.UniqueKey + ' for reference number');
            $scope.newDriver = {};
        }

        function addDriverFailed(response) {
            console.log(response);
            if (response.status == '400')
                $scope.submission.errorMessages = response.data;
            else
                $scope.submission.errorMessages = response.statusText;
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.datepicker.opened = true;
        };
    }

})(angular.module('tradeScales'));