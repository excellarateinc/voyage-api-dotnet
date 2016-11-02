(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssRemoveClaim', {
            templateUrl: '/app/role/remove-claim.component.html',
            controller: RemoveClaimController,
            controllerAs: 'vm',
            bindings: {
            },
        });

    RemoveClaimController.$inject = ['roleService'];
    function RemoveClaimController(roleService) {
        var vm = this;
        vm.roles = [];
        vm.selectedRole = null;
        vm.refresh = refresh;
        vm.save = save;

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
                    vm.selectedRole = vm.roles[0];
                    vm.selectedClaim = null;
                });
        }

        function save(){
            if(vm.selectedClaim){
            roleService.removeClaim(vm.selectedRole, vm.selectedClaim)
                .then(function(){
                    vm.refresh();
                });
            }
        }
        

    }
})();