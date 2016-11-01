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
            addClaim: addClaim
        };
        
        return service;

        ////////////////
        function addRole(roleName) { 
            var deferred = $q.defer();

            var role = {
                name: roleName
            };

            $http.post('/api/role', role)
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

            $http.get('/api/role')
                .then(function(response){
                    deferred.resolve(response.data);
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

            $http.post('/api/role/claim', roleClaim)
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