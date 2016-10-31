(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssAddClaim', {
            templateUrl:'/app/role/add-claim.component.html',
            controller: AddClaimController,
            controllerAs: 'vm',
            bindings: {
            },
        });

    AddClaimController.$inject = ['roleService'];
    function AddClaimController(roleService) {
        var vm = this;
        
        vm.claimType = "";
        vm.claimValue = "";
        vm.roles = [];
        vm.save = save;

        ////////////////

        vm.$onInit = function() { 
            roleService.getRoles()
                .then(function(roles){
                    vm.roles = roles;
                    vm.selectedRole = vm.roles[0];
                });
        };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function save(){

            roleService.addClaim(vm.selectedRole, vm.claimType, vm.claimValue)
                .then(function(data){
                    vm.claimType = '';
                    vm.claimValue = '';
                });
        }
    }
})();