(function (app) {
    'use strict';

    app.controller('driverEditCtrl', driverEditCtrl);

    driverEditCtrl.$inject = ['$scope', '$modalInstance', '$timeout', 'apiService', 'notificationService'];

    function driverEditCtrl($scope, $modalInstance, $timeout, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateDriver = updateDriver;

        $scope.openDatePicker = openDatePicker;
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };
        $scope.datepicker = {};

        function updateDriver() {
            console.log($scope.EditedDriver);
            apiService.post('/api/drivers/update/', $scope.EditedDriver,
                updateDriverCompleted,
                updateDriverLoadFailed);
        }

        function updateDriverCompleted(response) {
            notificationService.displaySuccess($scope.EditedDriver.FirstName + ' ' + $scope.EditedDriver.LastName + ' has been updated');
            $scope.EditedDriver = {};
            $modalInstance.dismiss();
        }

        function updateDriverLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }

        function openDatePicker($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $timeout(function () {
                $scope.datepicker.opened = true;
            });

            $timeout(function () {
                $('ul[datepicker-popup-wrap]').css('z-index', '10000');
            }, 100);
        };
    }

})(angular.module('tradeScales'));