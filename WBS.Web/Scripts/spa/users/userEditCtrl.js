(function (app) {
    'use strict';

    app.controller('userEditCtrl', userEditCtrl);

    userEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function userEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateUser = updateUser;
   
        function updateUser() {
            console.log($scope.EditedUser);
            apiService.post('/api/users/update/', $scope.EditedUser, updateUserCompleted, updateUserLoadFailed);
        }

        function updateUserCompleted(response) {
            notificationService.displaySuccess($scope.EditedUser.Username + ' has been updated successfully');
            $scope.EditedUser = {};
            $modalInstance.dismiss();
        }

        function updateUserLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }
    }

})(angular.module('tradeScales'));