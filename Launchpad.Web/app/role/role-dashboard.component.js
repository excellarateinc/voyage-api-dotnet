(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssRoleDashboard', {
            templateUrl: '/app/role/role-dashboard.component.html',
            controller: RoleDashboardController,
            controllerAs: 'vm',
            bindings: {
              
            },
        });

 
    function RoleDashboardController() {
        var vm = this;
        

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };
    }
})();