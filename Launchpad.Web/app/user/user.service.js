(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('userService', UserService);

    UserService.$inject = ['$q', '$http'];
    function UserService($q, $http) {
        var service = {
            getUsers: getUsers,
            assign: assign,
            revoke: revoke,
            getClaims: getClaims,
            getClaimsMap: getClaimsMap

        };
        
        return service;

        ////////////////
        function getUsers(){
            var deferred = $q.defer();

            $http.get('/api/user')
                .then(function(response){
                    deferred.resolve(response.data);
                });

            return deferred.promise;
        }

        function assign(role, user){
            var deferred = $q.defer();

            var userRole = {
                role: role,
                user: user
            };

            $http.post('/api/user/assign', userRole)
                .then(function(response)
                {
                    deferred.resolve(response.data);
                });
            return deferred.promise;
        }

        function revoke(role, user){
            var deferred = $q.defer();

            var userRole = {
                role: role,
                user: user
            };

            $http.post('/api/user/revoke', userRole)
                .then(function(response)
                {
                    deferred.resolve(response.data);
                });
            return deferred.promise;
         }

        function getClaims(){
            var deferred = $q.defer();

            $http.get('api/user/claims')
                .then(function(response){
                    deferred.resolve(response.data);
                });
            return deferred.promise;
        }

        function getClaimsMap(){
            var deferred = $q.defer();

            getClaims().then(
                function(claims){
                    var map = {};
                    claims.reduce(
                        function(previous, current){
                                previous[current.claimType + "->" + current.claimValue] = true;
                                return previous;
                            }, 
                            map);
                   deferred.resolve(map);
                });   

            return deferred.promise;
        }
        
    }
})();