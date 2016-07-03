(function () {
    var app = angular.module('AppContactsManager', ['ui.bootstrap']);
    app.config(['$compileProvider', function( $compileProvider )
        {   
            $compileProvider.aHrefSanitizationWhitelist(/^\s*(https?|ftp|mailto|callto):/);
        }
    ]);
    app.factory('getContactsService', function ($http) {

        var getData = function () {
            return $http({
                method: "GET",
                url: "/api/ApiContacts/"
            }).then(function mySucces(response) {
                return response.data["Result"];
            }, function myError(response) {
                return [];
            });
        };
        return { getData: getData };
    });
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

    app.controller('ContactsCtrl', function ($scope, $http, $filter, $uibModal, $log, getContactsService) {

        var ContactPromise = getContactsService.getData();
        ContactPromise.then(function (result) {
            $scope.contacts = result;
        });

        $scope.animationsEnabled = true;
        $scope.selected_contact = { Phones: [{ Number: "", Desscription: "" }], Emails: [{ Address: "", Description: "" }] };

        $scope.openContact = function (event) {
            var id = $(event.target).attr("data-id");
            for (var i = 0; i < $scope.contacts.length; i++) {
                if ($scope.contacts[i].Id === id) {
                    $scope.selected_contact = $scope.contacts[i];
                    break;
                }
            }
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'editContactForm.html',
                controller: 'ModalContactCtrl',
                size: "lg",
                resolve: {
                    contact: function () {
                        return $scope.selected_contact;
                    }
                }
            });

            modalInstance.result.then(function () {
                $log.info('Modal updated at: ' + $scope.selected_contact.Id);
                $http({
                    method: "POST",
                    url: "/api/ApiContacts/",
                    data: $scope.selected_contact
                }).then(function mySucces(response) {
                    $log.info('Succes post: ' + response);
                }, function myError(response) {
                    $log.info('Error post: ' + response);
                });
                $scope.selected_contact = { Phones: [{ Number: "", Desscription: "" }], Emails: [{ Address: "", Description: "" }] };
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
                $scope.selected_contact = { Phones: [{ Number: "", Desscription: "" }], Emails: [{ Address: "", Description: "" }] };
            });
        };
        
        $scope.toggleAnimation = function () {
            $scope.animationsEnabled = !$scope.animationsEnabled;
        };
    });
    app.controller('ModalContactCtrl', function ($scope, $uibModalInstance, $http, $filter, $uibModal, $log, contact) {

        $scope.contact = contact;

        $scope.ok = function () {
            $uibModalInstance.close(contact.Id);
            $scope.contact = {}
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
            $scope.contact = {}
        };

        $scope.selectOrganization = function (event) {
            var id = $(event.target).attr("data-id");
            
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'selectOrganizationForm.html',
                controller: 'ModalOrganizationCtrl',
                size: "md",
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            });

            modalInstance.result.then(function (orgSelected) {
                contact.OrganizationName = orgSelected.OrganizationName;
                contact.OrganizationId = orgSelected.Id;
                $log.info('Result Ok at:' + new Date());
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };
    });
    app.controller('ModalOrganizationCtrl', function ($scope, $uibModalInstance, $log, getOrganizationsService, id) {

        var OrganizationPromise = getOrganizationsService.getData();
        OrganizationPromise.then(function (result) {
            $scope.organizations = result;
            $scope.orgSelected = "";
            for (var i = 0; i < $scope.organizations.length; i++) {
                if ($scope.organizations[i].Id === id) {
                    $scope.resultSelected = $scope.organizations[i];
                    $scope.organizations[i].isSelected = true;
                } else {
                    $scope.organizations[i].isSelected = false;
                }
            }
        });

        $scope.selectOrganization = function (event) {
            $log.info('Event = ' + event.target);
            $log.info('Event data-id = ' + $(event.target).attr("data-id"));
            $log.info('Event checked = ' + $(event.target).attr("checked"));

            var id = $(event.target).attr("data-id");
            for (var i = 0; i < $scope.organizations.length; i++) {
                if ($scope.organizations[i].Id === id) {
                    $scope.resultSelected = $scope.organizations[i];
                    $scope.organizations[i].isSelected = true;
                } else {
                    $scope.organizations[i].isSelected = false;
                }
            }
        };

        $scope.ok = function () {
            $log.info('Result Ok at: org name = ' + $scope.orgSelected.OrganizationName);
            $uibModalInstance.close($scope.resultSelected);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });
})();