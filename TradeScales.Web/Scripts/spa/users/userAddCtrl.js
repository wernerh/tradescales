(function (app) {
    'use strict';

    app.controller('userAddCtrl', userAddCtrl);

    userAddCtrl.$inject = ['$scope', 'membershipService'];

    function userAddCtrl($scope, membershipService) {

        $scope.newUser = {};
        $scope.addUser = addUser;

        $scope.submission = {
            successMessages: ['Successfull submission will appear here.'],
            errorMessages: ['Submition errors will appear here.']
        };

        function addUser() {
            membershipService.register($scope.newUser, addUserSucceded);
        }

        function addUserSucceded(response) {
            $scope.submission.errorMessages = ['Submition errors will appear here.'];
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newUser.Username + ' has been successfully registered');
            $scope.newUser = {};
        }      
    }

})(angular.module('tradeScales'));