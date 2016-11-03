(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('accountService', AccountService);

    AccountService.$inject = ['$q', '$http', 'authorizationService'];
    function AccountService($q, $http, authorizationService) {
        var service = {
            login:login,
            register: register
           
        };
        
        return service;

        ////////////////
        function login(username, password) { 
            var deferred = $q.defer();
 
            var content = "grant_type=password&username=" + username + "&password=" + password;  
         
            $http.post("/api/Token", content, {
                headers: { 'Content-Type' :  'application/x-www-form-urlencoded'  }
            })
            .success(function(response){
                authorizationService.setToken(response.access_token);
                deferred.resolve(true);
            });
            return deferred.promise;
        }


      

        function register(username, password){
            var deferred = $q.defer();

            var user = {
                email: username,
                password: password,
                confirmPassword: password
            };

            $http.post("/api/account/register", user)
                .then(function(response){
                    deferred.resolve(response.data);
                }, 
                function(response){
                    deferred.reject(response.data);
                });

            return deferred.promise;
        }
    }
})();