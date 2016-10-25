(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssDashboard', {
            //template:'htmlTemplate',
            templateUrl: '/app/dashboard/dashboard.component.html',
            controller: DashboardController,
            controllerAs: 'vm',
            bindings: {
                Binding: '=',
            },
        });

    DashboardController.$inject = ['widgetService'];
    function DashboardController(widgetService) {
        var vm = this;
        //vm.widgets = [];

        ////////////////

        vm.$onInit = function() { 
            widgetService.getWidgets()
                .then(function(response){
                    vm.widgets = response;
                });
        };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };
    }
})();