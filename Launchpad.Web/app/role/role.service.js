(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('roleService', RoleService);

    RoleService.$inject = ['$http', '$q'];
    function RoleService($http, $q) {
        var service = {
            addRole: addRole,
            getRoles: getRoles,
            addClaim: addClaim,
            removeRole: removeRole,
            removeClaim: removeClaim
        };
        
        return service;

        ////////////////
        function addRole(roleName) { 
            var deferred = $q.defer();

            var role = {
                name: roleName
            };

            $http.post('/api/v1/role', role)
                 .then(function(response){
                     deferred.resolve(true);
                 }, 
                 function(response){
                     deferred.reject(response.data);
                 });

            return deferred.promise;
        }

        function getRoles(){
            var deferred = $q.defer();

            $http.get('/api/v1/role')
                .then(function(response){
                    deferred.resolve(response.data);
                });

            return deferred.promise;
        }

        function removeRole(role){
            var deferred = $q.defer();

            $http.delete('api/v1/role', {params: role})
                .then(function(response){
                    deferred.resolve(true);
                }, 
                function(err){
                    deferred.reject(err.data);
                });
            return deferred.promise;
        }

        function removeClaim(role, claim){
            var deferred = $q.defer();
            var roleClaim = {
                roleName: role.name,
                claimType: claim.claimType,
                claimValue: claim.claimValue
            };
            $http.delete('/api/v1/role/claim', {params: roleClaim})
                .then(function(response){
                    deferred.resolve(response.data);
                },
                function(err){
                    deferred.reject(err.data);
                });

            return deferred.promise;
        }

        function addClaim(role, claimType, claimValue){
            var deferred = $q.defer();
            var roleClaim = {
                role: role,
                claim: {
                    claimType: claimType,
                    claimValue: claimValue
                }
            };

            $http.post('/api/v1/role/claim', roleClaim)
                .then(function(response){
                    deferred.resolve(response.data);
                },
                function(err){
                    deferred.reject(err.data);
                });

            return deferred.promise;
        }
    }
})();