(function (app) {
    'use strict';

    app.controller('hauliersCtrl', hauliersCtrl);

    hauliersCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function hauliersCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-products';
        $scope.loadingHauliers = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Hauliers = [];

        $scope.search = search;
        $scope.clearSearch = clearSearch;     
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingHauliers = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterHauliers
                }
            };

            apiService.get('/api/hauliers/search/', config, hauliersLoadCompleted, hauliersLoadFailed);
        }

        function openEditDialog(haulier) {
            $scope.EditedHaulier = haulier;
            $modal.open({
                templateUrl:'scripts/spa/hauliers/haulierEditModal.html',
                controller: 'haulierEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function hauliersLoadCompleted(result) {
            $scope.Hauliers = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingHauliers = false;

            if ($scope.filterHauliers && $scope.filterHauliers.length) {
                notificationService.displayInfo(result.data.Items.length + ' hauliers found');
            }
        }

        function hauliersLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterHauliers = '';
            search();
        }

        $scope.search();
    }

})(angular.module('tradeScales'));