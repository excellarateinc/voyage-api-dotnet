(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssUserList', {
            templateUrl: '/app/user/user-list.component.html',
            controller: UserListController,
            controllerAs: 'vm',
            bindings: {
            },
        });

 
    UserListController.$inject = ['userService'];
    function UserListController(userService) {
        var vm = this;
        
        vm.users = [];
        vm.refresh = refresh;

        ////////////////

        vm.$onInit = function() { 
          vm.refresh();
        };

        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function refresh(){
            userService.getUsersWithRoles()
                .then(function(users){
                    vm.users = users;
                });
        }

    }
})();