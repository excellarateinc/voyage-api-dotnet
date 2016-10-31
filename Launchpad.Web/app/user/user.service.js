(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('userService', UserService);

    UserService.$inject = ['$q', '$http'];
    function UserService($q, $http) {
        var service = {
            getUsers: getUsers,
            assign: assign
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
        
    }
})();