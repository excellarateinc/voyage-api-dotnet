(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('statusService', StatusService);

    StatusService.$inject = ['$http', '$q'];
    function StatusService($http, $q) {
        var service = {
            getStatus: getStatus
        };
        
        return service;

        ////////////////
        function getStatus() { 
             var deferred = $q.defer();
            $http.get("/api/v2/status")
                .then(function(response){
                    deferred.resolve(response.data);
                },
                function(response){
                    deferred.reject(response.status);
                }
                );
            return deferred.promise;
        }
    }
})();