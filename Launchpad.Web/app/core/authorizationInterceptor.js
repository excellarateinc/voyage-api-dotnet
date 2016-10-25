(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('authorizationInterceptor', AuthorizationInterceptor);


    AuthorizationInterceptor.$inject = ['$q', 'authorizationService', '$state'];
    function AuthorizationInterceptor($q, authorizationService, $state) {
    
        var service = {
            request: request,
            responseError: responseError
        };
        return service;

        ////////////////
        function request(config) { 
            var token = authorizationService.getToken();
            if(token){
            config.headers.Authorization = 'bearer ' + token;
            }
            return config;
        }

        function responseError(response){
            var deferred = $q.defer();
            if(response.status == 401){
                $state.go('login');
            }
            deferred.reject(response);
            return deferred.promise;
        }

    }
})();