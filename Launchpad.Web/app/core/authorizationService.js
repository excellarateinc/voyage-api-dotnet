(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('authorizationService', AuthorizationService);

    function AuthorizationService() {
        var accessToken = null;

        var service = {
           setToken : setToken,
           getToken : getToken
        };
        
        return service;

        ////////////////
        function setToken(token) { 
            accessToken = token;
        }
        function getToken() {
            return accessToken;
        }
    }
})();