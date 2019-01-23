(function (app) {
    'use strict';

    app.controller('haulierEditCtrl', haulierEditCtrl);

    haulierEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function haulierEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateHaulier = updateHaulier;
      
        function updateHaulier() {           
            apiService.post('/api/hauliers/update/', $scope.EditedHaulier, updateHaulierCompleted, updateHaulierFailed);
        }

        function updateHaulierCompleted(response) {
            notificationService.displaySuccess($scope.EditedHaulier.Name + ' has been updated successfully');
            $scope.EditedHaulier = {};
            $modalInstance.dismiss();
        }

        function updateHaulierFailed(response) {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }
    }

})(angular.module('tradeScales'));