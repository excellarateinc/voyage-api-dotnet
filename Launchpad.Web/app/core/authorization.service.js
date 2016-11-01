(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('authorizationService', AuthorizationService);

    AuthorizationService.$inject = ['$q'];
    function AuthorizationService($q) {
        var _accessToken = null;
        var _claimsMap = {};

        var service = {
           setToken : setToken,
           getToken : getToken,
           hasClaim : hasClaim,
           setClaims: setClaims
        };
        
        return service;

        function setClaims(claimsMap){
            _claimsMap = claimsMap;
        }


        function hasClaim(claimType, claimValue){
            var hasClaim = _claimsMap[claimType + "->" + claimValue] === true;       
            return hasClaim;
        }

        function setToken(token) { 
            _accessToken = token;
            if(_accessToken === null){
               
                _claimsMap = {};
            }
        }

        function getToken() {
            return _accessToken;
        }
    }
})();