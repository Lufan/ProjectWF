(function () {
    var app = angular.module('AppContactsManager', []);
    app.controller('ContactsCtrl', function ($scope, $http, $filter) {
        
        $http({
            method: "GET",
            url: "/api/contacts"
        }).then(function mySucces(response) {
            $scope.contacts = response.data;
            $scope.query_result = response.statusText;
        }, function myError(response) {
            $scope.contacts = [];
            $scope.query_result = response.statusText;
        });

    });
})();