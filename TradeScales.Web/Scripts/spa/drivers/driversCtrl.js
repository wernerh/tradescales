(function (app) {
    'use strict';

    app.controller('driversCtrl', driversCtrl);

    driversCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function driversCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-customers';
        $scope.loadingDrivers = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Drivers = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;

        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingDrivers = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterDrivers
                }
            };

            apiService.get('/api/drivers/search/', config,
                driversLoadCompleted,
                driversLoadFailed);
        }

        function openEditDialog(driver) {
            $scope.EditedDriver = driver;
            $modal.open({
                templateUrl: 'scripts/spa/drivers/driverEditModal.html',
                controller: 'driverEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function driversLoadCompleted(result) {
            $scope.Drivers = result.data.Items;

            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingDrivers = false;

            if ($scope.filterDrivers && $scope.filterDrivers.length) {
                notificationService.displayInfo(result.data.Items.length + ' drivers found');
            }

        }

        function driversLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterDrivers = '';
            search();
        }

        $scope.search();
    }

})(angular.module('tradeScales'));