(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssStatusList', {
            //template:'htmlTemplate',
            templateUrl: '/app/status/status-list.component.html',
            controller: StatusListController,
            controllerAs: 'vm',
            bindings: {
                Binding: '=',
            },
        });

    StatusListController.$inject = ['statusService'];
    function StatusListController(statusService) {
        var vm = this;
        vm.statuses = [];

        ////////////////

        vm.$onInit = function() { 
            statusService.getStatus()
                .then(
                    function(statuses){
                        vm.statuses = statuses;
                    },
                    function(error){

                    }
                );
        };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };
    }
})();