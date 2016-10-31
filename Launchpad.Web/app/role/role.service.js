(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('roleService', RoleService);

    RoleService.$inject = ['$http', '$q'];
    function RoleService($http, $q) {
        var service = {
            addRole:addRole
        };
        
        return service;

        ////////////////
        function addRole(roleName) { 
            var deferred = $q.defer();

            var role = {
                name: roleName
            };

            $http.post("/api/role", role)
                 .then(function(response){
                     deferred.resolve(true);
                 }, 
                 function(response){
                     deferred.reject(response.data);
                 });

            return deferred.promise;
        }
    }
})();