(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssAssignRole', {
            templateUrl: '/app/user/assign-role.component.html',
            controller: AssignRoleController,
            controllerAs: 'vm',
            bindings: {
            },
        });

    var _assignValue = "assign";
    var _revokeValue = "revoke";
    AssignRoleController.$inject = ['roleService', 'userService'];
    function AssignRoleController(roleService, userService) {
        var vm = this;
        
        vm.users = [];
        vm.roles = [];
        vm.assign = assign;
        vm.operation = _assignValue;

        ////////////////

        vm.$onInit = function() { 
            roleService.getRoles()
                .then(function(roles){
                    vm.roles = roles;
                    vm.selectedRole = vm.roles[0];
                });
            userService.getUsers()
                .then(function(users){
                    vm.users = users;
                    vm.selectedUser = vm.users[0];
                });
        };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function assign(){
            if(vm.operation === _assignValue){
            userService.assign(vm.selectedRole, vm.selectedUser);
            }
            else{
                userService.revoke(vm.selectedRole, vm.selectedUser);
            }
        }

    }
})();