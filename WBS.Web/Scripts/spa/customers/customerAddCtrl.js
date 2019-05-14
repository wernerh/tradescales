(function (app) {
    'use strict';

    app.controller('customerAddCtrl', customerAddCtrl);

    customerAddCtrl.$inject = ['$scope', '$location', '$rootScope', 'apiService'];

    function customerAddCtrl($scope, $location, $rootScope, apiService) {

        $scope.newCustomer = {};
        $scope.AddCustomer = AddCustomer;

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

        function AddCustomer() {
            apiService.post('/api/customers/add', $scope.newCustomer, addCustomerSucceded, addCustomerFailed);
        }

        function addCustomerSucceded(response) {
            $scope.submission.errorMessages = ['Submition errors will appear here.'];
            console.log(response);
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newCustomer.Name + ' has been successfully registered');
            $scope.newCustomer = {};
        }

        function addCustomerFailed(response) {
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