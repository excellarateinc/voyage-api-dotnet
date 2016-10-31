(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssAddRole', {
            templateUrl:'/app/role/add-role.component.html',
            controller: AddRoleController,
            controllerAs: 'vm',
            bindings: {
                //Binding: '=',
            },
        });

    AddRoleController.$inject = ['roleService'];
    function AddRoleController(roleService) {
        var vm = this;
        
        vm.roleName = "";
        vm.save = save;

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function save(){
            roleService.addRole(vm.roleName)
                .then(
                  function(data){
                      vm.roleName = "";
                  },
                  function(err){

                  }
                );
        }
    }
})();