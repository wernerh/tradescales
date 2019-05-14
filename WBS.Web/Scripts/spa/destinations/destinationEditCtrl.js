(function (app) {
    'use strict';

    app.controller('destinationEditCtrl', destinationEditCtrl);

    destinationEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function destinationEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateDestination = updateDestination;
     
        function updateDestination() {
            apiService.post('/api/destinations/update/', $scope.EditedDestination, updateDestinationCompleted, updateDestinationFailed);
        }

        function updateDestinationCompleted(response) {
            notificationService.displaySuccess($scope.EditedDestination.Name + ' has been updated successfully');
            $scope.EditedDestination = {};
            $modalInstance.dismiss();
        }

        function updateDestinationFailed(response) {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }
    }

})(angular.module('tradeScales'));