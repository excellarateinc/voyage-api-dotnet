(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('errorService', ErrorService);
    
    //This could be an interceptor
    function ErrorService() {
        var service = {
            getModelStateErrors:getModelStateErrors
        };
        
        return service;

        ////////////////
        function getModelStateErrors(failure) { 
            var errors = [];
            if(failure.modelState){
                for(var key in failure.modelState){
                    var states = failure.modelState[key];
                    for(var i = 0; i < states.length; ++i){
                        errors.push(states[i]);
                    }
                }
            }
            return errors;
        }
    }
})();