(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssLogin', {
            //template:'login.component.html',
            templateUrl: '/app/account/login.component.html',
            controller: LoginController,
            controllerAs: 'vm',
            bindings: {
                Binding: '=',
            },
        });

    LoginController.$inject = ['accountService', '$state'];
    function LoginController(accountService, $state) {
        var vm = this;
        
        vm.greeting = 'Hello and Welcome!';
        vm.login = login;
        vm.username = 'fred@fred.com';
        vm.password = 'Hello123!';
        
        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function login(){
            accountService.login(vm.username, vm.password)
            .then(function(result){
                $state.go('dashboard');
            });
        }
    }
})();