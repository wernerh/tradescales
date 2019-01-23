(function (app) {
    'use strict';

    app.controller('destinationsCtrl', destinationsCtrl);

    destinationsCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function destinationsCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-products';
        $scope.loadingDestinations = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Destinations = [];
     
        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingDestinations = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterDestinations
                }
            };

            apiService.get('/api/destinations/search/', config, destinationsLoadCompleted, destinationsLoadFailed);
        }

        function openEditDialog(destination) {
            $scope.EditedDestination = destination;
            $modal.open({
                templateUrl:'scripts/spa/destinations/destinationEditModal.html',
                controller: 'destinationEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function destinationsLoadCompleted(result) {
            $scope.Destinations = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingDestinations = false;

            if ($scope.filterDestinations && $scope.filterDestinations.length) {
                notificationService.displayInfo(result.data.Items.length + ' destinations found');
            }
        }

        function destinationsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterDestinations = '';
            search();
        }

        $scope.search();
    }

})(angular.module('tradeScales'));