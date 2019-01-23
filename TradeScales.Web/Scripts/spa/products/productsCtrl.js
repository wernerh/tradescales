(function (app) {
    'use strict';

    app.controller('productsCtrl', productsCtrl);

    productsCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function productsCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-products';
        $scope.loadingProducts = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Products = [];
 
        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingProducts = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.filterProducts
                }
            };

            apiService.get('/api/products/search/', config, productsLoadCompleted, productsLoadFailed);
        }

        function openEditDialog(product) {
            $scope.EditedProduct = product;
            $modal.open({
                templateUrl:'scripts/spa/products/productEditModal.html',
                controller: 'productEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function productsLoadCompleted(result) {
            $scope.Products = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingProducts = false;

            if ($scope.filterProducts && $scope.filterProducts.length) {
                notificationService.displayInfo(result.data.Items.length + ' products found');
            }
        }

        function productsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterProducts = '';
            search();
        }

        $scope.search();
    }

})(angular.module('tradeScales'));