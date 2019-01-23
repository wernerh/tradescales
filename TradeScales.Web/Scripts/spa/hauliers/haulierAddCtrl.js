(function (app) {
    'use strict';

    app.controller('haulierAddCtrl', haulierAddCtrl);

    haulierAddCtrl.$inject = ['$scope', 'apiService'];

    function haulierAddCtrl($scope, apiService) {

        $scope.newHaulier = {};
        $scope.addHaulier = addHaulier;

        $scope.submission = {
            successMessages: ['Successfull submission will appear here.'],
            errorMessages: ['Submition errors will appear here.']
        };

        function addHaulier() {
            apiService.post('/api/hauliers/add', $scope.newHaulier, addHaulierSucceeded, addHaulierFailed);
        }

        function addHaulierSucceeded(response) {
            $scope.submission.errorMessages = ['Submition errors will appear here.'];
            $scope.submission.successMessages = [];
            $scope.submission.successMessages.push($scope.newHaulier.Name + ' has been successfully added');
            $scope.newHaulier = {};
        }

        function addHaulierFailed(response) {
            $scope.submission.errorMessages = response.data;
        }
    }

})(angular.module('tradeScales'));