(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssRoleList', {
            templateUrl: '/app/role/role-list.component.html',
            controller: RoleListController,
            controllerAs: 'vm',
            bindings: {
            },
        });

    RoleListController.$inject = ['roleService'];
    function RoleListController(roleService) {
        var vm = this;
        vm.roles = [];
       
        vm.refresh = refresh;

        ////////////////

        vm.$onInit = function() {
            vm.refresh();
         };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function refresh(){
              roleService.getRoles()
                .then(function(roles){
                    vm.roles = roles;
                });
        }
    }
})();