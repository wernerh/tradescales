(function (app) {
    'use strict';

    app.controller('productAddCtrl', productAddCtrl);

    productAddCtrl.$inject = ['$scope', 'apiService'];

    function productAddCtrl($scope, apiService) {

        $scope.newProduct = {};
        $scope.addProduct = addProduct;

        $scope.submission = {
            successMessages: ['Successfull submission will appear here.'],
            errorMessages: ['Submition errors will appear here.']
        };

        function addProduct() {
            apiService.post('/api/products/add', $scope.newProduct, addProductSucceeded, addProductFailed);
        }

        function addProductSucceeded(response) {
            $scope.submission.errorMessages = ['Submition errors will appear here.'];         
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newProduct.Name + ' has been successfully added');
            $scope.newProduct = {};
        }

        function addProductFailed(response) {
            $scope.submission.errorMessages = response.data;
        }
    }

})(angular.module('tradeScales'));