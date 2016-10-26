(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('widgetService', WidgetService);

    WidgetService.$inject = ['$http', '$q'];
    function WidgetService($http, $q) {
        var service = {
            getWidgets: getWidgets
        };
        
        return service;

        ////////////////
        function getWidgets() { 
            var deferred = $q.defer();
            $http.get("/api/v2/widget")
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