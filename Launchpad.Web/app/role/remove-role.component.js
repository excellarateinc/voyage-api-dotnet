(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssRemoveRole', {
            templateUrl: '/app/role/remove-role.component.html',
            controller: RoleController,
            controllerAs: 'vm',
            bindings: {
                
            }
        });

    RoleController.$inject = ['roleService'];
    function RoleController(roleService) {
        var vm = this;
        vm.roles = [];
        vm.selectedRole = null;
        vm.save = save;
        vm.refresh = refresh;

        ////////////////

        vm.$onInit = function() {
           vm.refresh();  
         };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function save(){
            roleService.removeRole(vm.selectedRole)
                .then(function(){
                    vm.refresh();
                });
        }

        function refresh(){
            roleService.getRoles()
                .then(function(roles){
                    vm.roles = roles;
                    vm.selectedRole = vm.roles[0];
                    
                });
        }
    }
})();