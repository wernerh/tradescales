(function (app) {
    'use strict';

    app.controller('productEditCtrl', productEditCtrl);

    productEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function productEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateProduct = updateProduct;
     
        function updateProduct() {
            console.log($scope.EditedProduct);
            apiService.post('/api/products/update/', $scope.EditedProduct, updateProductCompleted, updateProductLoadFailed);
        }

        function updateProductCompleted(response) {
            notificationService.displaySuccess($scope.EditedProduct.Name + ' has been updated successfully');
            $scope.EditedProduct = {};
            $modalInstance.dismiss();
        }

        function updateProductLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }
    }

})(angular.module('tradeScales'));