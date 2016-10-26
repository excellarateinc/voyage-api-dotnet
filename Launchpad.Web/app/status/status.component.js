(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssStatus', {
            //template:'htmlTemplate',
            templateUrl: '/app/status/status.component.html',
            controller: StatusController,
            controllerAs: 'vm',
            bindings: {
                status: '=',
            },
        });

    function StatusController() {
        var vm = this;
        

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };
    }
})();