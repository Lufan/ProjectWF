(function () {
    var app = angular.module('AppOrganizationsManager', ['ui.bootstrap']);
    app.config(['$compileProvider', function ($compileProvider) {
        $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|callto):/);
    }
    ]);
    app.factory('getOrganizationsService', function ($http) {

        var getData = function () {
            return $http({
                method: "GET",
                url: "/api/ApiOrganizations/"
            }).then(function mySucces(response) {
                return response.data["Result"];
            }, function myError(response) {
                return [];
            });
        };
        return { getData: getData };
    });

    app.controller('OrganizationsCtrl', function ($scope, $http, $filter, $uibModal, $log, getOrganizationsService) {

        var ContactPromise = getOrganizationsService.getData();
        ContactPromise.then(function (result) {
            $scope.organizations = result;
        });

        $scope.animationsEnabled = true;
        $scope.selected_organization = { Adress: { Phones: [{ Number: "", Desscription: "" }], Emails: [{ Address: "", Description: "" }], AdressLines: [""] } };

        $scope.openOrganization = function (event) {
            var id = $(event.target).attr("data-id");
            for (var i = 0; i < $scope.organizations.length; i++) {
                if ($scope.organizations[i].Id === id) {
                    $scope.selected_organization = $scope.organizations[i];
                    break;
                }
            }
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'editOrganizationForm.html',
                controller: 'ModalOrganizationCtrl',
                size: "lg",
                resolve: {
                    organization: function () {
                        return $scope.selected_organization;
                    }
                }
            });

            modalInstance.result.then(function (data) {
                $log.info('Modal updated at: ' + $scope.selected_organization.Id + " == " + data.id + " isDeleted = " + data.isDeleted);
                if (data.isDeleted) {
                    $http({
                        method: "DELETE",
                        url: "/api/ApiOrganizations/" + data.id,
                        data: null
                    }).then(function mySucces(response) {
                        $log.info('Succes delete: ' + response);
                        for (var i = 0; i < $scope.organizations.length; i++) {
                            if ($scope.organizations[i].Id === data.id) {
                                $scope.organizations.splice(i, 1);
                                break;
                            }
                        }
                    }, function myError(response) {
                        // TO DO: show message to user about error
                        $log.info('Error delete: ' + response);
                    });
                } else {
                    var saved_organization = JSON.parse(JSON.stringify($scope.selected_organization));
                    $http({
                        method: "POST",
                        url: "/api/ApiOrganizations/",
                        data: $scope.selected_organization
                    }).then(function mySucces(response) {
                        $log.info('Succes post.');
                        isExist = false;
                        if (typeof saved_organization.Id === "undefined") {
                            saved_organization.Id = response.data;
                            $scope.organizations.push(saved_organization);
                        }
                    }, function myError(response) {
                        // TO DO: show message to user about error
                        $log.info('Error post: ' + response);
                    });
                };

                $scope.selected_organization = { Adress: { Phones: [{ Number: "", Desscription: "" }], Emails: [{ Address: "", Description: "" }], AdressLines: [""] } };
            }, function (res) {
                $log.info('Modal dismissed at: ' + new Date() + " " + res);
                $scope.selected_organization = { Adress: { Phones: [{ Number: "", Desscription: "" }], Emails: [{ Address: "", Description: "" }], AdressLines: [""] } };
            });
        };

        $scope.toggleAnimation = function () {
            $scope.animationsEnabled = !$scope.animationsEnabled;
        };
    });
    app.controller('ModalOrganizationCtrl', function ($scope, $uibModalInstance, $http, $filter, $uibModal, $log, organization) {

        $scope.organization = organization;

        $scope.ok = function () {
            $uibModalInstance.close({ "id": organization.Id, isDeleted: false });
            $scope.organization = {}
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
            $scope.organization = {}
        };

        $scope.delete = function () {
            $uibModalInstance.close({ "id": organization.Id, isDeleted: true });
            $scope.organization = {}
        };
    });
})();