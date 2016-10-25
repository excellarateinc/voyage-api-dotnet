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
                .success(function(response){
                    deferred.resolve(response);
                })
                .error(function(error){
                    deferred.reject(error);
                });

            return deferred.promise;
        }
    }
})();