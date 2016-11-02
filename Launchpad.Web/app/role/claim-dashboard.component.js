(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssClaimDashboard', {
            templateUrl: '/app/role/claim-dashboard.component.html',
            controller: ClaimDashboardController,
            controllerAs: 'vm',
            bindings: {
             
            },
        });

    function ClaimDashboardController() {
        var vm = this;
        

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };
    }
})();