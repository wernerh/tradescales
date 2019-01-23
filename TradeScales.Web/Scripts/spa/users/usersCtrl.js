(function (app) {
    'use strict';

    app.controller('usersCtrl', usersCtrl);

    usersCtrl.$inject = ['$scope', '$modal', 'apiService', 'notificationService'];

    function usersCtrl($scope, $modal, apiService, notificationService) {

        $scope.pageClass = 'page-users';
        $scope.loadingUsers = true;
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.Users = [];
   
        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.openEditDialog = openEditDialog;

        function search(page) {
            page = page || 0;

            $scope.loadingUsers = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 4,
                    filter: $scope.Users
                }
            };

            apiService.get('/api/users/search/', config, usersLoadCompleted, usersLoadFailed);
        }

        function openEditDialog(user) {
            $scope.EditedUser = user;
            $modal.open({
                templateUrl: 'scripts/spa/users/userEditModal.html',
                controller: 'userEditCtrl',
                scope: $scope
            }).result.then(function ($scope) {
                clearSearch();
            }, function () {
            });
        }

        function usersLoadCompleted(result) {
            $scope.Users = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingUsers = false;

            if ($scope.filterUsers && $scope.filterUsers.length) {
                notificationService.displayInfo(result.data.Items.length + ' users found');
            }
        }

        function usersLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterCompanies = '';
            search();
        }

        $scope.search();
    }

})(angular.module('tradeScales'));