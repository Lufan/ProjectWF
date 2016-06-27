(function () {
    var app = angular.module('AppContactsManager', ['ui.bootstrap']);
    app.controller('ContactsCtrl', function ($scope, $http, $filter, $uibModal, $log) {
        
        $http({
            method: "GET",
            url: "/api/ApiContacts/"
        }).then(function mySucces(response) {
            $scope.contacts = response.data["Result"];
            $scope.query_result = response.statusText;
        }, function myError(response) {
            $scope.contacts = [];
            $scope.query_result = response.statusText;
        });

        $scope.animationsEnabled = true;

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

            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };

        $scope.openOrganization = function (id) {
            // TO DO - Not implemented yet
            var user = {};
            for (var i = 0; i < $scope.contacts.length; i++) {
                if ($scope.contacts[i].Id === id) {
                    user = $scope.contacts[i];
                    break;
                }
            }
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'editContactForm.html',
                controller: 'ModalInstanceCtrl',
                size: "lg",
                resolve: {
                    user: function () {
                        return user;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                $scope.selected = selectedItem;
            }, function () {
                $log.info('Modal dismissed at: ' + new Date());
            });
        };

        $scope.toggleAnimation = function () {
            $scope.animationsEnabled = !$scope.animationsEnabled;
        };
    });
    app.controller('ModalContactCtrl', function ($scope, $uibModalInstance, contact) {

        $scope.contact = contact;

        $scope.ok = function () {
            $uibModalInstance.close(contact.Id);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });
})();